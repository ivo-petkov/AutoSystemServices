using System;
using System.Collections.Generic;

namespace AutoSystem.Models
{
    public class Car
    {
        private ICollection<Repair> repairs;

        public Car()
        {
            this.repairs = new HashSet<Repair>();
        }

        public int CarId { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public int Year { get; set; }

        public string Town { get; set; }

        public string Telephone { get; set; }

        public decimal EngineSize { get; set; }

        public string Chassis { get; set; }

        public string Engine { get; set; }

        public string RegisterPlate { get; set; }

        public int ClientId { get; set; }
        public virtual Client Client { get; set; }     

        public virtual ICollection<Repair> Repairs
        {
            get { return repairs; }
            set { repairs = value; }
        }
        
    }
}
