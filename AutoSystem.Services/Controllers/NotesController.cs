﻿using AutoSystem.DataLayer;
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
    public class NotesController : ApiController
    {
       private NotesRepository notesRepository;
       private RepairsRepository repairsRepository;

       public NotesController()
        {
            var context = new AutoSystemContext();
            this.notesRepository = new NotesRepository(context);
            this.repairsRepository = new RepairsRepository(context);
        }

        // api/notes/add
        [HttpPost]
        [ActionName("add")]
        public HttpResponseMessage Add([FromBody]Note value)
        {
            //check for empty properties (not needed)     

            Note newNote = new Note()
            {
                NoteId = value.NoteId,
                Text = value.Text,
                RepairID = value.RepairID
            };

            notesRepository.Add(newNote);

            return Request.CreateResponse(HttpStatusCode.Created, newNote);
        }

        //api/notes/all?repairId=23
        [HttpGet]
        [ActionName("all")]
        public HttpResponseMessage GetAllNotes(int repairId)
        {
            var repair = this.repairsRepository.GetById(repairId);
            var notes = new List<NoteModel>();

            foreach (var item in repair.Notes)
            {
                var newNote = new NoteModel()
                {
                    NoteId = item.NoteId,
                    Text = item.Text,
                    RepairId = item.RepairID
                };

                notes.Add(newNote);
            }

            return Request.CreateResponse(HttpStatusCode.OK, notes);
        }
	}
}