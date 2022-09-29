
using System.ComponentModel.DataAnnotations;

namespace AssetTracking1.Models
{
    public abstract class Asset
    {
        public static readonly int AssetLifespanInMonths = 36;
        
        [Required]
        public String Model { get; set; } = String.Empty;
        public Brand BrandName { get; set; } = Brand.Unknown;


        public DateOnly PurchaseDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        public Office Office { get; set; }
        
     
        public Price PurchasePrice { get;} = new Price();
        
        
        public DateOnly EndOfLifeDate { get {
                return PurchaseDate.AddMonths(AssetLifespanInMonths);} }

        protected Asset() { }

        protected Asset(Office office, Price price, DateOnly purchaseDate, string model, Brand brandName = Brand.Unknown)
        {
            Model = model;
            BrandName = brandName;   
            PurchasePrice = price;
            PurchaseDate = purchaseDate; 
            Office = office;    
        }


        public int GetDaysRemainingToEndOfLife() =>
            IsEndOfLife() ? 0 : EndOfLifeDate.DayNumber - DateOnly.FromDateTime(DateTime.Now).DayNumber;
                     

        public bool IsEndOfLife() => 
            (EndOfLifeDate <= DateOnly.FromDateTime(DateTime.Now));

  
        public virtual string GetAssetType()
        {
            return this.GetType().Name;
        }

    }//Asset class


    public class Laptop : Computer
    {
        public Laptop() { }

        public Laptop(Office office,Price price, DateOnly purchaseDate, string model, Brand brandName = Brand.Unknown) : base(office, price, purchaseDate, model, brandName)
        { }

        public override string GetAssetType()
        {
            return this.GetType().Name;
        }
    }// Laptop class


    public class Desktop : Computer
    {
        public Desktop() { }
        public Desktop(Office office,Price price, DateOnly purchaseDate, string model, Brand brandName = Brand.Unknown) : base(office, price, purchaseDate, model, brandName)
        { }

        public override string GetAssetType()
        {
            return this.GetType().Name;
        }
    }// Desktop class

    public class Mobile : Asset
    {
        public Mobile() { }
        public Mobile(Office office,Price price, DateOnly purchaseDate, string model, Brand brandName = Brand.Unknown) : base(office, price, purchaseDate, model, brandName)
        { }

        public override string GetAssetType()
        {
            return this.GetType().Name;
        }
    }// Mobile class


    public abstract class Computer : Asset
    {
        public Computer() { }
        public Computer(Office office,Price price, DateOnly purchaseDate, string model, Brand brandName = Brand.Unknown) : base(office, price, purchaseDate, model, brandName)
        { }

        public override string GetAssetType()
        {
            return this.GetType().Name;
        }

    }//class Computer


    public class Office
    {
        public Office() { }

        [Required]
        public String  Name { get; set; } = String.Empty;

        [Required]
        public String Currency { get; set; } = "USD";

        public Office(String name, string currency)
        {
            this.Name = name;
            Currency = currency;    
        }

    }//class Office


    public class Price: IComparable<Price>
    {
        [Required]
        public decimal PurchasePrice { get; set; } = 0.0M;

        [Required]
        public String Currency { get; set; } = "USD";

        public int CompareTo(Price price)
        {
            return this.PurchasePrice.CompareTo(price.PurchasePrice);
        }
        public Price(decimal price = 0.0M)
        {
            this.PurchasePrice = price;
          
        }

    }//class Price


    [System.Flags]
    public enum Brand : uint  //32 Brands are the maximum
    { 
        Apple       = 0,  
        Asus        = 2,
        Hp          = 4, 
        Huawei      = 8,    
        Lenovo      = 16,
        Motorola    = 32,
        Nokia       = 64,
        Samsung     = 128,
        Unknown     = 256
    }// enum Brand


}//namespace
