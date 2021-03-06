﻿using AutoSystem.Models;
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
        public AttachmentDocumentType DocumentType { get; set; }
        public AttachmentFileFormat FileFormat { get; set; }
        public byte[] Data { get; set; }
        public int RepairId { get; set; }
    }
}