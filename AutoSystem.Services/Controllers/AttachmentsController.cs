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
    public class AttachmentsController : ApiController
    {
       private AttachmentsRepository attachmentsRepository;

       public AttachmentsController()
        {
            var context = new AutoSystemContext();
            this.attachmentsRepository = new AttachmentsRepository(context);
        }


        // api/attachments/delete?id=23
        [HttpPost]
        [ActionName("delete")]
        public HttpResponseMessage Register(int id)
        {

            if (attachmentsRepository.Get(id) == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                                              "Invalid attachment id.");
            }

            attachmentsRepository.Delete(id);

            return Request.CreateResponse(HttpStatusCode.OK, "Attachment deleted.");
        }
	}
}