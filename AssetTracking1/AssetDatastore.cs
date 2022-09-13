using AssetTracking1.Models;


namespace AssetTracking1
{
    internal class AssetDatastore
    {
        public static List<Asset> Assets = new();
        public static void LoadTestData()
        {
            Assets.Clear();
            Assets.Add(new Mobile(new Price(45), new DateOnly(2022,3,4), "Model abcs", Brand.Nokia));
            Assets.Add(new Mobile(new Price(455), new DateOnly(2020, 4, 4), "Model 13", Brand.Apple));
            Assets.Add(new Mobile(new Price(51), new DateOnly(2010, 10, 4), "Model 3", Brand.Apple));
            Assets.Add(new Mobile(new Price(5777), new DateOnly(2019, 11, 12), "Model 45", Brand.Samsung));

            Assets.Add(new Laptop(new Price(11455), new DateOnly(2022, 3, 23), "Model asdsedrgerg", Brand.HP));
            Assets.Add(new Laptop(new Price(14455), new DateOnly(2022, 5, 14), "Modello 5", Brand.HP));
            Assets.Add(new Laptop(new Price(12761), new DateOnly(2019, 9, 14), "Modello 5", Brand.Lenovo));
            Assets.Add(new Laptop(new Price(31271), new DateOnly(2019, 9, 12), "Modello 5 EOL", Brand.Lenovo));

            Assets.Add(new Desktop(new Price(12355), new DateOnly(2022, 4, 30), "Modello 66", Brand.Lenovo));
            Assets.Add(new Desktop(new Price(15999), new DateOnly(2022, 5, 2), "Topping", Brand.Asus));
            Assets.Add(new Desktop(new Price(23989), new DateOnly(2019, 10, 5), "ToppingChoklad", Brand.Asus));
        }

    }
}
