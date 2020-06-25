using CraftBackEnd.Common.Configs;
using CraftBackEnd.Common.Models;
using CraftBackEnd.Common.Models.Base;
using CraftBackEnd.Common.Models.Exception;
using CraftBackEnd.Common.Models.IO;
using CraftBackEnd.Services;
using CraftBackEnd.Services.Entity;
using CraftBackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace CraftBackEnd.Tests
{
    public class CalculatorSerivce_Test
    {
        private IValidationService _validationService;
        private ICalculatorService _calculatorService;

        public CalculatorSerivce_Test() {
            //var services = new ServiceCollection();
            

            //services.Configure<TierCountLimit>(System.Configuration.GetSection("TierCountLimit"));
            //services.AddTransient<ICalculatorService, CalculatorService>();
            //services.AddTransient<IUserTierService, UserTierService>();
            //services.AddTransient<IIAMService, IAMService>();
            //services.AddTransient<IValidationService, ValidationService>();

            //var serviceProvider = services.BuildServiceProvider();

            //_calculatorService = serviceProvider.GetService<ICalculatorService>();

            ////services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            ////_dbContext = new DatabaseContext();
            ////_validationService = new ValidationService();
            ////httpContextAccessor = new HttpContextAccessor();
            ////_iamService = new IAMService(_dbContext, _validationService, );
            ////_userTierService = new UserTierService()
            ////_calculatorService = new CalculatorService();

            _validationService = new ValidationService();
            _calculatorService = new CalculatorService(_validationService);
        }

        [Fact]
        public void CalculatorServiceTest_HappyPathTest() {
            var networthReq = new NetworthRequest();
            networthReq.Assets = new List<FinancialEntity>();
            networthReq.Liabilities = new List<FinancialEntity>();

            var asset = new FinancialEntity();
            asset.StaticFields = new List<LabelValue>();
            asset.DynamicFields = new List<LabelValue>();

            asset.StaticFields.Add(new LabelValue { Label = "Chequing", Value = 1000 });
            asset.StaticFields.Add(new LabelValue { Label = "Primary Home", Value = 500000 });

            asset.DynamicFields.Add(new LabelValue { Label = "Investment 1", Value = 2000 });

            var liability = new FinancialEntity();
            liability.StaticFields = new List<LabelValue>();
            liability.DynamicFields = new List<LabelValue>();

            liability.StaticFields.Add(new LabelValue { Label = "Loan", Value = 10000 });
            liability.DynamicFields.Add(new LabelValue { Label = "Credit Card 1", Value = 2000 });

            networthReq.Assets.Add(asset);
            networthReq.Liabilities.Add(liability);

            var result = _calculatorService.CalculateNetworth(networthReq, 1);

            Assert.Equal(503000, result.TotalAssets);
            Assert.Equal(12000, result.TotalLiabilities);
            Assert.Equal(491000, result.NetWorth);
        }

        [Fact]
        public void CalculatorServiceTest_NegativeValueTest() {
            var networthReq = new NetworthRequest();
            networthReq.Assets = new List<FinancialEntity>();
            networthReq.Liabilities = new List<FinancialEntity>();

            var asset = new FinancialEntity();
            asset.StaticFields = new List<LabelValue>();
            asset.DynamicFields = new List<LabelValue>();

            asset.StaticFields.Add(new LabelValue { Label = "Chequing", Value = -1000 });
            asset.StaticFields.Add(new LabelValue { Label = "Primary Home", Value = 500000 });

            asset.DynamicFields.Add(new LabelValue { Label = "Investment 1", Value = 2000 });

            var liability = new FinancialEntity();
            liability.StaticFields = new List<LabelValue>();
            liability.DynamicFields = new List<LabelValue>();

            liability.StaticFields.Add(new LabelValue { Label = "Loan", Value = 10000 });
            liability.DynamicFields.Add(new LabelValue { Label = "Credit Card 1", Value = 2000 });

            networthReq.Assets.Add(asset);
            networthReq.Liabilities.Add(liability);

            Action testAction = () => _calculatorService.CalculateNetworth(networthReq, 5);

            Assert.Throws<ValidationErrorException>(testAction);
        }

        [Fact]
        public void CalculatorServiceTest_NullValueTest() {
            var networthReq = new NetworthRequest();
            networthReq.Assets = new List<FinancialEntity>();
            networthReq.Liabilities = new List<FinancialEntity>();

            var asset = new FinancialEntity();
            asset.StaticFields = new List<LabelValue>();
            asset.DynamicFields = new List<LabelValue>();

            asset.StaticFields.Add(new LabelValue { Label = "Chequing", Value = 1000 });
            asset.StaticFields.Add(new LabelValue { Label = "Primary Home", Value = 500000 });

            asset.DynamicFields.Add(new LabelValue { Label = "Investment 1", Value = 2000 });

            var liability = new FinancialEntity();
            liability.StaticFields = new List<LabelValue>();
            liability.DynamicFields = new List<LabelValue>();

            liability.StaticFields.Add(new LabelValue { Label = "Loan", Value = 10000 });
            liability.DynamicFields.Add(new LabelValue { Label = "Credit Card 1", Value = 2000 });

            networthReq.Assets.Add(asset);
            
            // null value here
            networthReq.Liabilities = null;

            Action testAction = () => _calculatorService.CalculateNetworth(networthReq, 10);

            Assert.Throws<ValidationErrorException>(testAction);
        }

        [Fact]
        public void CalculatorServiceTest_DynamicFieldLengthDisallowedTest() {
            var networthReq = new NetworthRequest();
            networthReq.Assets = new List<FinancialEntity>();
            networthReq.Liabilities = new List<FinancialEntity>();

            var asset = new FinancialEntity();
            asset.StaticFields = new List<LabelValue>();
            asset.DynamicFields = new List<LabelValue>();

            asset.StaticFields.Add(new LabelValue { Label = "Chequing", Value = -1000 });
            asset.StaticFields.Add(new LabelValue { Label = "Primary Home", Value = 500000 });

            asset.DynamicFields.Add(new LabelValue { Label = "Investment 1", Value = 2000 });
            asset.DynamicFields.Add(new LabelValue { Label = "Investment 2", Value = 5000 });

            var liability = new FinancialEntity();
            liability.StaticFields = new List<LabelValue>();
            liability.DynamicFields = new List<LabelValue>();

            liability.StaticFields.Add(new LabelValue { Label = "Loan", Value = 10000 });
            liability.DynamicFields.Add(new LabelValue { Label = "Credit Card 1", Value = 2000 });

            networthReq.Assets.Add(asset);
            networthReq.Liabilities.Add(liability);

            Action testAction = () => _calculatorService.CalculateNetworth(networthReq, 1);

            Assert.Throws<ValidationErrorException>(testAction);
        }
    }
}
