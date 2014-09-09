using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoSystem.Services.Models
{
    public class NoteModel
    {
        public int NoteId { get; set; }
        public string Text { get; set; }
        public int RepairId { get; set; }
    }
}