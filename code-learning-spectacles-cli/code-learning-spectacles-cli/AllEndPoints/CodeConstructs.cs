using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace code_learning_spectacles_cli
{
    internal class CodeConstructs
    {
        (HttpResponseMessage, string) responseTuple;
        readonly HttpClient client;
        // Constructor
        public CodeConstructs(HttpClient client)
        {
            this.client = client;
        }

        public (HttpResponseMessage, string) GetResponse()
        {
            return this.responseTuple;
        }

        // GET ALL code constructs
        public async void  HitCodeConstructs(string constructId = "")
        {
            HttpResponseMessage response;
            if (constructId == "")
            {
                Console.WriteLine("Getting all code constructs...");
                response = this.client.GetAsync("Codeconstructs").Result;
            }
            else
            {
                Console.WriteLine($"Getting construct with constructId {constructId}...");
                response = this.client.GetAsync($"Codeconstructs/{constructId}").Result;
            }
            string responseStr = await response.Content.ReadAsStringAsync();
            this.responseTuple = (response, responseStr);
        }

        public async void HitCodeConstructsByConstructTypeId(string constructTypeId = "")
        {
            HttpResponseMessage response;
            response = this.client.GetAsync($"Codeconstructs/getByConstructType/{constructTypeId}").Result;
            string responseStr = await response.Content.ReadAsStringAsync();
            this.responseTuple = (response, responseStr);
        }

    }
}
