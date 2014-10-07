using AutoSystem.DataLayer;
using AutoSystem.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Web.Http;
using System.Web.Http.ValueProviders;
using AutoSystem.Models;
using AutoSystem.Services.Models;
using Forum.WebApi.Attributes;


namespace AutoSystem.Services.Controllers
{
    public class CarsController : ApiController
    {
        private CarsRepository carsRepository;

        public CarsController()
        {
            var context = new AutoSystemContext();
            this.carsRepository = new CarsRepository(context);
        }

        // api/cars/add
        [HttpPost]
        [ActionName("add")]
        public HttpResponseMessage Register([FromBody]Car value)
        {
            //check for empty properties (not needed)
            
            //check for already registered car
            if (carsRepository.GetByRegisterPlate(value.RegisterPlate) != null)
            {
                return Request.CreateResponse(HttpStatusCode.Accepted,
                                              "Car already registered!");
            }

            carsRepository.Add(value);

            var carModel = new CarModel()
            {
                CarId = value.CarId,
                Brand = value.Brand,
                Model = value.Model,
                Year = value.Year,
                EngineNumber = value.EngineNumber,
                Engine = value.Engine,
                EngineSize = value.EngineSize,
                Chassis = value.Chassis,
                RegisterPlate = value.RegisterPlate,
                ClientId = value.ClientId
            };

            return Request.CreateResponse(HttpStatusCode.Created, carModel);
        }


        //api/cars?id=23
        [HttpGet]
        public CarModel GetById(int id)
        {
            var car = this.carsRepository.Get(id);

            var model = new CarModel()
            {
                CarId = car.CarId,
                Brand = car.Brand,
                Model= car.Model,
                RegisterPlate = car.RegisterPlate,
                Engine = car.Engine,
                EngineSize = car.EngineSize,
                Chassis = car.Chassis,
                EngineNumber = car.EngineNumber,
                Year = car.Year,
                ClientId = car.ClientId
            };

            return model;
        }

        //api/cars?registeraPlate=CO6623AT
        [HttpGet]
        public bool CheckForExistingCar(string registerPlate)
        {
            var car = this.carsRepository.GetByRegisterPlate(registerPlate);
            if (car == null)
            {
                return false;
            }
            return true;
        }
	}
}