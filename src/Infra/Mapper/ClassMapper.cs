using API.Registration;
using AutoMapper;

namespace API.Infra.Mapper
{
    /// <summary>
    /// Base class for mapping entities
    /// </summary>
    public static class ClassMapper
    {
        private static IMapper _mapper = null;

        private static void Configure()
        {
            MapperConfiguration config = new MapperConfiguration(x =>
            {
                MapperRegister.Register(x);
            });

            _mapper = config.CreateMapper();
        }

        public static T Map<T>(object obj) where T : class
        {
            if (_mapper == null)
                Configure();

            return _mapper.Map<T>(obj);
        }
    }
}
