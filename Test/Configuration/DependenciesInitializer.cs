using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using AutoMapper;
using log4net;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using Test.DAL;
using Test.DTO;
using Test.Models;
using Test.Repositories;
using Test.Services;
using Test.Services.Data;
using Test.Util;

namespace Test.Configuration
{
    /// <summary>
    /// Contains methods to initialize the DI container.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class DependenciesInitializer
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(DependenciesInitializer));

        /// <summary>
        /// Configures the Dependency Injection container for this application, registering implementations for interfaces.
        /// </summary>
        /// <returns>DI Container that must be used to initialize the Web API dependency resolver.</returns>
        public static Container Init()
        {
            var container = new Container();
            try
            {
                container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

                var assemblies = getImplAssemblies();

                container.Register<IMapper>(() =>
                {
                    var config = new MapperConfiguration(cfg => EntityDtoMapper.InitMaps(cfg));
                    return config.CreateMapper();
                }, Lifestyle.Singleton);

                container.Register<Test.DAL.DbContext>(Lifestyle.Scoped);

                container.Register<IDolarRequest, DolarRequest>();
                container.Register<IBaseDataRepository<User, int>>(() => new UserRepository(), Lifestyle.Scoped);

                container.Register<IStoredDataService<User, UserDto, int>, BaseStoredDataService<User, UserDto, int>>(Lifestyle.Scoped);


                //container.Verify();
            }
            catch (Exception ex)
            {
                logger.Fatal("DI Container initialization failed.", ex);
                System.Diagnostics.Debugger.Break();
                throw ex;
            }
            return container;
        }

        private static IEnumerable<Assembly> getImplAssemblies()
        {
            var result = new List<Assembly>();
            var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in allAssemblies)
            {
                if (assembly.FullName.Contains("Test")) result.Add(assembly);
            }
            return result;
        }
    }
}
