using AutoSystem.Models;
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
        public decimal Price { get; set; }
        public int PerformerId { get; set; }
        public int CarId { get; set; }
        public IEnumerable<Note> Notes { get; set; }
        public IEnumerable<Attachment> Attachments { get; set; }
    }
}