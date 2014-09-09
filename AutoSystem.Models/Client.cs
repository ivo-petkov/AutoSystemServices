using System;
using System.Collections.Generic;

namespace AutoSystem.Models
{
    public class Client
    {
        private ICollection<Car> cars;

        public Client()
        {
            this.cars = new HashSet<Car>();
        }

        public int ClientId { get; set; }

        public string Name { get; set; }

        public string Telephone { get; set; }

        public string Address { get; set; }        

        public ICollection<Car> Cars
        {
            get { return this.cars; }
            set { this.cars = value; }
        }
        
    }
}
