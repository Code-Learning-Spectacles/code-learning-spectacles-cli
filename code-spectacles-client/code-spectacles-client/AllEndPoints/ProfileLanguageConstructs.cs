using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace code_spectacles_client.AllEndPoints
{
    internal class ProfileLanguageConstructs
    {
        (HttpResponseMessage, string) responseTuple;
        readonly HttpClient client;
        // Constructor
        public ProfileLanguageConstructs(HttpClient client)
        {
            this.client = client;
        }

        public (HttpResponseMessage, string) GetResponse()
        {
            // add check here for if tuple is empty
            return responseTuple;
        }

        public async void HitProfileLanguageConstructs(string profLangConstructId = "")
        {
            HttpResponseMessage response;
            if (profLangConstructId == "")
            {
                Console.WriteLine($"Getting all profiles' language constructs...");
                response = this.client.GetAsync("Profilelanguageconstructs").Result;
            }
            else
            {
                Console.WriteLine($"Getting language constructs for profLangConstructId {profLangConstructId}...");
                response = client.GetAsync($"Profilelanguageconstructs/{profLangConstructId}").Result;
            }
            string responseStr = await response.Content.ReadAsStringAsync();
            this.responseTuple = (response, responseStr);
        }
    }
}
