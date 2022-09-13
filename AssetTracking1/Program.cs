using AssetTracking1.Models;
using AssetTracking1;


    Console.WriteLine("Hello, World!");
    TestData();
    TestPrint();





    void TestData()
    {
        AssetDatastore.LoadTestData();
    }

    void TestPrint()
    {
    int padding = 20;
    
    Console.WriteLine("Type".PadRight(padding) + "PurchaseDate".PadRight(padding) + "DueDate".PadRight(padding) + "  Active".PadRight(padding) + "Daysleft".PadRight(padding));
    foreach (var asset in AssetDatastore.Assets)
        {
            Console.WriteLine($"{asset.GetAssetType().PadRight(padding)} {asset.PurchaseDate.ToString().PadRight(padding)} {asset.EndOfLifeDate.ToString().PadRight(padding)} {(!asset.IsEndOfLife()).ToString().PadRight(padding)} {asset.GetDaysRemainingToEndOfLife().ToString()}");          
        }
}



