
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetTracking1.Models
{
    public abstract class Asset
    {
        public static readonly int AssetLifespanInMonths = 36;

        public Guid AssetId { get; set; } = Guid.Empty; 


        public String Model { get; set; } = String.Empty;
        public Brand BrandName { get; set; } = Brand.Unknown;

        public DateTime PurchaseDate { get; set; } = DateTime.Now;

        public Office Office { get; set; }

        [NotMapped]
        public Price Price { get; } = new Price();

        public decimal PurchasePrice {
            get
            {
                return Price.PurchasePrice;
            }

            set
            {
                Price.PurchasePrice = value;
            } 
                
        }


        public DateOnly EndOfLifeDate{ 
            get {
                var e = PurchaseDate.AddMonths(AssetLifespanInMonths);
                return new DateOnly(e.Year,e.Month,e.Day);               
            }
        }

        public DateOnly GetPurchaseDate()
        {
            return DateOnly.FromDateTime(PurchaseDate);

        }

        protected Asset() { }

        protected Asset(Office office, decimal price, DateTime purchaseDate, string model, Brand brandName = Brand.Unknown)
        {
            Model = model;
            BrandName = brandName;
            Price.PurchasePrice = price;
            PurchaseDate = purchaseDate;
            Office = office;
        }


        public int GetDaysRemainingToEndOfLife() =>
            IsEndOfLife() ? 0 : (int) (EndOfLifeDate.DayNumber - DateOnly.FromDateTime(DateTime.Today).DayNumber);


        public bool IsEndOfLife() =>
            (EndOfLifeDate <= DateOnly.FromDateTime(DateTime.Today));


        public virtual string GetAssetType()
        {
            return this.GetType().Name;
        }

    }//Asset class


    public class Laptop : Computer
    {
        public Laptop() { }

        public Laptop(Office office, decimal price, DateTime purchaseDate, string model, Brand brandName = Brand.Unknown) : base(office, price, purchaseDate, model, brandName)
        { }

        public override string GetAssetType()
        {
            return this.GetType().Name;
        }
    }// Laptop class


    public class Desktop : Computer
    {
        public Desktop() { }
        public Desktop(Office office, decimal price, DateTime purchaseDate, string model, Brand brandName = Brand.Unknown) : base(office, price, purchaseDate, model, brandName)
        { }

        public override string GetAssetType()
        {
            return this.GetType().Name;
        }
    }// Desktop class

    public class Mobile : Asset
    {
        public Mobile() { }
        public Mobile(Office office, decimal price, DateTime purchaseDate, string model, Brand brandName = Brand.Unknown) : base(office, price, purchaseDate, model, brandName)
        { }

        public override string GetAssetType()
        {
            return this.GetType().Name;
        }
    }// Mobile class


    public abstract class Computer : Asset
    {
        public Computer() { }
        public Computer(Office office, decimal price, DateTime purchaseDate, string model, Brand brandName = Brand.Unknown) : base(office, price, purchaseDate, model, brandName)
        { }

        public override string GetAssetType()
        {
            return this.GetType().Name;
        }

    }//class Computer


    public class Office
    {
        public Office() { }

        public int OfficeId { get; set; }


        public String Name { get; set; } = String.Empty;

        [Required]
        public String Currency { get; set; } = "USD";

        public Office(int id,String name, string currency)
        {
            this.OfficeId = id;
            this.Name = name;
            Currency = currency;
        }

    }//class Office


    public class Price : IComparable<Price>
    {
        [Required]
        public decimal PurchasePrice { get; set; } = 0.0M;

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
        Apple = 0,
        Asus = 2,
        Hp = 4,
        Huawei = 8,
        Lenovo = 16,
        Motorola = 32,
        Nokia = 64,
        Samsung = 128,
        Unknown = 256
    }// enum Brand

   

}//namespace
