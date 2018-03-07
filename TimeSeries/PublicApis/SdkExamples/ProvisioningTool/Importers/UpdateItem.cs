using System;

namespace ProvisioningTool.Importers
{
    public class UpdateItem<TParsedItem>
    {
        public TParsedItem ParsedItem { get; set; }
        public Guid UniqueId { get; set; }

        public UpdateItem()
        {
        }

        public UpdateItem(Guid uniqueId, TParsedItem parsedItem)
            :this()
        {
            UniqueId = uniqueId;
            ParsedItem = parsedItem;
        }
    }
}
