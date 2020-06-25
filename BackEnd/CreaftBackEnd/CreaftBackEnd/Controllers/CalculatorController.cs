using CraftBackEnd.Common.Models.IO;
using CraftBackEnd.Filters;
using CraftBackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CraftBackEnd.Controllers
{
    //[Authorize]
    //[AuthFilter(false)]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class CalculatorController : BaseController
    {
        private readonly ICalculatorService _calculatorService;
        private readonly IUserTierService _userTierService;

        public CalculatorController(ICalculatorService calculatorService,
            IUserTierService userTierService) {
            _calculatorService = calculatorService;
            _userTierService = userTierService;
        }

        [HttpPost("CalculateNetworth")]
        public async Task<IActionResult> CalculateNetworth([FromBody] NetworthRequest networthRequest) {
            var test = CurrentUser;
            var limit = await _userTierService.GetFieldCountLimitAsync();
            return new ObjectResult(_calculatorService.CalculateNetworth(networthRequest, limit));
        }

        [HttpGet("TestCalculate")]
        public IActionResult TestCalculate() {
            return new ObjectResult(2525);
        }
    }
}
