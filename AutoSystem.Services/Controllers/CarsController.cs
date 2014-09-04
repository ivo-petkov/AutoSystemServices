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
                Telephone = value.Telephone,
                Town = value.Town,
                Engine = value.Engine,
                EngineSize = value.EngineSize,
                Chassis = value.Chassis,
                RegisterPlate = value.RegisterPlate,
                ClientId = value.ClientId
            };

            return Request.CreateResponse(HttpStatusCode.Created, carModel);
        }
	}
}