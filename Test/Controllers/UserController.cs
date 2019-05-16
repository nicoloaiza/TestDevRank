using System;
using Microsoft.AspNetCore.Mvc;
using Test.Controllers.Data;
using Test.DTO;
using Test.Models;
using Test.Services;

namespace Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : AbstractDataEntityController<User, UserDto, int>
    {
        private readonly IStoredDataService<User, UserDto, int> userService;
        /// <summary>
        /// Controller constuctor
        /// </summary>
        /// <param name="service"></param>
        public UserController(IStoredDataService<User, UserDto, int> service) : base(service) => userService = service ?? throw new ArgumentNullException();

    }
}
