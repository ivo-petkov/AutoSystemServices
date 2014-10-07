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
            repair.CostPrice = (updatedRepair.CostPrice != null) ? updatedRepair.CostPrice : repair.CostPrice;
            repair.FinalPrice = (updatedRepair.FinalPrice != null) ? updatedRepair.FinalPrice : repair.FinalPrice;
            repair.AdminStatus = (updatedRepair.AdminStatus != null) ? updatedRepair.AdminStatus : repair.AdminStatus;
            repair.PerformerId = (updatedRepair.PerformerId != null) ? updatedRepair.PerformerId : repair.PerformerId;
            repair.Date = (updatedRepair.Date != null) ? updatedRepair.Date : repair.Date;

            dbContext.SaveChanges();
            return true;
        }
    }
}
