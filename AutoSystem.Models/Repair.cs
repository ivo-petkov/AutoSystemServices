using System;
using System.Collections.Generic;

namespace AutoSystem.Models
{
    public class Repair
    {
        private ICollection<Note> notes;

        private ICollection<Attachment> attachments;

        private ICollection<Parts> parts;

        public Repair()
        {
            this.notes = new HashSet<Note>();
            this.attachments = new HashSet<Attachment>();
            this.parts = new HashSet<Parts>();
        }

        public int RepairId { get; set; }

        public RepairStatus Status { get; set; }

        public DateTime Date { get; set; }

        public int Milage { get; set; }

        public decimal FianlePrice { get; set; }

        public decimal PerformerPrice { get; set; }

        public bool IsEditable { get; set; }

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

        public virtual ICollection<Parts> Parts 
        { 
            get {return this.parts;}
            set { this.parts = value; }
        }
    }
}
