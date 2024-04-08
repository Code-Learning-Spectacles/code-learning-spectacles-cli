﻿using System;
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
            Console.WriteLine(">> get-construct-group: Returns the construct type group for a given language");
            Console.WriteLine(">> get-construct: Returns the construct for a specified language within a group of constructs");
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
                case "get-construct-group":
                    endpoints.chooseLanguageConstruct();
                    break;
                case "get-construct":
                    endpoints.chooseLanguageConstructByName();
                    break;
                default:
                    Console.WriteLine(">> Invalid command.");
                    break;
            }
        }
    }
}
