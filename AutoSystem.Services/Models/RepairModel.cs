using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoSystem.Services.Models
{
    public class RepairModel
    {
        public int RepairId { get; set; }
        public RepairStatusModel Status { get; set; }
        public DateTime Date { get; set; }
        public int Milage { get; set; }
        public decimal Price { get; set; }
        public IEnumerable<NoteModel> Notes { get; set; }
        public IEnumerable<AttachmentModel> Attachments { get; set; }
    }
}