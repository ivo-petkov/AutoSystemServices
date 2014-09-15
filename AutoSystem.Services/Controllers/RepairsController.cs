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

            IEnumerable<NoteModel> repairNotes = repair.Notes.Select(n => new NoteModel
            {
                RepairId = n.RepairID,
                Text = n.Text,
                NoteId = n.NoteId
            });

            IEnumerable<PartsModel> repairParts = repair.Parts.Select(p => new PartsModel
            {
                RepairId = p.RepairId,
                PartsId = p.PartsId,
                Text = p.Text,
                PriceInfo = p.PriceInfo
            });

            IEnumerable<AttachmentModel> repairAttachments = repair.Attachments.Select(a => new AttachmentModel
            {
                RepairId = a.RepairId,
                AttachmentId = a.AttachmentId,
                Name = a.Name,
                Data = a.Data,
                DocumentType = a.DocumentType,
                FileFormat = a.FileFormat
            });

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
                Parts = repairParts,
                Attachments = repairAttachments
            };

            return Request.CreateResponse(HttpStatusCode.OK, repairModel);
        }


        //api/repairs/filter?RegisterPlate=ВР8000АХ&Brand=Ford
        [HttpGet]
        [ActionName("filter")]
        public IHttpActionResult GetByFilter(
            [ValueProvider(typeof(HeaderValueProviderFactory<String>))] String sessionKey,
            [FromUri]FilterQuery filter)
        {
            Performer performer = performersRepository.GetBySessionKey(sessionKey);
            if (performer == null)
            {
                return BadRequest("Invalid session key");
            }
            
            DateTime? formatedStartDate = DateTime.ParseExact(filter.StartDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            DateTime? formatedEndtDate = DateTime.ParseExact(filter.EndDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            IEnumerable<Repair> performerRepairs = performer.Repairs;    
        
            var filteredRepairs = performerRepairs.Where(r => r.RepairId != null 
                && (filter.RegisterPlate != null ? r.Car.RegisterPlate.ToLower().Contains(filter.RegisterPlate.ToLower()) : true)
                && (filter.Brand != null ? r.Car.Brand == filter.Brand : true)
                && (filter.ClientId != null ? r.Car.ClientId == filter.ClientId : true)
                && (filter.Status != null ? (int)(r.Status) == filter.Status : true)
                && (formatedStartDate != null ? r.Date.Year >= formatedStartDate.Value.Year && r.Date.Month >= formatedStartDate.Value.Month && r.Date.Day >= formatedStartDate.Value.Day : true)
                && (formatedEndtDate != null ? r.Date.Year <= formatedEndtDate.Value.Year && r.Date.Month <= formatedEndtDate.Value.Month && r.Date.Day <= formatedEndtDate.Value.Day : true)
                );
              

            IEnumerable<SimpleRepairModel> resposeRepairs = filteredRepairs.Select(r => new SimpleRepairModel
                                                                                            {
                                                                                                RepairId = r.RepairId,
                                                                                                ClientName = r.Car.Client.Name,
                                                                                                Status = r.Status,
                                                                                                Date = r.Date.ToString("dd/MM/yy HH:mm")
                                                                                            });
            var orederedRepairs = resposeRepairs.OrderByDescending(r => r.RepairId).ToList();

            return Ok(orederedRepairs);
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

                IEnumerable<SimpleRepairModel> simpleRepairs = repairs.Select(r => new SimpleRepairModel
                {
                    RepairId = r.RepairId,
                    ClientName = r.Car.Client.Name,
                    Status = r.Status,
                    Date = r.Date.ToString("dd/MM/yy HH:mm")
                });
                var orederedRepairs = simpleRepairs.OrderByDescending(r => r.RepairId).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, orederedRepairs);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid session key");
        }


        // api/repairs/add
        [HttpPost]
        [ActionName("add")]
        public HttpResponseMessage Add([ValueProvider(typeof(HeaderValueProviderFactory<String>))] String sessionKey,
            [FromBody]RepairModel repair)
        {

            Performer performer = performersRepository.GetBySessionKey(sessionKey);
            if (performer == null)
            {
                 return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid session key");
            }
            //check for empty properties (not needed)
            //check for already registered repair (not needed)

            CarModel car = repair.Car;

            Car newCar = new Car()
            {
                Brand = car.Brand,
                Model = car.Model,
                RegisterPlate = car.RegisterPlate,
                Telephone = car.Telephone,
                Town = car.Town,
                Year = car.Year,
                Chassis = car.Chassis,
                Engine = car.Engine,
                EngineSize = car.EngineSize,
                ClientId = car.ClientId,
            };

            Car existingCar = carsRepository.GetByRegisterPlate(newCar.RegisterPlate);
            if (existingCar == null)
            {
                carsRepository.Add(newCar);
            }
            else
            {
                if (!carsRepository.EditCar(newCar, existingCar.CarId))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Could not edit existing car.");
                }
            }

            //Creating repair's notes 
            IEnumerable<Note> repairNotes = repair.Notes.Select(n => new Note
            {
                Text = n.Text
            });            

            //Creating repair's parts  
            IEnumerable<Parts> repairParts = repair.Parts.Select(p => new Parts
            {
                Text = p.Text,
                PriceInfo = p.PriceInfo
            });      
            
            //Creating repair's attachments
            IEnumerable<Attachment> repairAttachments = repair.Attachments.Select(a => new Attachment
            {
                Name = a.Name,
                Data = a.Data,
                DocumentType = a.DocumentType,
                FileFormat = a.FileFormat
            });


            Car repairCar = carsRepository.GetByRegisterPlate(repair.Car.RegisterPlate);

            Repair repairToAdd = new Repair()
            {
                CarId = repairCar.CarId,
                PerformerId = performer.PerformerId,
                Date = DateTime.Now,
                Milage = repair.Milage,
                FianlePrice = repair.FinalPrice,
                PerformerPrice = repair.PerformerPrice,
                IsEditable = repair.IsEditable,
                Status = repair.Status
            };

            if (repairNotes != null)
	        {
                repairToAdd.Notes = repairNotes.ToList();
	        }
            if (repairParts != null)
            {
                repairToAdd.Parts = repairParts.ToList();
            }
            if (repairAttachments != null)
            {
                repairToAdd.Attachments = repairAttachments.ToList();
            }

            repairsRepository.Add(repairToAdd);       
            return Request.CreateResponse(HttpStatusCode.Created, "Repair Created.");
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

            var uneditedCar = this.carsRepository.GetById(editedRepairData.Car.CarId);
            CarModel editedCarData = editedRepairData.Car;

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
            EditAtachments(editedRepairData);



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
            IEnumerable<Note> editedNotesData = editedRepairData.Notes.Select(n => new Note
            {
                NoteId = n.NoteId,
                Text = n.Text,
                RepairID = n.RepairId
            });

            notesRepository.EditRepairNotes(editedNotesData.ToList(), editedRepairData.RepairId);
        }

        private void EditParts(RepairModel editedRepairData)
        {
            IEnumerable<Parts> editedPartsData = editedRepairData.Parts.Select(p => new Parts
            {
                PartsId = p.PartsId,
                Text = p.Text,
                PriceInfo = p.PriceInfo,
                RepairId = p.RepairId
            });

            partsRepository.EditRepairParts(editedPartsData.ToList(), editedRepairData.RepairId);
        }

        private void EditAtachments(RepairModel editedRepairData)
        {
            IEnumerable<Attachment> editedAttachmentsData = editedRepairData.Attachments.Select(a => new Attachment
            {
                AttachmentId = a.AttachmentId,
                RepairId = a.RepairId,
                Name = a.Name,
                Data = a.Data,
                DocumentType = a.DocumentType,
                FileFormat = a.FileFormat
            });

            attachmentsRepository.EditRepairAttachment(editedAttachmentsData.ToList(), editedRepairData.RepairId);
        }        
	}
}