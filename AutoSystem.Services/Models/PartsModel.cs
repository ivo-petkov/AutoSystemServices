using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoSystem.Services.Models
{
    public class PartsModel
    {
        public int PartsId { get; set; }
        public string Text { get; set; }
        public string PriceInfo { get; set; }
        public int RepairId { get; set; }
    }
}