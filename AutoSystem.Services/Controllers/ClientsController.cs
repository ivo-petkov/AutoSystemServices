using AutoSystem.DataLayer;
using AutoSystem.Models;
using AutoSystem.Repositories;
using AutoSystem.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace AutoSystem.Services.Controllers
{
    public class ClientsController : ApiController
    {
        private ClientsRepository clientsRepository;

        public ClientsController()
        {
            var context = new AutoSystemContext();
            this.clientsRepository = new ClientsRepository(context);
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
                Telephone = value.Address
            };

            clientsRepository.Add(client);

            return Request.CreateResponse(HttpStatusCode.Created, client);
        }
	}
}