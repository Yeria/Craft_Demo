using CraftBackEnd.Common.Models;
using CraftBackEnd.Common.Models.IO;
using CraftBackEnd.Filters;
using CraftBackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CraftBackEnd.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class IAMController : BaseController
    {
        private readonly IIAMService _iamService;
        
        public IAMController(IIAMService iamService) {
            _iamService = iamService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest) {
            return new ObjectResult(await _iamService.LoginAsync(loginRequest.Username, loginRequest.Password));
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateAccount([FromBody] User user) {
            var result = await _iamService.CreateAccountAsync(user);
            
            return new ObjectResult(result);
        }

        [HttpGet("IsAuthenticated")]
        public async Task<IActionResult> IsAuthenticated() {
            return new ObjectResult(await _iamService.IsAuthenticatedAsync());
        }
    }
}
