using System;
using AutoMapper;

namespace ProvisioningTool.Mappers
{
    public abstract class MapperBase
    {
        private MapperConfiguration Maps { get; set; }
        protected IMapper Mapper { get; private set; }

        protected const Int64 UnsavedId = 0;

        private static readonly object SyncLock = new object();

        protected void CreateMappings(params Profile[] profiles)
        {
            lock (SyncLock)
            {
                Maps = new MapperConfiguration(maps =>
                {
                    foreach (var profile in profiles)
                        maps.AddProfile(profile);
                    ConfigureMappings(maps);
                });
            }

            Mapper = Maps.CreateMapper();
        }

        protected abstract void ConfigureMappings(IMapperConfigurationExpression maps);

        protected TDestination Map<TDestination>(object source)
        {
            return Mapper.Map<TDestination>(source);
        }

        protected TDestination Map<TSource, TDestination>(TSource source)
        {
            return Mapper.Map<TSource, TDestination>(source);
        }

        public virtual void VerifyTypeMappings()
        {
            Maps.AssertConfigurationIsValid();
        }
    }
}
