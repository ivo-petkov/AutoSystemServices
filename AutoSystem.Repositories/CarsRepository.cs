using AutoSystem.DataLayer;
using AutoSystem.Models;
using System;
using System.Linq;


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

        public Car GetByRegisterPlate(string registerPlate)
        {
            return dbContext.Cars.FirstOrDefault(u => u.RegisterPlate == registerPlate);
        }

        public Car GetById(int id)
        {
            return dbContext.Cars.FirstOrDefault(u => u.CarId == id);
        }
    }
}
