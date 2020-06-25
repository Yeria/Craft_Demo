using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CraftBackEnd.Common.Models.IO
{
    public class NetworthRequest
    {
        [Required]
        public IList<FinancialEntity> Liabilities { get; set; }
        [Required]
        public IList<FinancialEntity> Assets { get; set; }
    }
}
