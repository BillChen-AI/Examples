using System.Collections.Generic;
using AutoMapper;

namespace ProvisioningTool.Mappers
{
    public class SimpleMapper<TSource, TTarget>
    {
        private readonly IMapper _mapper;
        public SimpleMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TTarget>());
            _mapper = config.CreateMapper();
        }

        public IEnumerable<TTarget> MapMany(IEnumerable<TSource> source)
        {
            return _mapper.Map<IEnumerable<TSource>, IEnumerable<TTarget>>(source);
        }
    }
}
