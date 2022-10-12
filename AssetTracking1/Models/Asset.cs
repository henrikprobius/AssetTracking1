
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetTracking1.Models
{
    public class Asset
    {
        public static readonly int AssetLifespanInMonths = 36;

        public Guid AssetId { get; set; } = Guid.Empty; 

        [Required]
        public String Model { get; set; } = String.Empty;
        public Brand BrandName { get; set; } = Brand.Unknown;


        public DateTime PurchaseDate { get; set; } =DateTime.Now;

        public Office Office { get; set; }


        public Price PurchasePrice { get; } = new Price();


        public DateTime EndOfLifeDate { get {
                return PurchaseDate.AddMonths(AssetLifespanInMonths); } }

        protected Asset() { }

        protected Asset(Office office, Price price, DateTime purchaseDate, string model, Brand brandName = Brand.Unknown)
        {
            Model = model;
            BrandName = brandName;
            PurchasePrice = price;
            PurchaseDate = purchaseDate;
            Office = office;
        }


        public int GetDaysRemainingToEndOfLife() =>
            IsEndOfLife() ? 0 : (int) (EndOfLifeDate - DateTime.Now).TotalDays;


        public bool IsEndOfLife() =>
            (EndOfLifeDate <= DateTime.Now);


        public virtual string GetAssetType()
        {
            return this.GetType().Name;
        }

    }//Asset class


    public class Laptop : Computer
    {
        public Laptop() { }

        public Laptop(Office office, Price price, DateTime purchaseDate, string model, Brand brandName = Brand.Unknown) : base(office, price, purchaseDate, model, brandName)
        { }

        public override string GetAssetType()
        {
            return this.GetType().Name;
        }
    }// Laptop class


    public class Desktop : Computer
    {
        public Desktop() { }
        public Desktop(Office office, Price price, DateTime purchaseDate, string model, Brand brandName = Brand.Unknown) : base(office, price, purchaseDate, model, brandName)
        { }

        public override string GetAssetType()
        {
            return this.GetType().Name;
        }
    }// Desktop class

    public class Mobile : Asset
    {
        public Mobile() { }
        public Mobile(Office office, Price price, DateTime purchaseDate, string model, Brand brandName = Brand.Unknown) : base(office, price, purchaseDate, model, brandName)
        { }

        public override string GetAssetType()
        {
            return this.GetType().Name;
        }
    }// Mobile class


    public abstract class Computer : Asset
    {
        public Computer() { }
        public Computer(Office office, Price price, DateTime purchaseDate, string model, Brand brandName = Brand.Unknown) : base(office, price, purchaseDate, model, brandName)
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


        [Required]
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
