using System;
using System.Collections.Generic;
using System.Text;

namespace CraftBackEnd.Common.Models.IO
{
    public struct NetworthResult
    {
        public NetworthResult(decimal totalAssets, decimal totalLiabilities) {
            TotalAssets = totalAssets;
            TotalLiabilities = totalLiabilities;
            NetWorth = totalAssets - totalLiabilities;
        }
        public decimal TotalAssets { get; }
        public decimal TotalLiabilities { get; }
        public decimal NetWorth { get; }
    }
}
