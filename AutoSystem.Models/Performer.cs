using System;
using System.Collections.Generic;

namespace AutoSystem.Models
{
    public class Performer
    {
        private ICollection<Repair> repairs;
        public Performer()
        {
            this.repairs = new HashSet<Repair>();
        }

        public int PerformerId { get; set; }

        public string Name { get; set; }

        public string Telephone { get; set; }

        public string Address { get; set; }

        public string Username { get; set; }

        public string AuthCode { get; set; }

        public string SessionKey { get; set; }
        
        public virtual ICollection<Repair> Repairs
        {
            get { return this.repairs; }
            set { this.repairs = value; }
        }
        
    }
}
