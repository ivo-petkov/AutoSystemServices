using AutoSystem.DataLayer;
using AutoSystem.Models;
using System;
using System.Collections.Generic;

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

        public void EditRepairAttachment(ICollection<Attachment> editedAttachments, int repairId)
        {
            foreach (var attachment in editedAttachments)
            {
                if (!EditAttachment(attachment))
                {
                    AddAttachment(attachment, repairId);
                }
            }
        }

        public bool EditAttachment(Attachment value)
        {
            var attachment = dbContext.Attachments.Find(value.AttachmentId);

            if (attachment == null)
            {
                return false;
            }

            attachment.Name = value.Name;
            attachment.Data = value.Data == null ? attachment.Data : value.Data;
            attachment.DocumentType = value.DocumentType;
            attachment.FileFormat = value.FileFormat;            

            dbContext.SaveChanges();
            return true;
        }

        public void AddAttachment(Attachment att, int repairId)
        {

            Attachment newAttachment = new Attachment()
            {
                Name = att.Name,
                Data = att.Data,
                DocumentType = att.DocumentType,
                FileFormat = att.FileFormat,
                RepairId = repairId
            };

            this.Add(newAttachment);
            dbContext.SaveChanges();            
        }
    }
}
