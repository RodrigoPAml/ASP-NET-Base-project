using API.Models.Entities;
using API.Models.NewEntity;
using API.Models.UpdatedEntity;
using AutoMapper;

namespace API.Registration
{
    public static class MapperRegister
    {
        /// <summary>
        /// Create map between models
        /// </summary>
        /// <param name="configuration"></param>
        public static void Register(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<NewUser, User>();
            configuration.CreateMap<UpdatedUser, User>();

            configuration.CreateMap<NewMovie, Movie>();
            configuration.CreateMap<UpdatedMovie, Movie>();

            configuration.CreateMap<NewSession, Session>();
            configuration.CreateMap<UpdatedSession, Session>();
        }
    }
}
