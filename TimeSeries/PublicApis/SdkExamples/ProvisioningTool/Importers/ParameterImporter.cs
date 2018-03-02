using System.Collections.Generic;
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

        protected override IEnumerable<Parameter> GetAllExistingItems()
        {
            var response = ServiceClient.Provisioning.Get(new GetParameters());

            return response.Results;
        }

        protected override bool AreSameItems(Parameter existing, ParsedParameter parsedItem)
        {
            return existing.ParameterId == parsedItem.ParameterId;
        }

        protected override Parameter Create(ParsedParameter parsedParameter)
        {
            var postParameter = _parameterMapper.ToPostParameter(parsedParameter);

            return ServiceClient.Provisioning.Post(postParameter);
        }

        protected override Parameter Update(ParsedParameter parsedParameter,
            List<Parameter> existingAqParams)
        {
            var uniqueId = existingAqParams.Find(p => p.ParameterId == parsedParameter.ParameterId).UniqueId;

            var putParameter = _parameterMapper.ToPutParameter(uniqueId, parsedParameter);

            return ServiceClient.Provisioning.Put(putParameter);
        }
    }
}
