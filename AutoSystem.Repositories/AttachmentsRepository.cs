using AutoSystem.DataLayer;
using AutoSystem.Models;
using System;

namespace AutoSystem.Repositories
{
    public class AttachmentsRepository : EfRepository<Attachment>
    {
        private AutoSystemContext dbContext;

        public AttachmentsRepository(AutoSystemContext context)
            : base(context)
        {
            this.dbContext = context;
        }
    }
}
