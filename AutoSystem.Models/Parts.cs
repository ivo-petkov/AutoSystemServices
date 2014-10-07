using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSystem.Models
{
    public class Parts
    {
        public int PartsId { get; set; }

        public string Text { get; set; }

        public string Provider { get; set; }

        public string PriceInfo { get; set; }

        public int RepairId { get; set; }
        public virtual Repair Repair { get; set; }
    }
}
