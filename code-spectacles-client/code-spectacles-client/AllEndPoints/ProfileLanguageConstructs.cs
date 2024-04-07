using Newtonsoft.Json;
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

        internal class Favourite
        {
            public int profileId { get; set; }
            public int languageconstructId { get; set; }
            public string notes { get; set; }
        }
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

        public async void PostNote(Favourite payload)
        {
            Console.WriteLine("Posting...");
            var stringPayload = JsonConvert.SerializeObject(payload);
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            // Discard response with "_"
            _ = await this.client.PostAsync("Profilelanguageconstructs", httpContent); // Don't know if this is right ?
            Console.WriteLine("Note posted");
        }
    }
}
