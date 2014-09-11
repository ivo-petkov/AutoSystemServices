using AutoSystem.DataLayer;
using AutoSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoSystem.Repositories
{
    public class RepairsRepository: EfRepository<Repair>
    {
        private AutoSystemContext dbContext;

        public RepairsRepository(AutoSystemContext context)
            : base(context)
        {
            this.dbContext = context;
        }

        public Repair GetById(int repairId)
        {
            return dbContext.Repairs.FirstOrDefault(u => u.RepairId == repairId);
        }


        public bool EditRepair(Repair updatedRepair, int repairId)
        {
            var repair = dbContext.Repairs.Find(repairId);
            if (repair  == null)
            {               
                return false;                
            }

            repair.RepairId = (updatedRepair.RepairId != null) ? updatedRepair.RepairId : repair.RepairId;
            repair.CarId = (updatedRepair.CarId != null) ? updatedRepair.CarId : repair.CarId;
            repair.Status = (updatedRepair.Status != null) ? updatedRepair.Status : repair.Status;
            repair.Milage = (updatedRepair.Milage != null) ? updatedRepair.Milage : repair.Milage;
            repair.PerformerPrice = (updatedRepair.PerformerPrice != null) ? updatedRepair.PerformerPrice : repair.PerformerPrice;
            repair.FianlePrice = (updatedRepair.FianlePrice != null) ? updatedRepair.FianlePrice : repair.FianlePrice;
            repair.IsEditable = (updatedRepair.IsEditable != null) ? updatedRepair.IsEditable : repair.IsEditable;
            repair.PerformerId = (updatedRepair.PerformerId != null) ? updatedRepair.PerformerId : repair.PerformerId;
            repair.Date = DateTime.Now;
            repair.Notes = (updatedRepair.Notes != null) ? updatedRepair.Notes : repair.Notes;
            repair.Parts = (updatedRepair.Parts != null) ? updatedRepair.Parts : repair.Parts;
            repair.Attachments = (updatedRepair.Attachments != null) ? updatedRepair.Attachments : repair.Attachments;

            dbContext.SaveChanges();
            return true;
        }
    }
}
