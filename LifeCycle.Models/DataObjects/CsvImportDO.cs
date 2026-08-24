using CsvHelper.Configuration.Attributes;

namespace LifeCycle.DataObjects
{
    public class CsvImportDO
    {
        [Name("Ordner")]
        public string FolderPath { get; set; }

        [Name("Komponentengruppe")]
        public string ComponentGroup { get; set; }

        [Name("Komponentengruppentyp")]
        public string ComponentType { get; set; }

        [Name("Artikelnummer")]
        public string ItemNumber { get; set; }

        [Name("Hersteller")]
        public string Manufacturer { get; set; }

        [Name("Menge")]
        public int Quantity { get; set; }

    }
}
