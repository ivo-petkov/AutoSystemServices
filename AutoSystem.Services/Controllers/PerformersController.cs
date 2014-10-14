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
    public class PerformersController : ApiController
    {
        private PerformersRepository performersRepository;
        private ClientsRepository clientsRepository;
        private const int SessionKeyLength = 50;
        private const string SessionKeyChars =
            "qwertyuioplkjhgfdsazxcvbnmQWERTYUIOPLKJHGFDSAZXCVBNM";
        private static readonly Random rand = new Random();

        public PerformersController()
        {
            var context = new AutoSystemContext();
            this.performersRepository = new PerformersRepository(context);
            this.clientsRepository = new ClientsRepository(context);
        }

        [HttpGet]
        [ActionName("all")]
        public HttpResponseMessage GetAllPerformers()
        {

            var performers = performersRepository.All()
                .Select(u => new PerformerModel()
                {
                    PerformerId = u.PerformerId,
                    Username = u.Username,
                    Name = u.Name,
                    Address = u.Address,
                    Telephone = u.Telephone,
                    Email = u.Email,
                    Mol = u.Mol,
                    Bulstat = u.Bulstat,
                    Clients = u.Clients.Select(c => new ClientModel()
                    {
                        ClientId = c.ClientId,
                        Name = c.Name
                    })
                });

            return Request.CreateResponse(HttpStatusCode.OK, performers);
        }

        [HttpPost]
        [ActionName("edit")]
        public HttpResponseMessage EditPerformer([FromBody]PerformerModel value)
        {
            if (string.IsNullOrEmpty(value.Username) || string.IsNullOrWhiteSpace(value.Username)
               || value.Username.Length < 5 || value.Username.Length > 30)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                                              "Invalid username. Should be between 5 and 30 characters");
            }

            var performer = performersRepository.Get(value.PerformerId);
            if (performer == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid performer id");
            }

            var performerToEdit = new Performer()
                                 {
                                     PerformerId = performer.PerformerId,
                                     Username = value.Username,
                                     AuthCode = (value.AuthCode.ToLower() == "da39a3ee5e6b4b0d3255bfef95601890afd80709") ? performer.AuthCode : value.AuthCode,
                                     Name = value.Name,
                                     Address = value.Address,
                                     Telephone = value.Telephone,
                                     Mol = value.Mol,
                                     Bulstat = value.Bulstat,
                                     Email = value.Email
                                     
                                 };

            if (performersRepository.EditPerformer(performerToEdit))
            {   
                return Request.CreateResponse(HttpStatusCode.OK);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, "Could not edit user");
        }



        // api/performers/register
        [HttpPost]
        [ActionName("register")]
        public HttpResponseMessage Register([FromBody]Performer value)
        {
            if(string.IsNullOrEmpty(value.Username) || string.IsNullOrWhiteSpace(value.Username)
                || value.Username.Length < 5 || value.Username.Length > 30)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                                              "Invalid username. Should be between 5 and 30 characters");
            }

            if(performersRepository.GetByUsername(value.Username) != null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                                              "Вече съществува подизпълнител със същото потребителско име");
            }

            performersRepository.Add(value);
            var performerModel = new PerformerModel()
                                {
                                    PerformerId = value.PerformerId,
                                    Username = value.Username,
                                    Name = value.Name,
                                    Address = value.Address,
                                    Telephone = value.Telephone,
                                    Mol = value.Mol,
                                    Bulstat = value.Bulstat,
                                    Email = value.Email
                                };

            return Request.CreateResponse(HttpStatusCode.Created, performerModel);
        }

        //api/performers/login
        [HttpPost]
        [ActionName("login")]
        public HttpResponseMessage Login([FromBody]Performer value)
        {
            var performer = performersRepository.CheckLogin(value.Username, value.AuthCode);
            if (performer != null)
            {
                var sessionKey = GenerateSessionKey(performer.PerformerId);
                performersRepository.SetSessionKey(performer, sessionKey);

                var performerModel = new PerformerModel()
                                    {
                                        PerformerId = performer.PerformerId,
                                        SessionKey = sessionKey,
                                        Username = performer.Username,
                                        Name = performer.Name,
                                        Address = performer.Address,
                                        Telephone = performer.Telephone
                                    };

                return Request.CreateResponse(HttpStatusCode.OK, performerModel);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, "Invalid username or password");
            }
        }

        //api/performers/logout
        [HttpGet]
        [ActionName("logout")]
        public HttpResponseMessage Logout(
            [ValueProvider(typeof(HeaderValueProviderFactory<String>))] String sessionKey)
        {
            var performer = performersRepository.GetBySessionKey(sessionKey);
            if(performer == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid session key");
            }

            performersRepository.Logout(performer);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // api/performers/allclients
        [HttpGet]
        [ActionName("allclients")]
        public HttpResponseMessage GetAllClients(
            [ValueProvider(typeof(HeaderValueProviderFactory<String>))] String sessionKey)
        {
            Performer performer = performersRepository.GetBySessionKey(sessionKey);

            if (performer != null)
            {
                var clients = performer.Clients;

                IEnumerable<ClientModel> simpleClients = clients.Select(client => new ClientModel
                {
                    ClientId = client.ClientId,
                    Name = client.Name,
                    Address = client.Address,
                    Telephone = client.Telephone
                });

                return Request.CreateResponse(HttpStatusCode.OK, simpleClients);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid session key");
        }

        // api/performers/addclients
        [HttpPost]
        [ActionName("addclients")]
        public HttpResponseMessage AddClients([FromBody]PerformerClientsModel model)
        {
            //Performer performer = performersRepository.Get(performerId);
            var clients = new List<Client>();
            foreach (var id in model.ClientIds)
            {
                Client client = clientsRepository.Get(id);
                clients.Add(client);
            }

            if (!this.performersRepository.EditClients(clients, model.PerformerUsername))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid performerId");
            }
            return Request.CreateResponse(HttpStatusCode.OK, "Clients Added to performer");           
                        
        }

        //api/performers/delete
        [HttpPost]
        [ActionName("delete")]
        public HttpResponseMessage DeletePerformer([FromBody]PerformerModel value)
        {

            var performer = performersRepository.Get(value.PerformerId);
            if (performer == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid performer id");
            }

            this.performersRepository.Delete(value.PerformerId);
            
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        //api/performers/checkRepairs?id=23
        [HttpGet]
        [ActionName("checkRepairs")]
        public HttpResponseMessage CheckForRepairs([FromUri]int id)
        {

            var performer = performersRepository.Get(id);
            if (performer == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid performer id");
            }

            bool hasRepairs = this.performersRepository.CheckForRepairs(id);

            return Request.CreateResponse(HttpStatusCode.OK, hasRepairs);
        }


        //api/performers/checkUsername?username=Jitan
        [HttpGet]
        [ActionName("checkUsername")]
        public HttpResponseMessage CheckUsername([FromUri]string username)
        {
            PerformerModel existingPerformer = new PerformerModel();
            var performer = performersRepository.GetByUsername(username);
            if (performer != null)
            {
                existingPerformer.PerformerId = performer.PerformerId;
               
            }

            return Request.CreateResponse(HttpStatusCode.OK, existingPerformer);
        }

        private string GenerateSessionKey(int performerId)
        {
            StringBuilder skeyBuilder = new StringBuilder(SessionKeyLength);
            skeyBuilder.Append(performerId);
            while (skeyBuilder.Length < SessionKeyLength)
            {
                var index = rand.Next(SessionKeyChars.Length);
                skeyBuilder.Append(SessionKeyChars[index]);
            }
            return skeyBuilder.ToString();
        }
    }
}
