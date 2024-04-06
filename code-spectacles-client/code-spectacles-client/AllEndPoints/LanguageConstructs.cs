using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace code_spectacles_client.AllEndPoints
{
    internal class LanguageConstructs
    {
        (HttpResponseMessage, string) responseTuple;
        readonly HttpClient client;
        // Constructor
        public LanguageConstructs(HttpClient client)
        {
            this.client = client;
        }

        public (HttpResponseMessage, string) GetResponse()
        {
            // add check here for if tuple is empty
            return responseTuple;
        }

        public async void HitLanguageConstructs(string langConstructId = "")
        {
            HttpResponseMessage response;
            if (langConstructId == "")
            {
                Console.WriteLine($"Getting all language construct types...");
                response = this.client.GetAsync("Languageconstructs").Result;
            }
            else
            {
                Console.WriteLine($"Getting language construct by langConstructId {langConstructId}...");
                response = client.GetAsync($"Languageconstructs/{langConstructId}").Result;
            }
            string responseStr = await response.Content.ReadAsStringAsync();
            this.responseTuple = (response, responseStr);
        }
    }
}
