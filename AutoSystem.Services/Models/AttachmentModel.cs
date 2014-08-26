using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoSystem.Services.Models
{
    public class AttachmentModel
    {
        public int AttachmentId { get; set; }
        public string Name { get; set; }
        public AttachmentTypeModel Type { get; set; }
        public byte[] Data { get; set; }
    }
}