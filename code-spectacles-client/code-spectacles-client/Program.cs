using System;
using System.Net;

namespace code_spectacles_client
{
    class Program
    {
        static void Main(string[] args)
        {
            Endpoints codeSpecsEndpoints = new Endpoints();

            while (true)
            {
                Console.WriteLine(">> Please provide a command or type '--help' to get a list of available commands:");
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
            Console.WriteLine(">> GetCodeConstructs: Retrieves all code constructs or a specific code construct by ID.");
            Console.WriteLine(">> GetCodingLanguages: Retrieves all coding languages or a specific coding language by ID.");
            Console.WriteLine(">> GetConstructTypes: Retrieves all construct types or a specific construct type by ID.");
            Console.WriteLine(">> GetLanguageConstruct: Retrieves all language constructs or a specific language construct by ID.");
            Console.WriteLine(">> GetProfileLanguageConstructs: Retrieves all profile language constructs or a specific profile language construct by ID.");
            Console.WriteLine(">> GetProfiles: Retrieves all profiles or a specific profile by ID.");
            Console.WriteLine(">> GetProfileLanguageConstructsNotes: Retrieves the notes for the language constructor");
        }

        static void ProcessCommand(Endpoints endpoints, string command)
        {
            switch (command)
            {
                case "getcodeconstructs":
                    endpoints.GetCodeConstructs();
                    break;
                case "getcodinglanguages":
                    endpoints.GetCodingLanguages();
                    break;
                case "getconstructtypes":
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
                case "GetProfileLanguageConstructsNotes":
                    endpoints.GetProfileLanguageConstructsNotes();
                    break;
                default:
                    Console.WriteLine(">> Invalid command.");
                    break;
            }
        }
    }
}
