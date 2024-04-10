using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace code_learning_spectacles_cli.AllEndPoints
{
    internal class Profiles
    {
        (HttpResponseMessage, string) responseTuple;
        readonly HttpClient client;
        // Constructor
        public Profiles(HttpClient client)
        {
            this.client = client;
        }

        public (HttpResponseMessage, string) GetResponse()
        {
            return this.responseTuple;
        }

        // GET ALL code constructs
        public async void HitProfiles(string profileId = "")
        {
            HttpResponseMessage response;
            if (profileId == "")
            {
                Console.WriteLine("Getting all profiles...");
                response = this.client.GetAsync("Profiles").Result;
            }
            else
            {
                Console.WriteLine($"Getting profile by profileId {profileId}...");
                response = client.GetAsync($"Profiles/{profileId}").Result;
            }
            string responseStr = await response.Content.ReadAsStringAsync();
            this.responseTuple = (response, responseStr);
        }
    }
}
