using CsvHelper.Configuration;
using LifeCycle.DataObjects;

namespace LifeCycle.Models.DataObjects
{
    public class CsvMappingDO : ClassMap<CsvImportDO>
    {
        public CsvMappingDO()
        {
            Map(m => m.FolderPath).Name("Ordner");
            Map(m => m.ComponentGroup).Name("Komponentengruppe");
            Map(m => m.ComponentType).Name("Komponentengruppentyp");
            Map(m => m.ItemNumber).Name("Artikelnummer");
            Map(m => m.Manufacturer).Name("Hersteller");
            Map(m => m.Quantity).Name("Menge");

        }
    }
}
