using AutoSystem.DataLayer;
using AutoSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoSystem.Repositories
{
    public class AttachmentsRepository : EfRepository<Attachment>
    {
        private AutoSystemContext dbContext;
        private RepairsRepository repairsRepository;

        public AttachmentsRepository(AutoSystemContext context)
            : base(context)
        {
            this.dbContext = context;   
            this.repairsRepository = new RepairsRepository(context);
        }

        public void EditRepairAttachment(ICollection<Attachment> editedAttachments, int repairId)
        {
           List<Attachment> atts = new List<Attachment>(this.repairsRepository.Get(repairId).Attachments);
           List<int> editedAttsIds = new List<int>();     

            foreach (var attachment in editedAttachments)
            {
                editedAttsIds.Add(attachment.AttachmentId);

                var check = dbContext.Attachments.Find(attachment.AttachmentId);
                if (check == null)
                {
                    AddAttachment(attachment, repairId);
                }
            }

            foreach (var att in atts)
            {
                if (!editedAttsIds.Contains(att.AttachmentId))
                {
                    this.Delete(att.AttachmentId);
                }
            }
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
