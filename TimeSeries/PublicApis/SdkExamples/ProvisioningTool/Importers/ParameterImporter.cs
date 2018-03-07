using System.Collections.Generic;
using System.Linq;
using Aquarius.TimeSeries.Client;
using Aquarius.TimeSeries.Client.ServiceModels.Provisioning;
using ProvisioningTool.Mappers;
using ProvisioningTool.Parsers;

namespace ProvisioningTool.Importers
{
    public class ParameterImporter : ImporterBase<ParameterCsvFileParser, ParsedParameter, Parameter>
    {
        private readonly ParameterMapper _parameterMapper = new ParameterMapper();

        public ParameterImporter(IAquariusClient serviceClient) : base(serviceClient)
        {
        }

        public override string ImportObjectName => "Parameter";

        protected override string GetId(ParsedParameter item)
        {
            return item.ParameterId;
        }

        protected override IEnumerable<UpdateItem<ParsedParameter>> GetItemsToUpdate(List<ParsedParameter> parsedItems)
        {
            var allExistingItems = GetAllExistingItems();

            return allExistingItems
                .Where(existing => parsedItems.Exists(parsedItem => AreSameItems(existing, parsedItem)))
                .Select(existing => new UpdateItem<ParsedParameter>(existing.UniqueId,
                    parsedItems.First(parsedItem => AreSameItems(existing, parsedItem))));

        }

        private IEnumerable<Parameter> GetAllExistingItems()
        {
            var response = ServiceClient.Provisioning.Get(new GetParameters());

            return response.Results;
        }

        private bool AreSameItems(Parameter existing, ParsedParameter parsedItem)
        {
            return existing.ParameterId == parsedItem.ParameterId;
        }

        protected override Parameter Create(ParsedParameter parsedParameter)
        {
            var postParameter = _parameterMapper.ToPostParameter(parsedParameter);

            return ServiceClient.Provisioning.Post(postParameter);
        }

        protected override Parameter Update(UpdateItem<ParsedParameter> parameterToUpdate)
        {
            var uniqueId = parameterToUpdate.UniqueId;

            var putParameter = _parameterMapper.ToPutParameter(uniqueId, parameterToUpdate.ParsedItem);

            return ServiceClient.Provisioning.Put(putParameter);
        }
    }
}
