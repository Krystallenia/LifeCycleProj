namespace LifeCycle.Services.IServices
{
    public interface ICsvImportService
    {
        Task<ImportResult> ImportCsvDataAsync(Stream csvStream);

        public class ImportResult
        {
            public bool Success { get; set; }

            public int TImportedCount { get; set; }

            public List<string> ErrorMessages { get; set; } = new List<string>();

            public List<string> Warnings { get; set; } = new List<string>();
        }
    }
}
