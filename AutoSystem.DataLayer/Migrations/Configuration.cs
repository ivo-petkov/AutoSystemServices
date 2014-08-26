using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using AutoSystem.Models;

namespace AutoSystem.DataLayer.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<AutoSystem.DataLayer.AutoSystemContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(AutoSystem.DataLayer.AutoSystemContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            context.Performers.AddOrUpdate(new Performer
            {
                Username = "Admin",
                AuthCode = "123123",
                Name = "Shefa",
                Telephone = "0883123123",
                Address = "Ivan Vazov 23"
            });
        }
    }
}
