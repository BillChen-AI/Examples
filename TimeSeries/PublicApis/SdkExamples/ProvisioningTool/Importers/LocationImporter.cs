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

        protected override IEnumerable<Location> GetAllExistingItems()
        {
            var response = ServiceClient.Publish.Get(new LocationDescriptionListServiceRequest());

            return response.LocationDescriptions.Select(_mapper.ToLocationDescription);
        }

        protected override bool AreSameItems(Location existing, ParsedLocation parsedItem)
        {
            return existing.Identifier == parsedItem.LocationIdentifier;
        }

        protected override Location Create(ParsedLocation parsedLocaiton)
        {
            var postLocation = _mapper.ToPostLocation(parsedLocaiton);

            return ServiceClient.Provisioning.Post(postLocation);
        }

        protected override Location Update(ParsedLocation parsedItem, List<Location> existingItems)
        {
            var uniqueId = existingItems.Find(l => l.Identifier == parsedItem.LocationIdentifier).UniqueId;

            var putLocation = _mapper.ToPutLocation(uniqueId, parsedItem);

            return ServiceClient.Provisioning.Put(putLocation);
        }
    }
}
