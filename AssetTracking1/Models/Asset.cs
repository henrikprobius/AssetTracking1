
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

        public Office? Office { get; set; }
        public Price? PurchasePrice { get; set; }

        public DateOnly EndOfLifeDate { get {
                return PurchaseDate.AddMonths(AssetLifespanInMonths);} }

        protected Asset(Price price, DateOnly purchaseDate, string model, Brand brandName = Brand.Unknown)
        {
            Model = model;
            BrandName = brandName;   
            PurchasePrice = price;
            PurchaseDate = purchaseDate;    
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
        public Laptop(Price price, DateOnly purchaseDate, string model, Brand brandName = Brand.Unknown) : base(price, purchaseDate, model, brandName)
        { }

        public override string GetAssetType()
        {
            return this.GetType().Name;
        }
    }// Laptop class


    public class Desktop : Computer
    {
        public Desktop(Price price, DateOnly purchaseDate, string model, Brand brandName = Brand.Unknown) : base(price, purchaseDate, model, brandName)
        { }

        public override string GetAssetType()
        {
            return this.GetType().Name;
        }
    }// Desktop class

    public class Mobile : Asset
    {
        public Mobile(Price price, DateOnly purchaseDate, string model, Brand brandName = Brand.Unknown) : base(price, purchaseDate, model, brandName)
        { }

        public override string GetAssetType()
        {
            return this.GetType().Name;
        }
    }// Mobile class


    public abstract class Computer : Asset
    {
        public Computer(Price price, DateOnly purchaseDate, string model, Brand brandName = Brand.Unknown) : base(price, purchaseDate, model, brandName)
        { }

        public override string GetAssetType()
        {
            return this.GetType().Name;
        }

    }//class Computer


    public class Office
    {
        String  Name { get; set; } = String.Empty;

        public Office(String name)
        {
            this.Name = name;
        }

    }//class Office


    public class Price
    {
        public decimal PurchasePrice { get; set; } = 0.0M;
        public String? Currency { get; set; }

        public Price(decimal price = 0.0M, string currency = "")
        {
            this.PurchasePrice = price;
            this.Currency = currency;   
        }

    }//class Price





    [System.Flags]
    public enum Brand : uint  //32 Brands are the maximum
    { 
        Apple       = 0,  
        Asus        = 2,
        HP          = 4, 
        Huawei      = 8,    
        Lenovo      = 16,
        Motorola    = 32,
        Nokia       = 64,
        Samsung     = 128,
        Unknown     = 256
    }// enum Brand


}//namespace
