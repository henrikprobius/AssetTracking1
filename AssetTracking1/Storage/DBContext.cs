﻿using AssetTracking1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace AssetTracking1.Storage
{
    public class DatabaseContext : DbContext
    {

        private string connstring = @"Server=localhost\sqlexpress;Database=AssetTracker;Trusted_Connection=True;MultipleActiveResultSets=true";
        public DatabaseContext()
        {
            Console.WriteLine("Constructor in class DatabaseContext");            
        }

        public DbSet<Asset>? Assets { get; set; }
        public DbSet<Office>? Offices { get; set; }


        public void DropAndCreateDB()
        {
            Console.WriteLine("DropAndCreateDB in class DatabaseContext");
            bool deleted = Database.EnsureDeleted();      
            bool created = Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder opt)
        {
            Console.WriteLine("OnConfiguring in class DatabaseContext");
            //adding logging and filtering out what Category to log and what loglevel, detta nedan ger bara SQL frågan
            //parameter värden är ersatta med ett ? default, använd EnableSensitiveDataLogging för att exponera
            //fiikns något som heter AsSplitQuery som bryter ner en fråga i delfrågor, värt att kolla
            opt.UseSqlServer(connstring)
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information);
            //opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

          protected override void OnModelCreating(ModelBuilder modelBuilder)
          {
            Console.WriteLine("OnModelCreating in class DatabaseContext");
            modelBuilder.Entity<Computer>().HasBaseType<Asset>();
            modelBuilder.Entity<Laptop>().HasBaseType<Computer>();
            modelBuilder.Entity<Desktop>().HasBaseType<Computer>();
            modelBuilder.Entity<Mobile>().HasBaseType<Asset>();


            modelBuilder.Entity<Asset>()
            .Property("Discriminator")
            .HasMaxLength(200);

            modelBuilder.Entity<Asset>().Property(m => m.BrandName).HasColumnType("int");
            modelBuilder.Entity<Asset>().Property(m => m.Model).HasColumnType("nvarchar(50)");

            modelBuilder.Entity<Office>().Property(m => m.Name).HasColumnType("nvarchar(50)");
            modelBuilder.Entity<Office>().Property(m => m.Currency).HasColumnType("char(3)");


            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Office>().HasData(new Office(1,"Sweden", "SEK"), new Office(2, "Norway", "NOK"), new Office(3, "Japan", "JPY"));
        }
        

    }
}
