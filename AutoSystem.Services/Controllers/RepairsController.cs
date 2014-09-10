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
        private ClientsRepository clientsRepository;

        public RepairsController()
        {
            var context = new AutoSystemContext();
            this.repairsRepository = new RepairsRepository(context);
            this.performersRepository = new PerformersRepository(context);
            this.carsRepository = new CarsRepository(context);
            this.clientsRepository = new ClientsRepository(context);
        }



        //api/repairs/get?id=23
        [HttpGet]
        public HttpResponseMessage GetById(int id)
        {
            var repair = this.repairsRepository.Get(id);

            if (repair == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid RepairId");
            }

            var repairNotes = new List<NoteModel>();

            foreach (var item in repair.Notes)
            {
                var newNote = new NoteModel() 
                {
                    RepairId = item.RepairID,
                    Text = item.Text,
                    NoteId = item.NoteId
                };
                repairNotes.Add(newNote);
            }

            var model = new RepairModel()
            {
                RepairId = repair.RepairId,
                Status = repair.Status,
                Date = repair.Date.ToString(),
                Milage = repair.Milage,
                FinalPrice = repair.FianlePrice,
                PerformerPrice = repair.PerformerPrice,
                CarId = repair.CarId,
                PerformerId = repair.PerformerId,
                Notes = repairNotes,
                Parts = repair.Parts,
                Attachments = repair.Attachments
            };

            return Request.CreateResponse(HttpStatusCode.OK, model);
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
                FianlePrice = value.FianlePrice,
                Status = value.Status,

            };

            repairsRepository.Add(repairToAdd);


            /// very questionable
            var repairModel = new RepairModel()
            {
                RepairId = value.RepairId,
                Date = value.Date.ToString(),
                Milage = value.Milage,
                FinalPrice = value.FianlePrice,
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
                        Date = item.Date.ToString(),
                        ClientName = item.Car.Client.Name
                    };

                    simpleRepairs.Add(newRepair);
                }

                return Request.CreateResponse(HttpStatusCode.OK, simpleRepairs);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid session key");
        }

        ////api/repairs/edit
        //[HttpPost]
        //[ActionName("edit")]
        //public HttpResponseMessage EditPerformer([FromBody]Repair editedRepairData)
        //{
        //    int repairId = editedRepairData.RepairId;
        //    var repair = repairsRepository.GetById(repairId);

        //    if (repair == null)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid RepairId");
        //    }

        //    var uneditedCar = this.carsRepository.GetById(editedRepairData.CarId);
        //    var uneditedClient = this.clientsRepository.GetById(uneditedCar.ClientId);


        //    Car editedCarData = editedRepairData.Car;
        //    Client editedClientData = editedCarData.Client;

        //    //check if client is entirely new or edited
        //    if (uneditedClient.Name != editedClientData.Name && uneditedClient.Address != editedClientData.Address && uneditedClient.Telephone != editedClientData.Telephone)
        //    { 

        //    }


        //    var repairNotesModel = new List<NoteModel>();
        //    var repairPartsModel = new List<PartsModel>();
        //    var repairAttachmentsModel = new List<AttachmentModel>();

        //    //adding the notes of the repair
        //    foreach (var note in editedRepairData.Notes)
        //    {
        //        NoteModel newNote = new NoteModel()
        //        {
        //            NoteId = note.NoteId,
        //            Text = note.Text,
        //            RepairId = note.RepairId
        //        };
        //        repairNotesModel.Add(newNote);
        //    }

        //    //adding the attachments of the repair
        //    foreach (var attachment in editedRepairData.Attachments)
        //    {
        //        AttachmentModel newAttachment = new AttachmentModel()
        //        {
        //            AttachmentId = attachment.AttachmentId,
        //        };
        //    }

        //    var repairToEdit = new Repair()
        //    {
        //        PerformerId = repair.PerformerId,
        //        Username = repair.Username,
        //        Name = editedRepairData.Name,
        //        Address = editedRepairData.Address,
        //        Telephone = editedRepairData.Telephone,
        //        AuthCode = editedRepairData.OldAuthCode
        //    };

        //    if (performersRepository.EditPerformer(repairToEdit, editedRepairData.NewAuthCode))
        //    {
        //        var updatedPerformer = performersRepository.Get(repairToEdit.PerformerId);
        //        var performerModel = new PerformerModel()
        //        {
        //            PerformerId = updatedPerformer.PerformerId,
        //            Username = updatedPerformer.Username,
        //            SessionKey = sessionKey,
        //            Name = updatedPerformer.Name,
        //            Address = updatedPerformer.Address,
        //            Telephone = updatedPerformer.Telephone
        //        };

        //        return Request.CreateResponse(HttpStatusCode.OK, performerModel);
        //    }

        //    return Request.CreateResponse(HttpStatusCode.BadRequest, "Could not edit user");
        //}
        
	}
}