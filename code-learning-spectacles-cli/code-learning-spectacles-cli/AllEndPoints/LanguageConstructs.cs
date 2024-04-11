using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace code_learning_spectacles_cli.AllEndPoints
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
            return this.responseTuple;
        }

        public async void HitLanguageConstructs(string langConstructId = "")
        {
            HttpResponseMessage response;
            if (langConstructId == "")
            {
                response = this.client.GetAsync("Languageconstructs").Result;
            }
            else
            {
                response = client.GetAsync($"Languageconstructs/{langConstructId}").Result;
            }
            string responseStr = await response.Content.ReadAsStringAsync();
            this.responseTuple = (response, responseStr);
        }

        public async void HitLanguageConstructsByLangIdByConstructId(string langId, string constructId)
        {
            HttpResponseMessage response;
            response = client.GetAsync($"Languageconstructs/getByLanguage/getByConstructId/{langId}/{constructId}").Result;
            string responseStr = await response.Content.ReadAsStringAsync();
            this.responseTuple = (response, responseStr);
        }
    }
}
