﻿using AutoSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoSystem.Services.Models
{
    public class RepairModel
    {
        public int RepairId { get; set; }
        public RepairStatus Status { get; set; }
        public string Date { get; set; }
        public int Milage { get; set; }
        public decimal FinalPrice { get; set; }
        public decimal PerformerPrice { get; set; }
        public AdminStatus AdminStatus { get; set; }
        public int PerformerId { get; set; }
        public int CarId { get; set; }
        public CarModel Car { get; set; }
        public PerformerModel Performer { get; set; }
        public IEnumerable<NoteModel> Notes { get; set; }
        public IEnumerable<AttachmentModel> Attachments { get; set; }
        public IEnumerable<PartsModel> Parts { get; set; }
        //public int ClientId { get; set; }
    }
}