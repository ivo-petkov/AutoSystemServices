using AutoSystem.DataLayer;
using AutoSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSystem.Repositories
{
    public class PartsRepository : EfRepository<Parts>
    {
        private AutoSystemContext dbContext;

        public PartsRepository(AutoSystemContext context)
            : base(context)
        {
            this.dbContext = context;
        }

        public void EditRepairParts(ICollection<Parts> editedParts, int repairId)
        {
            foreach (var partsInfo in editedParts)
            {
                if (!EditParts(partsInfo))
                {
                    AddParts(partsInfo, repairId);
                }
            }
        }

        public bool EditParts(Parts value)
        {
            var parts = dbContext.Parts.Find(value.PartsId);

            if (parts == null)
            {
                return false;
            }

            if (String.IsNullOrWhiteSpace(value.Text) || String.IsNullOrEmpty(value.Text))
            {
                parts.Text = null;
            }
            else
            {
                parts.Text = value.Text;
            }

            if (String.IsNullOrWhiteSpace(value.PriceInfo) || String.IsNullOrEmpty(value.PriceInfo))
            {
                parts.PriceInfo = null;
            }
            else
            {
                parts.PriceInfo = value.PriceInfo;
            }

            if (String.IsNullOrWhiteSpace(value.Provider) || String.IsNullOrEmpty(value.Provider))
            {
                parts.Provider = null;
            }
            else
            {
                parts.Provider = value.Provider;
            }

            dbContext.SaveChanges();
            return true;
        }

        public void AddParts(Parts parts, int repairId)
        {
            Parts newParts = new Parts()
            {
                Text = parts.Text,
                RepairId = repairId,
                Provider = parts.Provider,
                PriceInfo = parts.PriceInfo
            };

            this.Add(newParts);
            dbContext.SaveChanges();
           
        }
    }
}
