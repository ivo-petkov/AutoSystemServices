using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoSystem.Services.Models
{
    public class ClientPerformersModel
    {
        public int ClientId { get; set; }
        public IEnumerable<int> PerformersIds { get; set; }
    }
}