using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace code_learning_spectacles_cli
{
    internal class CodingLanguages
    {
        (HttpResponseMessage, string) responseTuple;
        readonly HttpClient client;
        // Constructor
        public CodingLanguages(HttpClient client)
        {
            this.client = client;
        }

        public (HttpResponseMessage, string) GetResponse()
        {
            return this.responseTuple;
        }

        // GET ALL coding languages
        public async void HitCodingLanguages(string languageId = "")
        {
            HttpResponseMessage response;
            if (languageId == "")
            {
                Console.WriteLine("Getting all Codinglanguages...");
                response = this.client.GetAsync("Codinglanguages").Result;
            }
            else
            {
                Console.WriteLine($"Getting language by languageId {languageId}...");
                response = client.GetAsync($"Codinglanguages/{languageId}").Result;
            }
            string responseStr = await response.Content.ReadAsStringAsync();
            this.responseTuple = (response, responseStr);
        }
    }
}
