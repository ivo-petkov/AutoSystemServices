using System;
using System.Collections.Generic;

namespace AutoSystem.Models
{
    public class Client
    {
        private ICollection<Car> cars;

        private ICollection<Performer> performers;

        public Client()
        {
            this.cars = new HashSet<Car>();
            this.performers = new HashSet<Performer>();
        }

        public int ClientId { get; set; }

        public string Name { get; set; }

        public string Telephone { get; set; }

        public string Address { get; set; }        

        public virtual ICollection<Car> Cars
        {
            get { return this.cars; }
            set { this.cars = value; }
        }

        public virtual ICollection<Performer> Performers
        {
            get { return performers; }
            set { performers = value; }
        }               
    }
}
