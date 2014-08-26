using AutoSystem.DataLayer;
using AutoSystem.DataLayer.Migrations;
using AutoSystem.Models;
using System;
using System.Data.Entity;

namespace AutoSystem.Client
{
    internal class Program
    {
        internal static void Main()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AutoSystemContext, Configuration>());

            var db = new AutoSystemContext();

            //Doesn't create database without initial data
            var performer = new Performer { Username = "Ceco", AuthCode = "123", Name = "ServizCeco" };
            db.Performers.Add(performer);


            db.SaveChanges();

            Console.WriteLine("Ready!");
            Console.ReadLine();
        }
    }
}
