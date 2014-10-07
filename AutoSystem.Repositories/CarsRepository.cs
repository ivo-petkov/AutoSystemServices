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

        public bool EditCar(Car editedCarData, int id)
        {
            var car = dbContext.Cars.Find(id);
            car.Brand = editedCarData.Brand;
            car.Model = editedCarData.Model;
            car.Year = editedCarData.Year;
            car.RegisterPlate = editedCarData.RegisterPlate;
            car.EngineNumber = editedCarData.EngineNumber;
            car.Chassis = editedCarData.Chassis;
            car.Engine = editedCarData.Engine;
            car.EngineSize = editedCarData.EngineSize;
            car.ClientId = editedCarData.ClientId;

            dbContext.SaveChanges();
            return true;
        }
    }
}
