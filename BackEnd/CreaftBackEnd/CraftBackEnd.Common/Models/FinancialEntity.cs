using CraftBackEnd.Common.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CraftBackEnd.Common.Models
{
    public class FinancialEntity
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public IList<LabelValue> StaticFields { get; set; }
        [Required]
        public IList<LabelValue> DynamicFields { get; set; }
    }
}
