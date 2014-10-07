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
    public class ClientsController : ApiController
    {
        private ClientsRepository clientsRepository;
        private PerformersRepository performersRepository;

        public ClientsController()
        {
            var context = new AutoSystemContext();
            this.clientsRepository = new ClientsRepository(context);
            this.performersRepository = new PerformersRepository(context);
        }

        // api/clients/add
        [HttpPost]
        [ActionName("add")]
        public HttpResponseMessage Add([FromBody]Client value)
        {
            //check for empty properties (not needed)
            
            //check for already registered client
            if (clientsRepository.CheckIfClientExist(value) != null)
            {
                return Request.CreateResponse(HttpStatusCode.Accepted,
                                              "Client already registered!");
            }            

            Client client = new Client()
            {
                ClientId = value.ClientId,
                Name = value.Name,
                Address = value.Address,
                Telephone = value.Telephone
            };

            clientsRepository.Add(client);

            return Request.CreateResponse(HttpStatusCode.Created, client);
        }


        //api/clients?id=23
        [HttpGet]
        public ClientModel GetById(int id)
        {
            var client = this.clientsRepository.Get(id);

            var model = new ClientModel()
            {
                ClientId = client.ClientId,
                Address = client.Address,
                Name = client.Name,
                Telephone = client.Telephone
            };

            return model;
        }


        // api/clients/getcars?id=23
        [HttpGet]
        [ActionName("getcars")]
        public HttpResponseMessage GetCars(int clientId,
            [ValueProvider(typeof(HeaderValueProviderFactory<String>))] String sessionKey)
        {
            //var clientCars = new List<CarModel>();
            var client = this.clientsRepository.Get(clientId);
            var performer = this.performersRepository.GetBySessionKey(sessionKey);
            var cars = client.Cars.Where(car => car.Repairs.Any(repair => repair.PerformerId == performer.PerformerId));

            IEnumerable<CarModel> clientCars = cars.Select(c => new CarModel
            {
                CarId = c.CarId,
                Brand = c.Brand,
                Model = c.Model,
                Year = c.Year,
                EngineNumber = c.EngineNumber,
                Engine = c.Engine,
                EngineSize = c.EngineSize,
                Chassis = c.Chassis,
                RegisterPlate = c.RegisterPlate
            });

            return Request.CreateResponse(HttpStatusCode.Created, clientCars);
        }
	}
}