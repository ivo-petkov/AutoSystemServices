using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoSystem.Services.Models
{
    public class ClientModel
    {
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string Telephone { get; set; }
        public string Address { get; set; }
        public string Bulstat { get; set; }
        public string Email { get; set; }
        public string Mol { get; set; }
        public IEnumerable<CarModel> Cars { get; set; }
    }
}