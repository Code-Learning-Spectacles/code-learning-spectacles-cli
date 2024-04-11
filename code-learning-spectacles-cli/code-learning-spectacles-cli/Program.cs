using code_learning_spectacles_cli.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace code_learning_spectacles_cli
{
    class Program
    {
        static void Main(string[] args)
        {
            Endpoints codeSpecsEndpoints = new Endpoints();

            string token = Helpers.ReadFromFile();
            if (token != "")
            {
                Environment.SetEnvironmentVariable("ACCESS_TOKEN", token);
                Endpoints.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Environment.GetEnvironmentVariable("ACCESS_TOKEN"));
                Authenticator.authenticationSuccessful = true;
                Authenticator.LoginHelper();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("You have been successfully authenticated");
                Console.ResetColor();
            }
            else
            {
                Authenticator.AuthenticateAsync();
                Endpoints.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Environment.GetEnvironmentVariable("ACCESS_TOKEN"));
                while (!Authenticator.authenticationSuccessful)
                {

                }
            }

            while (true)
            {
                Console.WriteLine("\n>> Please provide a command or type '--help' to get a list of available commands:");
                string input = Console.ReadLine().ToLower();

                if (input == "--help")
                {
                    DisplayHelp();
                }
                else if (input == "exit" || input == "close")
                {
                    break; // Exit the loop and end the program
                }
                else
                {
                    ProcessCommand(codeSpecsEndpoints, input);
                }
            }

            Console.WriteLine(">> Exiting");
        }

        static void DisplayHelp()
        {
            Console.WriteLine(">> Available commands and their functions:");
            Console.WriteLine(">> available-constructs: Retrieves all code constructs or a specific code construct by ID.");
            Console.WriteLine(">> coding-languages: Retrieves all coding languages or a specific coding language by ID.");
            Console.WriteLine(">> construct-types: Retrieves all construct types or a specific construct type by ID.");
            Console.WriteLine(">> get-construct: Returns the construt type for a given language");
            Console.WriteLine("\n>> Short hand notation: java for-loop");
        }

        static void ProcessCommand(Endpoints endpoints, string command)
        {
            //if (command != "note") endpoints.clearCurrentState();

            switch (command)
            {
                case "available-constructs":
                    endpoints.GetCodeConstructs();
                    break;
                case "coding-languages":
                    endpoints.GetCodingLanguages();
                    break;
                case "construct-types":
                    endpoints.GetConstructTypes();
                    break;
                case "get-construct":
                    endpoints.GetSingleConstruct();
                    break;
                case "note":
                    endpoints.FavouriteConstruct();
                    break;
                case "view-notes":
                    endpoints.ViewNotes();
                    endpoints.clearCurrentState();
                    break;
                default:
                    Console.WriteLine(">> Invalid command.");
                    break;
            }
        }
    }
}
