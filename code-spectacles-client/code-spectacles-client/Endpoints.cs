using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static code_spectacles_client.Program;

namespace code_spectacles_client
{
    internal class Endpoints
    {
        static HttpClient client = new HttpClient();

        public Endpoints()
        {
            RunAsync();
        }
        static async void RunAsync()
        {
            Console.WriteLine("We are in RunAsync");
            // Update port # in the following line.
            client.BaseAddress = new Uri("https://localhost:7107/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

        }

        // GET
        public async void GetCodeConstructs()
        {
            Console.WriteLine("Getting code constructs...");
            HttpResponseMessage response = client.GetAsync("Codeconstructs").Result;
            string responseStr = await response.Content.ReadAsStringAsync();
            printResponse(response, responseStr);
            response.ToString();
        }

        private void printResponse(HttpResponseMessage response, string responseStr)
        {
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("═══════════════════════════════════════════════════════════════════════════════════════════════════════════════════════");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Status code: ");
                Console.Write(response.StatusCode.ToString() + '\n');
                Console.ResetColor();
                Console.WriteLine("Response received from GET on https://localhost:7107/api/Codeconstructs:");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(responseStr);
                Console.ResetColor();
                Console.WriteLine("═══════════════════════════════════════════════════════════════════════════════════════════════════════════════════════");
            }
            else
            {
                Console.WriteLine("═══════════════════════════════════════════════════════════════════════════════════════════════════════════════════════");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Status code: ");
                Console.Write(response.StatusCode.ToString() + '\n');
                Console.ResetColor();
                Console.WriteLine("═══════════════════════════════════════════════════════════════════════════════════════════════════════════════════════");
            }
        }
    }
}
