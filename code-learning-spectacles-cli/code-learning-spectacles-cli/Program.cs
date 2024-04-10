using code_learning_spectacles_cli.Models;
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
            Console.WriteLine(Environment.GetEnvironmentVariable("ACCESS_TOKEN"));
            Endpoints codeSpecsEndpoints = new Endpoints();
            Authenticator authenticator = new Authenticator();
            authenticator.AuthenticateAsync();
            while (!Authenticator.authenticationSuccessful)
            {

            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("You have been successfully authenticated");
            Console.ResetColor();
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

            Console.WriteLine(">> Done");
        }

        static void DisplayHelp()
        {
            Console.WriteLine(">> Available commands and their functions:");
            Console.WriteLine(">> available-constructs: Retrieves all code constructs or a specific code construct by ID.");
            Console.WriteLine(">> coding-languages: Retrieves all coding languages or a specific coding language by ID.");
            Console.WriteLine(">> construct-types: Retrieves all construct types or a specific construct type by ID.");
            Console.WriteLine(">> GetLanguageConstruct: Retrieves all language constructs or a specific language construct by ID.");
            Console.WriteLine(">> GetProfileLanguageConstructs: Retrieves all profile language constructs or a specific profile language construct by ID.");
            Console.WriteLine(">> GetProfiles: Retrieves all profiles or a specific profile by ID.");
            Console.WriteLine(">> GetProfileLanguageConstructsNotes: Retrieves the notes for the language constructor");
            Console.WriteLine(">> get-construct: Returns the construt type for a given language");
            Console.WriteLine("\n>> Short hand notation: java for-loop");
        }

        static void ProcessCommand(Endpoints endpoints, string command)
        {
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
                case "getlanguageconstruct":
                    endpoints.GetLanguageConstruct();
                    break;
                case "getprofilelanguageconstructs":
                    endpoints.GetProfileLanguageConstructs();
                    break;
                case "getprofiles":
                    endpoints.GetProfiles();
                    break;
                case "getprofilelanguageconstructsnotes":
                    endpoints.GetProfileLanguageConstructsNotes();
                    break;
                case "get-construct":
                    endpoints.GetSingleConstruct();
                    break;
                case "note":
                    endpoints.FavouriteConstruct(constructId: 1, note: "This is a note posted during testing");
                    break;
                default:
                    Console.WriteLine(">> Invalid command.");
                    break;
            }
        }
    }
}
