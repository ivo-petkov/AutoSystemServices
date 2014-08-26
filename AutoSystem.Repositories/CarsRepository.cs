using AutoSystem.DataLayer;
using AutoSystem.Models;
using System;


namespace AutoSystem.Repositories
{
    public class CarsRepository : EfRepository<Car>
    {
        private AutoSystemContext dbContext;

        public CarsRepository(AutoSystemContext context)
            : base(context)
        {
            this.dbContext = context;
        }

    }
}
