using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoSystem.Services.Models
{
    public class PerformerClientsModel
    {
        public string PerformerUsername { get; set; }
        public IEnumerable<int> ClientIds { get; set; }
    }
}