using AutoSystem.DataLayer;
using AutoSystem.Models;
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
    public class RepairsController : ApiController
    {
        private RepairsRepository repairsRepository;
        private PerformersRepository performersRepository;
        private CarsRepository carsRepository;

        public RepairsController()
        {
            var context = new AutoSystemContext();
            this.repairsRepository = new RepairsRepository(context);
            this.performersRepository = new PerformersRepository(context);
            this.carsRepository = new CarsRepository(context);
        }

        // api/repairs/add
        [HttpPost]
        [ActionName("add")]
        public HttpResponseMessage Add([FromBody]Repair value,
            [ValueProvider(typeof(HeaderValueProviderFactory<String>))] String sessionKey)
        {
            //check for empty properties (not needed)
            
            //check for already registered repair (not needed)

            Performer performer = performersRepository.GetBySessionKey(sessionKey);
            Car car = carsRepository.GetById(value.CarId);
            Repair repairToAdd = new Repair()
            {
                Car = car,
                Performer = performer,
                Date = DateTime.Now,
                Milage = value.Milage,
                Price = value.Price,
                Status = value.Status,

            };

            repairsRepository.Add(repairToAdd);


            /// very questionable
            var repairModel = new RepairModel()
            {
                RepairId = value.RepairId,
                Date = DateTime.Now,
                Milage = value.Milage,
                Price = value.Price,
                Status = value.Status,
                PerformerId = performer.PerformerId,
                CarId = car.CarId,
            };


            return Request.CreateResponse(HttpStatusCode.Created, repairModel);
        }


        // api/repairs/all
        [HttpGet]
        [ActionName("all")]
        public HttpResponseMessage GetAllRepairs(
            [ValueProvider(typeof(HeaderValueProviderFactory<String>))] String sessionKey)
        {
            Performer performer = performersRepository.GetBySessionKey(sessionKey);          

            if (performer != null)
            {
                var repairs = performer.Repairs;
                var simpleRepairs = new List<SimpleRepairModel>();

                foreach (var item in repairs)
                {
                    var newRepair = new SimpleRepairModel()
                    {
                        RepairId = item.RepairId,
                        Status = item.Status,
                        Date = item.Date.ToString()
                    };

                    simpleRepairs.Add(newRepair);
                }

                return Request.CreateResponse(HttpStatusCode.OK, simpleRepairs);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid session key");
        }



	}
}