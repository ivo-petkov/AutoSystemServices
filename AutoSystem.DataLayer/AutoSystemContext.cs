using System;
using System.Data.Entity;
using System.Linq;
using AutoSystem.Models;

namespace AutoSystem.DataLayer
{
    public class AutoSystemContext : DbContext
    {
        public AutoSystemContext()
            : base("AutoSystemDb")
        {
            
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Performer> Performers { get; set; }
        public DbSet<Repair> Repairs { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Parts> Parts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
           

            base.OnModelCreating(modelBuilder);
        }
    }
}
