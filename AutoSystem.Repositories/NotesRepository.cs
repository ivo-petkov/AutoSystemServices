using AutoSystem.DataLayer;
using AutoSystem.Models;
using System;
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

        public Note GetById(int noteId)
        {
            return dbContext.Notes.FirstOrDefault(u => u.NoteId == noteId);
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
    }
}
