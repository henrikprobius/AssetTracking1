using AssetTracking1.Models;
using Microsoft.EntityFrameworkCore;

namespace AssetTracking1.Storage
{
    public class DatabaseContext : DbContext
    {

        private string connstring = @"Server=localhost\sqlexpress;Database=AssetTracker;Trusted_Connection=True;MultipleActiveResultSets=true";
        public DatabaseContext(){}

        public DbSet<Asset>? Assets { get; set; }
        public DbSet<Office>? Offices { get; set; }


        public void DropAndCreateDB()
        {
            Console.WriteLine("Dropping and recreate DB");
            bool deleted = Database.EnsureDeleted();      
            bool created = Database.EnsureCreated();
        }

        //här man du lägga till egna conventions
        // tex om alla strängar ska vara nvarchar(100) i DB
        //have... för alla, has.. för en typ
        protected override void ConfigureConventions(ModelConfigurationBuilder c)
        {
            base.ConfigureConventions(c);
            c.Properties<string>().HaveColumnType("nvarchar(100)");

        }

        
        
        protected override void OnConfiguring(DbContextOptionsBuilder opt)
        {
            opt.UseSqlServer(connstring)
                .EnableSensitiveDataLogging();
                //.LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information);
            //opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        // heter Owned Entities, OwnsOne heter metoden i modelbuilder på vald typ
        //tex om du bryter ut alla namn på en Person i ett egen relaterad klass istället för enskilda fält
        // obs om det finns Shadow Propertis så måste detta definieras i en migration
        protected override void OnModelCreating(ModelBuilder modelBuilder)
          {            
            modelBuilder.Entity<Computer>().HasBaseType<Asset>();
            modelBuilder.Entity<Laptop>().HasBaseType<Computer>();
            modelBuilder.Entity<Desktop>().HasBaseType<Computer>();
            modelBuilder.Entity<Mobile>().HasBaseType<Asset>();

            // det finns AutoInclude = true, alla frågor tar med relaterade object implicit
            //HasConversion<T>, tex om någon Enum ska lagras som text istället i DB, lamda dct går också, kallas Value Conversions

            modelBuilder.Entity<Asset>()
            .Property("Discriminator")
            .HasMaxLength(200);

            modelBuilder.Entity<Asset>().Property(m => m.BrandName).HasColumnType("int");
            modelBuilder.Entity<Asset>().Property(m => m.Model).HasColumnType("nvarchar(50)");

            modelBuilder.Entity<Office>().Property(m => m.Name).HasColumnType("nvarchar(50)");
            modelBuilder.Entity<Office>().Property(m => m.Currency).HasColumnType("char(3)");

            modelBuilder.Entity<Office>().HasData(new Office(1,"Sweden", "SEK"), new Office(2, "Norway", "NOK"), new Office(3, "Japan", "JPY"));
            modelBuilder.Entity<Asset>().OwnsOne(p => p.TrackInfo);
        }
        

    }
}
