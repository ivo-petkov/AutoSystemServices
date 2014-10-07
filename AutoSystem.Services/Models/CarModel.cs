using AutoSystem.Models;
using System;
using System.Linq;

namespace AutoSystem.Services.Models
{
    public class CarModel
    {
        public int CarId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string EngineNumber { get; set; }
        public int EngineSize { get; set; }
        public string Chassis { get; set; }
        public string Engine { get; set; }
        public string RegisterPlate { get; set; }
        public int ClientId { get; set; }
        public ClientModel Client { get; set; }

    }
}