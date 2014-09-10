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

        ////api/notes/edit
        //[HttpPost]
        //[ActionName("edit")]
        //public HttpResponseMessage EditNote([FromBody]IEnumerable<NoteModel> editedNotes)
        //{
        //    if (editedNotes == null)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.OK, "No notes to edit.");
        //    }

        //    var repairId = editedNotes.First().RepairId;
        //    var repair = this.repairsRepository.GetById(repairId);
        //    if (repair == null)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid RepairId");
        //    }

        //    foreach (var note in editedNotes)
        //    {
        //        var newNote = this.notesRepository.GetById(note.NoteId);
        //        if (newNote == null)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid NoteId");
        //        }

        //        var noteToEdit = new Note()
        //        {
        //            NoteId = newNote.NoteId,
        //            Text = newNote.Text,
        //            RepairID = newNote.RepairID,
        //            Repair = newNote.Repair
        //        };

        //        if (notesRepository.EditNote(noteToEdit))
        //        {
        //            var updatedNote = notesRepository.Get(noteToEdit.NoteId);
        //            var noteModel = new NoteModel()
        //            {
        //                NoteId = updatedNote.NoteId,
        //                Text = updatedNote.Text,
        //                RepairId = updatedNote.RepairID,
        //            };

        //            //add
        //            return Request.CreateResponse(HttpStatusCode.OK, noteModel);

        //        }   

        //    }           

        //    return Request.CreateResponse(HttpStatusCode.BadRequest, "Could not edit notes");
        //}
	}
}