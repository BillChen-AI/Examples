using System;
using Aquarius.TimeSeries.Client.ServiceModels.Provisioning;
using AutoMapper;
using ProvisioningTool.Parsers;

namespace ProvisioningTool.Mappers
{
    public class ParameterMapper : MapperBase
    {
        public ParameterMapper()
        {
            CreateMappings();
        }

        protected override void ConfigureMappings(IMapperConfigurationExpression maps)
        {
            maps.CreateMap<ParsedParameter, PostParameter>();
            maps.CreateMap<ParsedParameter, PutParameter>()
                .ForMember(dest => dest.UniqueId, opt => opt.Ignore());
        }

        public PostParameter ToPostParameter(ParsedParameter parsedParameter)
        {
            return Mapper.Map<PostParameter>(parsedParameter);
        }

        public Parameter ToAqtsParameter(ParsedParameter parsedParameter)
        {
            return Mapper.Map<Parameter>(parsedParameter);
        }

        public PutParameter ToPutParameter(Guid uniqueId, ParsedParameter parameter)
        {
            var putParam = Mapper.Map<PutParameter>(parameter);
            putParam.UniqueId = uniqueId;

            return putParam;
        }
    }
}
