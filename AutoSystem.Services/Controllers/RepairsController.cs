﻿using AutoSystem.DataLayer;
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
using System.Globalization;


namespace AutoSystem.Services.Controllers
{
    public class RepairsController : ApiController
    {
        private RepairsRepository repairsRepository;
        private PerformersRepository performersRepository;
        private CarsRepository carsRepository;
        private ClientsRepository clientsRepository;
        private NotesRepository notesRepository;
        private PartsRepository partsRepository;
        private AttachmentsRepository attachmentsRepository;

        public RepairsController()
        {
            var context = new AutoSystemContext();
            this.repairsRepository = new RepairsRepository(context);
            this.performersRepository = new PerformersRepository(context);
            this.carsRepository = new CarsRepository(context);
            this.clientsRepository = new ClientsRepository(context);
            this.notesRepository = new NotesRepository(context);
            this.partsRepository = new PartsRepository(context);
            this.attachmentsRepository = new AttachmentsRepository(context);
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

            Car repairCar = repair.Car;
            Client carClient = repairCar.Client;

            var clientModel = new ClientModel() 
            {
                ClientId = carClient.ClientId,
                Name = carClient.Name,
                Telephone = carClient.Telephone,
                Address = carClient.Address
            };

            var carModel = new CarModel()
            {
                CarId = repairCar.CarId,
                RegisterPlate = repairCar.RegisterPlate,
                Model = repairCar.Model,
                Brand = repairCar.Brand,
                Year = repairCar.Year,
                Town = repairCar.Town,
                Telephone = repairCar.Telephone,
                Engine = repairCar.Engine,
                EngineSize = repairCar.EngineSize,
                Chassis = repairCar.Chassis,
                Client = clientModel
            };
           

            var repairModel = new RepairModel()
            {
                RepairId = repair.RepairId,
                Status = repair.Status,
                Date = repair.Date.ToString(),
                Milage = repair.Milage,
                FinalPrice = repair.FianlePrice,
                PerformerPrice = repair.PerformerPrice,
                CarId = repair.CarId,
                Car = carModel,
                PerformerId = repair.PerformerId,
                Notes = repairNotes,
                Parts = repair.Parts,
                Attachments = repair.Attachments
            };

            return Request.CreateResponse(HttpStatusCode.OK, repairModel);
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

        //api/repairs/edit
        [HttpPost]
        [ActionName("edit")]
        public HttpResponseMessage EditPerformer([FromBody]RepairModel editedRepairData)
        {
            int repairId = editedRepairData.RepairId;
            var repair = repairsRepository.GetById(repairId);

            if (repair == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid RepairId");
            }

            var uneditedCar = this.carsRepository.GetById(editedRepairData.CarId);
            CarModel editedCarData = editedRepairData.Car;
            //editedCarData.ClientId = editedRepairData.ClientId;
            Car newCar = new Car()
            {
                Brand = editedCarData.Brand,
                Model = editedCarData.Model,
                RegisterPlate = editedCarData.RegisterPlate,
                Telephone = editedCarData.Telephone,
                Town = editedCarData.Town,
                Year = editedCarData.Year,
                Chassis = editedCarData.Chassis,
                Engine = editedCarData.Engine,
                EngineSize = editedCarData.EngineSize,
                ClientId = editedCarData.ClientId,
            };

            if (editedRepairData.Car.RegisterPlate != uneditedCar.RegisterPlate)
            {
                carsRepository.Add(newCar);
            }
            else
            {
                Car existingCar = carsRepository.GetByRegisterPlate(editedRepairData.Car.RegisterPlate);

                if (!carsRepository.EditCar(newCar, existingCar.CarId))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Could not edit car");
                }
            }

            Car updatedCar = carsRepository.GetByRegisterPlate(editedRepairData.Car.RegisterPlate);

            EditNotes(editedRepairData);
            EditParts(editedRepairData);
            //EditAtachments(editedRepairData);



            DateTime noRoundtripDate = DateTime.Parse(editedRepairData.Date, null,
                                          DateTimeStyles.None);           

            Repair updatedRepair = new Repair()
            {
                RepairId = editedRepairData.RepairId,
                Date = noRoundtripDate,
                Status = editedRepairData.Status,
                Milage = editedRepairData.Milage,
                FianlePrice = editedRepairData.FinalPrice,
                PerformerPrice = editedRepairData.PerformerPrice,
                IsEditable = editedRepairData.IsEditable,
                CarId = updatedCar.CarId,
                PerformerId = editedRepairData.PerformerId
            };

            if (!repairsRepository.EditRepair(updatedRepair, editedRepairData.RepairId))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Could not edit repair");
            }
            return Request.CreateResponse(HttpStatusCode.OK,"Repair Edited.");

        }

        private void EditNotes(RepairModel editedRepairData)
        {
            List<Note> editedNotesData = new List<Note>();
            foreach (var note in editedRepairData.Notes)
            {
                Note newNote = new Note()
                {
                    NoteId = note.NoteId,
                    Text = note.Text,
                    RepairID = note.RepairId
                };

                editedNotesData.Add(newNote);
            }
            notesRepository.EditRepairNotes(editedNotesData, editedRepairData.RepairId);
        }

        private void EditParts(RepairModel editedRepairData)
        {
            List<Parts> editedPartsData = new List<Parts>();
            foreach (var partsInfo in editedRepairData.Parts)
            {
                Parts newParts = new Parts()
                {
                    PartsId = partsInfo.PartsId,
                    Text = partsInfo.Text,
                    PriceInfo = partsInfo.PriceInfo,
                    RepairId = partsInfo.RepairId                     
                };

                editedPartsData.Add(newParts);
            }
            partsRepository.EditRepairParts(editedPartsData, editedRepairData.RepairId);
        }

        //private void EditAtachments(RepairModel editedRepairData)
        //{
        //    List<Note> editedNotesData = new List<Note>();
        //    foreach (var note in editedRepairData.Notes)
        //    {
        //        Note newNote = new Note()
        //        {
        //            NoteId = note.NoteId,
        //            Text = note.Text,
        //            RepairID = note.RepairId
        //        };

        //        editedNotesData.Add(newNote);
        //    }
        //    notesRepository.EditRepairNotes(editedNotesData, editedRepairData.RepairId);
        //}
        
	}
}