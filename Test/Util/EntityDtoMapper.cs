using System;
using AutoMapper;
using log4net;
using Test.DTO;
using Test.Models;

namespace Test.Util
{
    /// <summary>
    /// Contains methods to map from DTO object to Entity object and vice versa.
    /// </summary>
    public static class EntityDtoMapper
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(EntityDtoMapper));

        public static void InitMaps(IMapperConfigurationExpression cfg)
        {
            #region Entities to DTOs
            cfg.CreateMap<User, UserDto>();

            #endregion

            #region DTOs to Entities
            cfg.CreateMap<UserDto, User>();
            #endregion

        }
    }
}
