using System;
using System.Collections.Generic;
using System.Linq;
using Aquarius.TimeSeries.Client;
using Aquarius.TimeSeries.Client.ServiceModels.Provisioning;
using Aquarius.TimeSeries.Client.ServiceModels.Publish;
using ProvisioningTool.Mappers;
using ProvisioningTool.Parsers;
using TimeSeriesType = ProvisioningTool.Parsers.TimeSeriesType;

namespace ProvisioningTool.Importers
{
    public class TimeSeriesImporter : ImporterBase<TimeSeriesCsvFileParser, ParsedTimeSeries, TimeSeries>
    {
        private readonly TimeSeriesMapper _mapper = new TimeSeriesMapper();
        private IDictionary<string, Guid> _locationIdGuidMap = new Dictionary<string, Guid>();
        private IDictionary<string, Parameter> _displayIdParameterMap =  new Dictionary<string, Parameter>();

        public TimeSeriesImporter(IAquariusClient serviceClient) : base(serviceClient)
        {
        }

        public override string ImportObjectName => "TimeSeries";

        protected override string GetId(ParsedTimeSeries item)
        {
            return $@"{item.ParameterId}.{item.Label}@{item.LocationIdentifier}";
        }

        private bool AreSameItems(TimeSeriesDescription existing, ParsedTimeSeries parsedItem)
        {
            var paramId = GetParameterIdByDisplayId(existing.Parameter);

            return existing.Label == parsedItem.Label &&
                   paramId == parsedItem.ParameterId &&
                   existing.Unit == parsedItem.UnitId &&
                   existing.LocationIdentifier == parsedItem.LocationIdentifier;
        }

        private string GetParameterIdByDisplayId(string parameterDisplayId)
        {
            if(!_displayIdParameterMap.Any())
            {
                var parameters = ServiceClient.Provisioning.Get(new GetParameters()).Results;

                _displayIdParameterMap = parameters.ToDictionary(p => p.Identifier);
            }

            if (_displayIdParameterMap.ContainsKey(parameterDisplayId))
                return _displayIdParameterMap[parameterDisplayId].ParameterId;

            throw new ProvisioningToolException($"Parameter with display id '{parameterDisplayId}' does not exist.");
        }

        protected override IEnumerable<UpdateItem<ParsedTimeSeries>> GetItemsToUpdate(List<ParsedTimeSeries> parsedItems)
        {
            var allExistingItems = GetAllExistingItems();

            return allExistingItems
                .Where(existing => parsedItems.Exists(parsedItem => AreSameItems(existing, parsedItem)))
                .Select(existing => new UpdateItem<ParsedTimeSeries>(existing.UniqueId,
                            parsedItems.First(parsedItem => AreSameItems(existing, parsedItem))));
        }

        private List<TimeSeriesDescription> GetAllExistingItems()
        {
            var response = ServiceClient.Publish.Get(new TimeSeriesDescriptionServiceRequest());

            return response.TimeSeriesDescriptions;
        }


        protected override TimeSeries Create(ParsedTimeSeries parsedTimeSeries)
        {
            var locationUniqueId = GetLocationUniqueIdByIdentifier(parsedTimeSeries.LocationIdentifier);

            switch (parsedTimeSeries.TimeSeriesType)
            {
                case TimeSeriesType.Basic:
                    return CreateBasicTimeSeries(parsedTimeSeries, locationUniqueId);
                case TimeSeriesType.Reflected:
                    return CreateReflectedTimeSeries(parsedTimeSeries, locationUniqueId);
            }

            throw new ProvisioningToolException($"Unknown time series type: {parsedTimeSeries.TimeSeriesType}");
        }

        private TimeSeries CreateBasicTimeSeries(ParsedTimeSeries parsedTimeSeries, Guid locationUniqueId)
        {
            var postBasicTimeSeries = _mapper.ToPostBasicTimeSeries(locationUniqueId, parsedTimeSeries);

            return ServiceClient.Provisioning.Post(postBasicTimeSeries);
        }

        private TimeSeries CreateReflectedTimeSeries(ParsedTimeSeries parsedTimeSeries, Guid locationUniqueId)
        {
            var postReflectedTimeSeries = _mapper.ToPostReflectedTimeSeries(locationUniqueId, parsedTimeSeries);

            return ServiceClient.Provisioning.Post(postReflectedTimeSeries);
        }

        private Guid GetLocationUniqueIdByIdentifier(string locationIdentifier)
        {
            if(!_locationIdGuidMap.Any())
            {
                var response = ServiceClient.Publish.Get(new LocationDescriptionListServiceRequest());
                _locationIdGuidMap = response.LocationDescriptions.ToDictionary(des => des.Identifier, des => des.UniqueId);
            }

            if (_locationIdGuidMap.ContainsKey(locationIdentifier))
                return _locationIdGuidMap[locationIdentifier];

            throw new ProvisioningToolException($"Location '{locationIdentifier}' does not exist.");
        }

        protected override TimeSeries Update(UpdateItem<ParsedTimeSeries> updateItem)
        {
            var putTimeSeries = _mapper.ToPutTimeSeries(updateItem.UniqueId, updateItem.ParsedItem);

            return ServiceClient.Provisioning.Put(putTimeSeries);
        }
    }
}
