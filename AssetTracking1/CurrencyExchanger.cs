//https://www.xe.com/currencyconverter/convert/?Amount=1&From=USD&To=SEK

namespace AssetTracking1
{
    internal abstract class CurrencyExchanger
    {
        //caching exchangerate to avoid external calls
        private static Dictionary<string,decimal> rates = new();

        public static string Exchange(string from,string to,decimal amount)
        {            
            if(rates.TryGetValue(to, out decimal rate)) return (Math.Round(rate * amount,2)).ToString(); //check if value is in cache

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://currency-exchange.p.rapidapi.com/exchange?from={from}&to={to}&q=1.0"),
                Headers =
                {
                { "X-RapidAPI-Key", "bc52d0924bmsh1df249b4df0e23ap1eb383jsn2ca83d320353" },
                { "X-RapidAPI-Host", "currency-exchange.p.rapidapi.com" },
                },
            };

            String? ret = null;
            using (var response = client.Send(request))   //or SendAsync
            {
                response.EnsureSuccessStatusCode();
                var body =  response.Content.ReadAsStringAsync();
                ret = body.Result.Replace(".",",");
            }
            if (ret is null) return @"N/A";
            
            if (Decimal.TryParse(ret, out rate) && rate > 0.0M)
            {
                rates.Add(to, rate); //add to cache
                return Math.Round(rate * amount, 2).ToString();

            }
                
            return @"N/A";

        }

    } //class CurrencyExchanger

}


