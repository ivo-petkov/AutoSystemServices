using AutoSystem.DataLayer;
using AutoSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoSystem.Repositories
{
    public class NotesRepository  : EfRepository<Note>
    {
        private AutoSystemContext dbContext;

        public NotesRepository(AutoSystemContext context)
            : base(context)
        {
            this.dbContext = context;
        }       
        
        public void EditRepairNotes(ICollection<Note> editedNotes, int repairId)
        {
            foreach (var note in editedNotes)
            {
                if (!EditNote(note))
                {
                    AddNote(note, repairId);
                }
            }
        }

        public bool EditNote(Note value)
        {
            var note = dbContext.Notes.Find(value.NoteId);

            if (note == null)
            {
                return false;
            }

            if (String.IsNullOrWhiteSpace(value.Text) || String.IsNullOrEmpty(value.Text))
            {
                note.Text = null;
            }
            else
            {
                note.Text = value.Text;
            }

            dbContext.SaveChanges();
            return true;
        }

        public void AddNote(Note note, int repairId)
        {
            if (!(String.IsNullOrWhiteSpace(note.Text) || String.IsNullOrEmpty(note.Text)))
            {
                Note newNote = new Note() 
                {
                    Text = note.Text,
                    RepairID = repairId
                };

                this.Add(newNote);
                dbContext.SaveChanges();
            }
        }
    }
}
