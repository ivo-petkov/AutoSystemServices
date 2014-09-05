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
using System.Web.Http.Description;


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
                Date = value.Date.ToString(),
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

        //api/posts?keyword=web-services

        //public RepairModel GetById(string id,
        //    [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey)
        //{
        //    var models = this.GetAll(sessionKey)
        //        .Where(p => p.Title.Split(Separators, StringSplitOptions.RemoveEmptyEntries).Contains(keyword));
        //    return models;
        //}

        public RepairModel GetById(int id)
        {
            var repair = this.repairsRepository.Get(id);

            var model = new RepairModel()
            {
                RepairId = repair.RepairId,
                Status = repair.Status,
                Date = repair.Date.ToString(),
                Milage = repair.Milage,
                Price = repair.Price,
                CarId = repair.CarId,
                PerformerId = repair.PerformerId,
                Notes = repair.Notes,
                Attachments = repair.Attachments
            };

            return model;
        }
        //// GET api/repairs/12
        //[Route("api/repairs/{RepairId}")]
        //[ResponseType(typeof(RepairModel))]
        //public IHttpActionResult GetPersonsFromCompany(int id, ODataQueryOptions<Person> opts)
        //{
        //    IQueryable<Person> all = _peopleRepository.GetAll(p => p.Company.CompanyId == id, p => p.AllowedAction, p => p.AllowedActionNoEl, p => p.Company, p => p.AllowedActionGen).AsQueryable();

        //    if (opts == null)
        //        return Ok(all);

        //    IQueryable<Person> queryResults = opts.ApplyTo(all) as IQueryable<Person>;
        //    if (opts.InlineCount != null && opts.InlineCount.Value == InlineCountValue.AllPages)
        //    {
        //        JObject obj = new JObject();
        //        obj.Add("count", queryResults.Count());
        //        return Ok(obj);
        //    }
        //    else
        //    {
        //        return Ok(queryResults);
        //    }

        //}

        //// api/repairs/all
        //[HttpGet]
        //[ActionName("all")]
        //public HttpResponseMessage GetById(
        //    [ValueProvider(typeof(HeaderValueProviderFactory<String>))] String sessionKey)
        //{
        //    Performer performer = performersRepository.GetBySessionKey(sessionKey);

        //    if (performer != null)
        //    {
        //        var repairs = performer.Repairs;
        //        var simpleRepairs = new List<SimpleRepairModel>();

        //        foreach (var item in repairs)
        //        {
        //            var newRepair = new SimpleRepairModel()
        //            {
        //                RepairId = item.RepairId,
        //                Status = item.Status,
        //                Date = item.Date.ToString()
        //            };

        //            simpleRepairs.Add(newRepair);
        //        }

        //        return Request.CreateResponse(HttpStatusCode.OK, simpleRepairs);
        //    }

        //    return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid session key");
        //}



	}
}