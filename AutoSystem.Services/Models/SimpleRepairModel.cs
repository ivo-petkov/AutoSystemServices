using AutoSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoSystem.Services.Models
{
    public class SimpleRepairModel
    {
        public int RepairId { get; set; }
        public RepairStatus Status { get; set; }
        public string Date { get; set; }
        public string ClientName { get; set; }
        public string PerformerName { get; set; }
        public string CarBrand { get; set; }
        public string CarModel { get; set; }
        public string RegisterPlate { get; set; }
    }
}