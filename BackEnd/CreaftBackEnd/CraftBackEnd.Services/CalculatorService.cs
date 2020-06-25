using CraftBackEnd.Common.Models;
using CraftBackEnd.Common.Models.IO;
using CraftBackEnd.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CraftBackEnd.Services
{
    public class CalculatorService : ICalculatorService
    {
        private readonly IValidationService _validationService;

        public CalculatorService(IValidationService validationService) {
            _validationService = validationService;
        }

        public NetworthResult CalculateNetworth(NetworthRequest networthRequest, int fieldLengthLimit) {
            try {
                ValidateValues(networthRequest.Assets, fieldLengthLimit);
                ValidateValues(networthRequest.Liabilities, fieldLengthLimit);

                var assetTotal = GetTotal(networthRequest.Assets);
                var liabilityTotal = GetTotal(networthRequest.Liabilities);
                return new NetworthResult(assetTotal, liabilityTotal);
            } catch (NullReferenceException) {
                _validationService.ThrowValidationErrors("Field list not initialized");
            } 
            
            return new NetworthResult();
        }

        private void ValidateValues(IList<FinancialEntity> entities, int fieldLengthLimit) {
            foreach (var entity in entities) {
                if (entity.StaticFields.Where(f => f.Value < 0).FirstOrDefault() != null)
                    _validationService.ThrowValidationErrors("Negative value is disallowed");

                if (entity.DynamicFields.Where(f => f.Value < 0).FirstOrDefault() != null)
                    _validationService.ThrowValidationErrors("Negative value is disallowed");

                if (entity.DynamicFields.Count > fieldLengthLimit)
                    _validationService.ThrowValidationErrors($"Dynamic field count exceeds the limit of {fieldLengthLimit}");
            }
        }

        private decimal GetTotal(IList<FinancialEntity> entities) {
            var total = 0M;
            foreach(var entity in entities) {
                total += entity.StaticFields.Select(f => f.Value).Sum();
                total += entity.DynamicFields.Select(f => f.Value).Sum();
            }

            return total;
        }

        //private decimal GetLiabilitiesTotal(Liability liability, int fieldLengthLimit = 0) {
        //    var total = 0M;
        //    foreach(var sf in liability.StaticFields) {
        //        total += sf.Select(v => v.Value).Sum();
        //    }
            
        //    if (liability.LTDDynamicFields.Count > fieldLengthLimit || liability.STLDynamicFields.Count > fieldLengthLimit) {
        //        _validationService.ThrowValidationErrors($"Dynamic field count exceeds the limit of {fieldLengthLimit}");
        //    }

        //    //liability.LongTermDebt.Select(d => d.Value < 0).FirstOrDefault()
        //    var ltd = liability.LongTermDebt.Select(d => d.Value).Sum();
        //    var ltdd = liability.LTDDynamicFields.Select(dd => dd.Value).Sum();
        //    var stl = liability.ShortTermLiabilities.Select(l => l.Value).Sum();
        //    var stld = liability.STLDynamicFields.Select(sd => sd.Value).Sum();
        //    return ltd + ltdd + stl + stld;
        //}

        //private decimal GetAssetsTotal(Asset asset, int fieldLengthLimit = 0) {
        //    if (asset.CAIDynamicFields.Count > fieldLengthLimit || asset.LTADynamicFields.Count > fieldLengthLimit) {
        //        _validationService.ThrowValidationErrors($"Dynamic field count exceeds the limit of {fieldLengthLimit}");
        //    }

        //    var cai = asset.CashAndInvestments.Select(d => d.Value).Sum();
        //    var caid = asset.CAIDynamicFields.Select(cd => cd.Value).Sum();
        //    var lta = asset.LongTermAssets.Select(l => l.Value).Sum();
        //    var ltad = asset.LTADynamicFields.Select(ld => ld.Value).Sum();
        //    return cai + caid + lta + ltad;
        //}

        //private void NegativeValueValidation() {

        //}
    }
}
