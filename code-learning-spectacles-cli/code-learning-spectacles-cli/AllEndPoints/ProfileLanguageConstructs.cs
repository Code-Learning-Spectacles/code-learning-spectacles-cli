using code_learning_spectacles_cli.Models;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace code_learning_spectacles_cli.AllEndPoints
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
            return this.responseTuple;
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

        public async void HitPostNote(Endpoints.Payload payload)
        {
            //HttpResponseMessage response;
            Console.WriteLine("Posting...");
            using StringContent jsonContent = new(JsonSerializer.Serialize(new { Profileid = Int32.Parse(Helpers.ProfileID), Languageconstructid = payload.languageconstructId, Notes = payload.note }), Encoding.UTF8, "application/json");
            using HttpResponseMessage createProfileResponse = await Endpoints.client.PostAsync("Profilelanguageconstructs", jsonContent);
            //string responseStr = await response.Content.ReadAsStringAsync();
            //this.responseTuple = (response, responseStr);
        }
        public async void HitViewNotes()
        {
            HttpResponseMessage response;
            response = client.GetAsync($"Profilelanguageconstructs/getByProfile/{Helpers.ProfileID}").Result;
            string responseStr = await response.Content.ReadAsStringAsync();
            this.responseTuple = (response, responseStr);
        }
    }
}
