using System;
using System.Collections.Generic;

namespace AutoSystem.Models
{
    public class Repair
    {
        private ICollection<Note> notes;

        private ICollection<Attachment> attachments;

        public Repair()
        {
            this.notes = new HashSet<Note>();
            this.attachments = new HashSet<Attachment>();
        }

        public int RepairId { get; set; }

        public RepairStatus Status { get; set; }

        public DateTime Date { get; set; }

        public int Milage { get; set; }

        public decimal Price { get; set; }

        public int CarId { get; set; }
        public virtual Car Car { get; set; }

        public int PerformerId { get; set; }
        public virtual Performer Performer { get; set; }       

        public virtual ICollection<Note> Notes
        {
            get { return this.notes; }
            set { this.notes = value; }
        }        

        public virtual ICollection<Attachment> Attachments
        {
            get { return this.attachments; }
            set { this.attachments = value; }
        }        
    }
}
