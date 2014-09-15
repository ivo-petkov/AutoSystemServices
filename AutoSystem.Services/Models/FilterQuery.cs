using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoSystem.Services.Models
{
    public class FilterQuery
    {
        public string RegisterPlate { get; set; }
        public string Brand { get; set; }
        public int? ClientId { get; set; }
        public int? PerformerId { get; set; }
        public int? Status { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}