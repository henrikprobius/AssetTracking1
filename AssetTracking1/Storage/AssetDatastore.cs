using AssetTracking1.Models;
using Microsoft.EntityFrameworkCore;

namespace AssetTracking1.Storage
{
    internal class AssetDatastore:IDisposable
    {
        //public List<Asset> Assets = new();
        //  public static Dictionary<string, Office> offices = new();

        public DatabaseContext context = new DatabaseContext(); 

        public AssetDatastore()
        {
            context.DropAndCreateDB();

        }
        public (bool, string) AddAsset(string[] data)
        {
            string[] validOfficeNumbers = { "1", "2", "3" };
            Asset? a = CreateClass(data[0]);
            if (a is null) return (false, $"Wrong typename: {data[0]}");
            var brand = data[4].FirstCharToUpper();
            if (!Enum.IsDefined(typeof(Brand), brand)) return (false, $"Wrong brandname: {data[3]}");

            if (!decimal.TryParse(data[3].Replace('.', ','), out decimal price)) return (false, $"Not a number: {data[3]}");
            if (!DateTime.TryParse(data[2], out DateTime purchasedate)) return (false, $"Not a date: {data[2]}");
            if (!validOfficeNumbers.Contains(data[5])) return (false, $"Incorrect office: {data[5]}");

            a.Model = data[1];
            a.PurchaseDate = purchasedate;
            a.PurchasePrice = price;

            a.BrandName = (Brand)Enum.Parse(typeof(Brand), brand);
            //a.Office = offices[data[5]];
            a.Office = context.Offices.Find(int.Parse(data[5]));

            context.Assets.Add(a);
            context.SaveChanges();
            //Assets.Add(a);

            return (true, "Success adding a new Asset");
        }

        

        private Asset? CreateClass(string typeName)
        {
            typeName = "AssetTracking1.Models." + typeName;
            var type = Type.GetType(typeName, false, true);
            if (type is null) return null;
            return (Asset)Activator.CreateInstance(type);
        }

        public List<Asset> Assets()
        {
            return context.Assets.ToList();
        }

        private void LoadTestData()
        {

            var teststring = "Laptop,modelxyz,2023 - 12 - 30, 345.56, Nokia, 1";
            //Assets.Clear();
            /*

                        Assets.Add(new Mobile(o1, new Price(45), new DateOnly(2022, 3, 4), "Model abcs", Brand.Nokia));
                        Assets.Add(new Mobile(o1, new Price(455), new DateOnly(2020, 4, 4), "Model 13", Brand.Apple));
                        Assets.Add(new Mobile(o3, new Price(51), new DateOnly(2010, 10, 4), "Model 3", Brand.Apple));
                        Assets.Add(new Mobile(o1, new Price(598), new DateOnly(2019, 11, 12), "Model 45", Brand.Samsung));
                        Assets.Add(new Mobile(o1, new Price(468), new DateOnly(2020, 2, 12), "Modelito", Brand.Motorola));

                        Assets.Add(new Laptop(o2, new Price(1148), new DateOnly(2022, 3, 23), "Model sedrgerg", Brand.Hp));
                        Assets.Add(new Laptop(o1, new Price(1455), new DateOnly(2022, 5, 14), "Modello 5", Brand.Hp));
                        Assets.Add(new Laptop(o3, new Price(1271), new DateOnly(2019, 9, 14), "Modello 5", Brand.Lenovo));
                        Assets.Add(new Laptop(o1, new Price(3127), new DateOnly(2019, 9, 12), "Modello 5 EOL", Brand.Lenovo));

                        Assets.Add(new Desktop(o1, new Price(1235), new DateOnly(2022, 4, 30), "Modello 66", Brand.Lenovo));
                        Assets.Add(new Desktop(o2, new Price(5999), new DateOnly(2022, 5, 2), "Topping", Brand.Asus));
                        Assets.Add(new Desktop(o1, new Price(2389), new DateOnly(2019, 10, 5), "ToppingChoklad", Brand.Asus));
                        Assets.Add(new Desktop(o3, new Price(7399), new DateOnly(2020, 1, 26), "ToppingBanan", Brand.Lenovo));
            */
        }

        public void Dispose()
        {
            if (context is not null) context.Dispose();
            context = null;
        }
    }
}
