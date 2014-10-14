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

        // api/clients/all
        [HttpGet]
        [ActionName("all")]
        public HttpResponseMessage GetAllClients()
        {

            var clients = clientsRepository.All()
                .Select(u => new ClientModel()
                {
                    ClientId = u.ClientId,
                    Name = u.Name,
                    Address = u.Address,
                    Telephone = u.Telephone,
                    Performers = u.Performers.Select(c => new PerformerModel()
                    {
                        PerformerId = c.PerformerId,
                        Name = c.Name
                    })
                });

            return Request.CreateResponse(HttpStatusCode.OK, clients);
        }


        //api/performers/checkCars?id=23
        [HttpGet]
        [ActionName("checkCars")]
        public HttpResponseMessage CheckForRepairs([FromUri]int id)
        {

            var client = clientsRepository.Get(id);
            if (client == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid client id");
            }

            bool hasCars = this.clientsRepository.CheckForCars(id);

            return Request.CreateResponse(HttpStatusCode.OK, hasCars);
        }

        //api/clients/delete
        [HttpPost]
        [ActionName("delete")]
        public HttpResponseMessage DeletePerformer([FromBody]ClientModel value)
        {

            var client = clientsRepository.Get(value.ClientId);
            if (client == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid client id");
            }

            this.clientsRepository.Delete(value.ClientId);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        //api/clients/checkName?name=Jitan
        [HttpGet]
        [ActionName("checkName")]
        public HttpResponseMessage CheckUername([FromUri]string name)
        {
            ClientModel existingClient = new ClientModel();
            var client = clientsRepository.GetByName(name);
            if (client != null)
            {
                existingClient.ClientId = client.ClientId;
            }

            return Request.CreateResponse(HttpStatusCode.OK, existingClient);
        }

        // api/clients/addPerformers
        [HttpPost]
        [ActionName("addPerformers")]
        public HttpResponseMessage AddClients([FromBody]ClientPerformersModel model)
        {
            //Performer performer = performersRepository.Get(performerId);
            var performers = new List<Performer>();
            foreach (var id in model.PerformersIds)
            {
                Performer performer = performersRepository.Get(id);
                performers.Add(performer);
            }

            if (!this.clientsRepository.EditPerformers(performers, model.ClientId))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid clientID");
            }
            return Request.CreateResponse(HttpStatusCode.OK, "Performers Added to client");

        }
	}
}