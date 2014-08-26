using AutoSystem.DataLayer;
using AutoSystem.Models;
using System;

namespace AutoSystem.Repositories
{
    public class NotesReposiotory  : EfRepository<Note>
    {
        private AutoSystemContext dbContext;

        public NotesReposiotory(AutoSystemContext context)
            : base(context)
        {
            this.dbContext = context;
        }
    }
}
