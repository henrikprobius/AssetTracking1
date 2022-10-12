using AssetTracking1.Models;
using Microsoft.EntityFrameworkCore;

namespace AssetTracking1.Storage
{
    internal class AssetDatastore:IDisposable
    {
        public DatabaseContext context = new DatabaseContext(); 

        public AssetDatastore()
        {
            context.DropAndCreateDB();
            this.AddTestAssets();

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

        private void AddTestAssets()
        {

            Console.WriteLine("*************** Testcases ******************");

            String[] samples =
            {
                 //success
                "Laptop,modelxyz,2023-12-30, 345.56, Nokia,3",
                "Desktop,modelxyz,2023-12-30, 707, Nokia,2",
                "Mobile,modelxyz,2021-12-30, 98, samsung,3",
                "mobile,yellow,2020-01-26, 551, HP,2",  //yellow
                "desktop,red,2019-11-01, 942, Apple,1",  //red
                "Laptop,NiceDate,12/3 2020, 347.23, Nokia,2"
            };

            String[] split;
            (bool success, string msg) msg = (false, "");

            foreach (String sample in samples)
            {
                try
                {
                    split = StringExtensions.Split(sample, ",", 6);
                    msg = AddAsset(split);
                }
                catch (Exception e)
                {
                    Console.WriteLine(sample + "    Generated error: " + e.Message);
                }

                Console.WriteLine(sample + "    Generated output: " + msg.success + " " + msg.msg);
            }
            /*             
                         //failure
            "jmobile,modelxyz,2021-12-30, 551, HP,2",
            "mobile,modelxyz,2021-12-30, 551,3",
            "",
            "mobile,modelxyz,2021-12-30, sss, HP,1",
            "laptop,modelxyz,2021-02-30, 44.7, HP,3",
            "Laptop,modejjz,2021-01-30, 44.7, NoBrand,1",
            "Laptop,NiceDate,12/3 2020, 347.23, Nokia,4"             
             */
        }

        public void Dispose()
        {
            if (context is not null) context.Dispose();
            context = null;
        }
    }
}
