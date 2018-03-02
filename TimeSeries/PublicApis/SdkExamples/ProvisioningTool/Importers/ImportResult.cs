using System.Collections.Generic;

namespace ProvisioningTool.Importers
{
    public class ImportResult<TToImport, TImported>
    {
        public ImportResult()
        {
            ItemsToCreate = new List<TToImport>();
            ItemsToUpdate = new List<TToImport>();

            CreatedItems = new List<TImported>();
            UpdatedItems = new List<TImported>();

            CreateFailedItems = new List<TToImport>();
            UpdateFailedItems = new List<TToImport>();
        }
        public int ParsedItemTotal { get; set; }

        public List<TToImport> ItemsToCreate { get; }
        public List<TToImport> ItemsToUpdate { get; }

        public List<TImported> CreatedItems { get; }
        public List<TImported> UpdatedItems { get; }

        public List<TToImport> CreateFailedItems { get; }
        public List<TToImport> UpdateFailedItems { get; }
    }
}
