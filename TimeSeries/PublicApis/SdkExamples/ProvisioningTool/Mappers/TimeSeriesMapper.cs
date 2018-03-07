using System;
using Aquarius.TimeSeries.Client.ServiceModels.Provisioning;
using AutoMapper;
using NodaTime;
using ProvisioningTool.Parsers;

namespace ProvisioningTool.Mappers
{
    public class TimeSeriesMapper : MapperBase
    {
        public TimeSeriesMapper()
        {
            CreateMappings();
        }

        protected override void ConfigureMappings(IMapperConfigurationExpression maps)
        {
            maps.CreateMap<ParsedTimeSeries, PostReflectedTimeSeries>()
                .ForMember(destination => destination.GapTolerance, opt => 
                    opt.ResolveUsing(source => GetGapToleranceFromMinutes(source.GapToleranceInMinutes)))
                .ForMember(destination => destination.Parameter, opt => opt.MapFrom(source => source.ParameterId))
                .ForMember(destination => destination.Unit, opt => opt.MapFrom(source => source.UnitId))
                .ForMember(destination => destination.Method, 
                    opt => opt.ResolveUsing(source => string.IsNullOrWhiteSpace(source.Method) ? "DefaultNone" : source.Method))
                .ForMember(destination => destination.UtcOffset, opt => opt.ResolveUsing(source => TimeSpanToOffset(source.UtcOffset)));

            maps.CreateMap<PostReflectedTimeSeries, PostBasicTimeSeries>();

            maps.CreateMap<ParsedTimeSeries, PutTimeSeries>()
                .ForMember(destination => destination.ExtendedAttributeValues, opt => opt.Ignore());
        }

        private Duration GetGapToleranceFromMinutes(int minutes)
        {
            return minutes == 0
                ? Aquarius.TimeSeries.Client.Helpers.DurationExtensions.MaxGapDuration
                : Duration.FromMinutes(minutes);
        }

        private Offset TimeSpanToOffset(TimeSpan? timeSpan)
        {
            return timeSpan.HasValue
                ? Offset.FromHoursAndMinutes(timeSpan.Value.Hours, timeSpan.Value.Minutes)
                : Offset.Zero;
        }

        public PostReflectedTimeSeries ToPostReflectedTimeSeries(Guid locationUniqueId, ParsedTimeSeries parsedTimeSeries)
        {
            var postTimeSeries =  Mapper.Map<PostReflectedTimeSeries>(parsedTimeSeries);
            postTimeSeries.LocationUniqueId = locationUniqueId;

            return postTimeSeries;
        }

        public PostBasicTimeSeries ToPostBasicTimeSeries(Guid locationUniqueId, ParsedTimeSeries parsedTimeSeries)
        {
            var postReflectedTimeSeries = ToPostReflectedTimeSeries(locationUniqueId, parsedTimeSeries);

            return Mapper.Map<PostBasicTimeSeries>(postReflectedTimeSeries);
        }

        public PutTimeSeries ToPutTimeSeries(Guid uniqueId, ParsedTimeSeries parsedTimeSeries)
        {
            var putTimeSeries = Mapper.Map<PutTimeSeries>(parsedTimeSeries);
            putTimeSeries.TimeSeriesUniqueId = uniqueId;

            return putTimeSeries;
        }
    }
}
