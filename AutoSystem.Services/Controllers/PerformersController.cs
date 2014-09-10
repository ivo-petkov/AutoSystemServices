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
        public HttpResponseMessage GetAllUsers(
            [ValueProvider(typeof(HeaderValueProviderFactory<String>))] String sessionKey)
        {
            var performer = performersRepository.GetBySessionKey(sessionKey);
            if (performer != null)
            {
                var performers = performersRepository.All()
                    .Select(u => new PerformerModel()
                                     {
                                         PerformerId = u.PerformerId,
                                         Username = u.Username,
                                         Name = u.Name,
                                         Address = u.Address,
                                         Telephone = u.Telephone
                                     });

                return Request.CreateResponse(HttpStatusCode.OK, performers);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid session key");
        }

        [HttpPost]
        [ActionName("edit")]
        public HttpResponseMessage EditPerformer([FromBody]EditPerformerModel value,
            [ValueProvider(typeof(HeaderValueProviderFactory<String>))] String sessionKey)
        {
            var performer = performersRepository.GetBySessionKey(sessionKey);
            if (performer == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid session key");
            }

            var performerToEdit = new Performer()
                                 {
                                     PerformerId = performer.PerformerId,
                                     Username = performer.Username,
                                     Name = value.Name,
                                     Address = value.Address,
                                     Telephone = value.Telephone,
                                     AuthCode = value.OldAuthCode
                                 };

            if (performersRepository.EditPerformer(performerToEdit, value.NewAuthCode))
            {
                var updatedPerformer = performersRepository.Get(performerToEdit.PerformerId);
                var performerModel = new PerformerModel()
                {
                    PerformerId = updatedPerformer.PerformerId,
                    Username = updatedPerformer.Username,
                    SessionKey = sessionKey,
                    Name = updatedPerformer.Name,
                    Address = updatedPerformer.Address,
                    Telephone = updatedPerformer.Telephone
                };

                return Request.CreateResponse(HttpStatusCode.OK, performerModel);
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
                                              "Username already exists");
            }

            performersRepository.Add(value);
            var sessionKey = GenerateSessionKey(value.PerformerId);
            performersRepository.SetSessionKey(value, sessionKey);
            var performerModel = new PerformerModel()
                                {
                                    PerformerId = value.PerformerId,
                                    Username = value.Username,
                                    SessionKey = sessionKey,
                                    Name = value.Name,
                                    Address = value.Address,
                                    Telephone = value.Telephone
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
                var simpleClients = new List<ClientModel>();

                foreach (var item in clients)
                {
                    var newCliet = new ClientModel()
                    {
                        ClientId = item.ClientId,
                        Name = item.Name,
                        Address = item.Address,
                        Telephone = item.Telephone
                    };

                    simpleClients.Add(newCliet);
                }

                return Request.CreateResponse(HttpStatusCode.OK, simpleClients);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid session key");
        }

        // api/performers/addclient?performerId=23&clientId=23
        [HttpPost]
        [ActionName("addclient")]
        public HttpResponseMessage AddClient(int performerId, int clientId)
        {
            //Performer performer = performersRepository.Get(performerId);
            Client client = clientsRepository.Get(clientId);
            if (this.performersRepository.AddClient(client, performerId))
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Client Added to performer");
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid performerId");
                        
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
