using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Magal
{
    class APIHelper
    {
        
        private static readonly string CUOUNTRY_URL = "https://free.currencyconverterapi.com/api/v5/countries";
        private static readonly string CONVERT_REQUEST = "https://free.currencyconverterapi.com/api/v5/convert?compact=y&q=";
        private static List<Country> countries;

        public static async Task<List<Country>> GetCountriesAsync()
        {
            if (countries == null)
                await LoadCountriesAsync();
            return countries;
        }

        private static async Task LoadCountriesAsync()
        {
            countries = new List<Country>();
            JToken token = JsonConvert.DeserializeObject<JToken>(await ExecuteRequestAsync(CUOUNTRY_URL));
            countries.AddRange(token["results"].Select(t => t.First.ToObject<Country>()).OrderBy(c => c.currencyName));
        }

        public static async Task<double> getConvertUnit(string source, string dest)
        {
            string result = await ExecuteRequestAsync(CONVERT_REQUEST + source + "_" + dest);
            string unit = JsonConvert.DeserializeObject<JToken>(result).First().First()["val"].ToString();
            return Convert.ToDouble(unit);
        }

        private static async Task<string> ExecuteRequestAsync(string url)
        {
            HttpWebRequest request = WebRequest.CreateHttp(url);
            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
            StreamReader streamReader = new StreamReader(response.GetResponseStream());
            return streamReader.ReadToEnd();
        }

        internal static string getSignForID(string v)
        {
            return countries.Where(c => c.currencyId.Equals(v)).Select(c => c.currencySymbol).First();
        }
    }
}
