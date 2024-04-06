using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace code_spectacles_client
{
    internal class ConstructTypes
    {
        (HttpResponseMessage, string) responseTuple;
        readonly HttpClient client;
        // Constructor
        public ConstructTypes(HttpClient client)
        {
            this.client = client;
        }

        public (HttpResponseMessage, string) GetResponse()
        {
            // add check here for if tuple is empty
            return responseTuple;
        }

        public async void HitConstructTypes(string constructTypeId = "")
        {
            HttpResponseMessage response;
            if (constructTypeId == "")
            {
                Console.WriteLine($"Getting all construct types...");
                response = this.client.GetAsync("Constructtypes").Result;
            }
            else
            {
                Console.WriteLine($"Getting construct by constructTypeId {constructTypeId}...");
                response = client.GetAsync($"Constructtypes/{constructTypeId}").Result;
            }
            string responseStr = await response.Content.ReadAsStringAsync();
            this.responseTuple = (response, responseStr);
        }
    }
}
