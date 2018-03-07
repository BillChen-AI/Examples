using System.Collections.Generic;
using System.Linq;
using Aquarius.TimeSeries.Client;
using Aquarius.TimeSeries.Client.ServiceModels.Provisioning;
using Aquarius.TimeSeries.Client.ServiceModels.Publish;
using ProvisioningTool.Mappers;
using ProvisioningTool.Parsers;

namespace ProvisioningTool.Importers
{
    public class LocationImporter : ImporterBase<LocationCsvFileParser, ParsedLocation, Location>
    {
        private readonly LocationMapper _mapper = new LocationMapper();

        public LocationImporter(IAquariusClient serviceClient) : base(serviceClient)
        {
        }

        public override string ImportObjectName => "Location";

        protected override string GetId(ParsedLocation item)
        {
            return item.LocationIdentifier;
        }

        private IEnumerable<Location> GetAllExistingItems()
        {
            var response = ServiceClient.Publish.Get(new LocationDescriptionListServiceRequest());

            return response.LocationDescriptions.Select(_mapper.ToLocation);
        }

        protected override IEnumerable<UpdateItem<ParsedLocation>> GetItemsToUpdate(List<ParsedLocation> parsedItems)
        {
            var allExistingLocations = GetAllExistingItems();

            return allExistingLocations
                .Where(existing => parsedItems.Exists(parsedItem => AreSameItems(existing, parsedItem)))
                .Select(existing => new UpdateItem<ParsedLocation>(existing.UniqueId,
                            parsedItems.First(parsedItem => AreSameItems(existing, parsedItem))));
        }

        private bool AreSameItems(Location existing, ParsedLocation parsedItem)
        {
            return existing.Identifier == parsedItem.LocationIdentifier;
        }

        protected override Location Create(ParsedLocation parsedLocaiton)
        {
            var postLocation = _mapper.ToPostLocation(parsedLocaiton);

            return ServiceClient.Provisioning.Post(postLocation);
        }

        protected override Location Update(UpdateItem<ParsedLocation> itemToUpdate)
        {
            var putLocation = _mapper.ToPutLocation(itemToUpdate.UniqueId, itemToUpdate.ParsedItem);

            return ServiceClient.Provisioning.Put(putLocation);
        }
    }
}
