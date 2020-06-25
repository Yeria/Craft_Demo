using CraftBackEnd.Services.Interfaces;
using CraftBackEnd.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CraftBackEnd.Controllers
{
    [Authorize]
    [AuthFilter]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService) {
            _userService = userService;
        }

        /// <summary>
        /// Gets User Profile information
        /// </summary>
        /// <remarks>
        /// <b>- Valid Bearer token required</b>
        /// <hr />
        /// </remarks>
        /// <param name="id">User ID of user to get</param>
        /// <returns></returns>
        /// <response code="200">Returns User object of requested User ID</response>
        /// <response code="401">token does not match userId or token is invalid</response>  
        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser(int id) {
            if (CurrentUser.Id == id)
                return new ObjectResult(await _userService.GetUser(id));

            return new UnauthorizedResult();
        }
    }
}
