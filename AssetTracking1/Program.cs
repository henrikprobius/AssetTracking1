using AssetTracking1.Models;
using AssetTracking1;
using System.Text;
using AssetTracking1.Storage;

// "1" is office in Sweden with currency SEK
// "2" is office in Norway with currency NOK
// "3" is office in Japan with currency JPY

AssetDatastore store = new AssetDatastore();
//Test();
//PrintAssets();




var thekey = ConsoleKey.Q;
do
{
    //Console.Clear();
    Console.WriteLine("Press F2 to add Assets" + "\n" + "Press F3 to print all assets" +  "\n" + "Press q to quit");
    thekey = Console.ReadKey().Key;
    if (thekey == ConsoleKey.F2) { EnterAssets(); }
    else if (thekey == ConsoleKey.F3) { PrintAssets(); }

} while (thekey != ConsoleKey.Q);

store = null;

/************************** Methods **************************/
void EnterAssets()
{
    bool doRun = true;
    (bool success, string msg) msg;
    String? indata;
    String[]? strings;

    Console.Clear();
    Console.WriteLine("Valid types are: Laptop, Mobile, Desktop");
    Console.WriteLine();
    Console.WriteLine("1 means office in Sweden, 2 means office in Norway, 3 means office in Japan");
    Console.WriteLine();
    Console.WriteLine("Valid brands are: " + StringExtensions.ConvertBrandToString(","));
    Console.WriteLine();
    Console.WriteLine("Format: type,model,purchasedate,purchaseprice,brand,office");
    Console.WriteLine("Enter data, separate with comma like this: Laptop,modelxyz,2023-12-30, 345.56, Nokia, 1");
    Console.WriteLine();
    Console.WriteLine("Enter q to quit");
    do
    {
        indata = Console.ReadLine();
        if (indata is null) continue;
        if ((indata.Trim().ToLower() == "q")) doRun = false;
        else
        {
            try
            {
                strings = StringExtensions.Split(indata, ",",6);
                msg = store.AddAsset(strings);
                Console.WriteLine(msg.msg + "\n");
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

        }
    } while (doRun);

}



void PrintAssets()
    {
    Console.Clear();
    int padding = 15;
    if(store.Assets().Count < 1)
    {
        Console.WriteLine("There are no Assets yet, press any key to continue");
        var mykey = Console.ReadKey().Key;
        return;
    }
    var t = store.Assets().OrderBy(c => c.GetAssetType()).ThenBy(c => c.PurchaseDate);

    Console.WriteLine("Type".PadRight(padding) +" Brand".PadRight(padding) +"  Model".PadRight(padding) +"   Office".PadRight(padding) + "    PurchaseDate".PadRight(padding) + "      DueDate".PadRight(padding) + "      Active".PadRight(padding) + "        Daysleft".PadRight(padding) + "         Price $".PadRight(padding)+ "          LocalPrice".PadRight(padding));
    foreach (var asset in t)
    {
        Console.ForegroundColor = ConsoleColor.White;
        if (asset.GetDaysRemainingToEndOfLife() < 92 && !asset.IsEndOfLife()) Console.ForegroundColor = ConsoleColor.Red;
        else if (asset.GetDaysRemainingToEndOfLife() < 183 && !asset.IsEndOfLife()) Console.ForegroundColor = ConsoleColor.Yellow;
        var localprice = CurrencyExchanger.Exchange(asset.Price.Currency, asset.Office.Currency,asset.Price.PurchasePrice);
        Console.WriteLine($"{asset.GetAssetType().PadRight(padding)} {asset.BrandName.ToString().PadRight(padding)} {asset.Model.PadRight(padding)} {asset.Office.Name.PadRight(padding)} {asset.PurchaseDate.ToString().PadRight(padding)} " +
            $"  {asset.EndOfLifeDate.ToString().PadRight(padding)} {(!asset.IsEndOfLife()).ToString().PadRight(padding)} " +
            $"  {asset.GetDaysRemainingToEndOfLife().ToString().PadRight(padding)} {asset.Price.PurchasePrice.ToString().PadRight(padding)} {(localprice +" "+ asset.Office.Currency).PadRight(padding)}");
    }

    Console.WriteLine("Press any key to continue");
    var key = Console.ReadKey().Key;
}

void Test()
{
   

}

public static class StringExtensions
{
    public static string FirstCharToUpper(this string input) =>
    input switch
    {
        null => throw new ArgumentNullException("Data cannot be empty"),
        "" => throw new ArgumentException($"Data entered in wrong format, try again", nameof(input)),
        _ => string.Concat(input.Trim()[0].ToString().ToUpper(), input.Trim().ToLower().AsSpan(1))
    };

    public static String[] Split(String value, String splitter, int expectedNumberOfEntries)
    {
        var list = value.Split(splitter, StringSplitOptions.TrimEntries);
        if (list.Length != expectedNumberOfEntries) throw new ArgumentNullException("Wrong format entered, try again");
        return list;
    }

    public static string ConvertBrandToString(String separator)
    {
        StringBuilder ret = new();
        foreach (var brand in Enum.GetValues(typeof(Brand)).Cast<Brand>())
        {
            ret.Append(brand).Append(separator);  
        }
        ret.Remove(ret.Length - 1, 1); //remove last separator
        return ret.ToString();
    }
} //class StringExtensions



