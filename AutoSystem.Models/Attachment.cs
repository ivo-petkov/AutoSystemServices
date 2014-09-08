using System;

namespace AutoSystem.Models
{
    public class Attachment
    {
        public int AttachmentId { get; set; }

        public string Name { get; set; }

        public AttachmentDocumentType DocumentType { get; set; }

        public AttachmentFileFormat FileFormat { get; set; }

        public byte[] Data { get; set; }

        public int RepairId { get; set; }
        public virtual Repair Repair { get; set; }
    }
}
