using CsvHelper;
using CsvHelper.Configuration;
using LifeCycle.DataAccess.Repository.IRepository;
using LifeCycle.DataObjects;
using LifeCycle.Models;
using LifeCycle.Models.DataObjects;
using LifeCycle.Services.IServices;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using static LifeCycle.Services.IServices.ICsvImportService;

namespace LifeCycle.Services
{
    public class CSVImportService : ICsvImportService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CSVImportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<ImportResult> ImportCsvDataAsync(Stream csvStream)
        {

            var result = new ImportResult();
            var importedCount = 0;

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                using var reader = new StreamReader(csvStream);
                using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";",
                    HeaderValidated = null,
                    MissingFieldFound = null
                });


                csv.Context.RegisterClassMap<CsvMappingDO>();
                var records = csv.GetRecords<CsvImportDO>().ToList();

                if (!records.Any())
                {
                    result.ErrorMessages.Add("No records found in the CSV file.");
                    return result;
                }

                foreach (var record in records)
                {
                    try
                    {
                        await ProcessRecordAsync(record);
                        importedCount++;
                    }
                    catch (Exception ex)
                    {
                        result.ErrorMessages.Add($"Error processing record with FolderPath '{record.FolderPath}': {ex.Message}");

                    }

                    if (result.ErrorMessages.Any()) // rollback if any error occurs
                    {
                        await _unitOfWork.RollBackTransactionAsync();
                        result.Success = false;
                        result.TImportedCount = 0;
                        return result;
                    }

                    // save all new imports
                    await _unitOfWork.SaveAsync();
                    await _unitOfWork.CommitTransactionAsync();

                    result.Success = true;
                    result.TImportedCount = importedCount;

                }
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackTransactionAsync();
                result.Success = false;
                result.ErrorMessages.Add($"An error occurred during the import process: {ex.Message}");
            }

            return result;
        }

        private async Task ProcessRecordAsync(CsvImportDO record)
        {
            //get folder path 
            var pathParts = record.FolderPath.Split('/', StringSplitOptions.RemoveEmptyEntries);

            if (pathParts.Length < 3)
            {
                throw new Exception($"Ungültiger Pfad: '{record.FolderPath}'.");
            }

            //get location
            var locationName = pathParts[0];
            var location = await _unitOfWork.Context.Locations.FirstOrDefaultAsync(l => l.Name == locationName);

            if (location == null) // new location registration
            {
                location = new Location
                {
                    Name = locationName
                };
                await _unitOfWork.Context.Locations.AddAsync(location);
                await _unitOfWork.SaveAsync();
            }


            // get line 
            var lineName = pathParts[1];
            var line = await _unitOfWork.Context.Lines.FirstOrDefaultAsync(l => l.Name == lineName && l.LocationId == location.LocationId);

            if (line == null) // new line registration
            {
                line = new Line
                {
                    Name = lineName,
                    LocationId = location.LocationId
                };
                await _unitOfWork.Context.Lines.AddAsync(line);
                await _unitOfWork.SaveAsync();
            }


            //get machine
            var machineName = pathParts[2];
            var machine = await _unitOfWork.Context.Machines.FirstOrDefaultAsync(m => m.Name == machineName && m.LineId == line.LineId);

            if (machine == null)
            {
                machine = new Machine
                {
                    Name = machineName,
                    LineId = line.LineId
                };

                await _unitOfWork.Context.Machines.AddAsync(machine);
                await _unitOfWork.SaveAsync();
            }



            // get article
            var article = await _unitOfWork.Context.Articles.FirstOrDefaultAsync(a => a.ArticleNumber == record.ItemNumber);

            if (article == null)
            {
                article = new Article
                {
                    ArticleNumber = record.ItemNumber,
                    Manufacturer = record.Manufacturer,
                    Name = record.ItemNumber,

                };
                await _unitOfWork.Context.Articles.AddAsync(article);
                await _unitOfWork.SaveAsync();
            }

            //get component instance OR update quantity if it already exists
            var component = await _unitOfWork.Context.Components
                .FirstOrDefaultAsync(ci => ci.ArticleId == article.ArticleId && ci.MachineId == machine.MachineId);

            if (component != null) // update quantity
            {
                component.Quantity += record.Quantity;
                _unitOfWork.Context.Components.Update(component);

            }
            else // create new component
            {
                var newComponent = new Component
                {
                    ArticleId = article.ArticleId,
                    MachineId = machine.MachineId,
                    Quantity = record.Quantity,
                    ComponentGroup = record.ComponentGroup,
                    ComponentType = record.ComponentType,
                };
                await _unitOfWork.Context.Components.AddAsync(newComponent);
                await _unitOfWork.SaveAsync();
            }
        }
    }
}
