using System;
using Aquarius.TimeSeries.Client.ServiceModels.Provisioning;
using Aquarius.TimeSeries.Client.ServiceModels.Publish;
using AutoMapper;
using NodaTime;
using ProvisioningTool.Parsers;

namespace ProvisioningTool.Mappers
{
    public class LocationMapper : MapperBase
    {
        public LocationMapper()
        {
            CreateMappings();
        }

        protected override void ConfigureMappings(IMapperConfigurationExpression maps)
        {
            maps.CreateMap<LocationDescription, Location>()
                .ForMember(destination => destination.LocationName, opt => opt.MapFrom(source => source.Name));

            maps.CreateMap<ParsedLocation, PostLocation>()
                .ForMember(destination => destination.UtcOffset, opt => opt.ResolveUsing(source =>TimeSpanToOffset(source.UtcOffset)))
                .ForMember(destination => destination.ElevationUnits, 
                    opt => opt.Condition(source => !string.IsNullOrWhiteSpace(source.ElevationUnits)));

            maps.CreateMap<ParsedLocation, PutLocation>()
                .ForMember(destination => destination.ElevationUnits,
                    opt => opt.Condition(source => !string.IsNullOrWhiteSpace(source.ElevationUnits)));
        }

        private Offset TimeSpanToOffset(TimeSpan? timeSpan)
        {
            return timeSpan.HasValue
                ? Offset.FromHoursAndMinutes(timeSpan.Value.Hours, timeSpan.Value.Minutes)
                : Offset.Zero;
        }

        public Location ToLocationDescription(LocationDescription locationDescription)
        {
            return Mapper.Map<Location>(locationDescription);
        }

        public PostLocation ToPostLocation(ParsedLocation parsedLocation)
        {
            return Mapper.Map<PostLocation>(parsedLocation);
        }

        public PutLocation ToPutLocation(Guid uniqueId, ParsedLocation parsedLocation)
        {
            var putLocation = Mapper.Map<PutLocation>(parsedLocation);
            putLocation.LocationUniqueId = uniqueId;

            return putLocation;
        }
    }
}
