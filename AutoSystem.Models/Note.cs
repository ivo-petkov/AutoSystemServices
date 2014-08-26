using System;

namespace AutoSystem.Models
{
    public class Note
    {
        public int NoteId { get; set; }

        public string Text { get; set; }

        public int RepairID { get; set; }
        public virtual Repair Repair { get; set; }
    }
}
