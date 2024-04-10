using System.Net.Http.Headers;
using code_learning_spectacles_cli.AllEndPoints;
using System.Text.Json;
using System.Net.Http;
using System.Collections.Generic;
using System;
using code_learning_spectacles_cli.Models;
namespace code_learning_spectacles_cli
{
    internal class Endpoints
    {
        string baseUri = "https://localhost:7107/api/v1/";
        // Static mean there is only ever one instance of client, no matter how many Endpoint objects you have
        public static HttpClient client = new HttpClient();
        // All responses are stored in this tuple
        (HttpResponseMessage response, string responseStr) responseTuple;
        public struct Payload
        {
            public int constructId;
            public string note;
        }

        public Endpoints()
        {
            ConnectClient();
        }
        private void ConnectClient()
        {
            // Update port # in the following line.
            client.BaseAddress = new Uri(baseUri);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Attempting to connect to {client.BaseAddress}...");
            Console.ResetColor();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            CodeConstructs codeConstructsObj = new CodeConstructs(client);
            codeConstructsObj.HitCodeConstructs();
            this.responseTuple = codeConstructsObj.GetResponse();
            if (this.responseTuple.response.IsSuccessStatusCode)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Client connection successful");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Could not establish connection to {client.BaseAddress}");
                Console.ResetColor();
            }
        }

        // GET all code constructs
        public void GetCodeConstructs(string constructId = "")
        {
            CodeConstructs codeConstructsObj = new CodeConstructs(client);
            codeConstructsObj.HitCodeConstructs(constructId);
            this.responseTuple = codeConstructsObj.GetResponse();
            PrintConstructTypes(this.responseTuple.response, this.responseTuple.responseStr);
        }

        public void PrintConstructTypes(HttpResponseMessage response, string jsonResponse)
        {
            if (response.IsSuccessStatusCode)
            {
                using (JsonDocument document = JsonDocument.Parse(jsonResponse))
                {
                    JsonElement root = document.RootElement;
                    if (root.ValueKind == JsonValueKind.Array)
                    {
                        int index = 1;
                        foreach (JsonElement element in root.EnumerateArray())
                        {
                            JsonElement nameElement;
                            if (element.TryGetProperty("name", out nameElement) && nameElement.ValueKind == JsonValueKind.String)
                            {
                                string name = nameElement.GetString();
                                Console.WriteLine($"{index}. {name}");
                                index++;
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
        }

        // GET all coding languages
        public void GetCodingLanguages(string languageId = "")
        {
            CodingLanguages codingLanguagesObj = new CodingLanguages(client);
            codingLanguagesObj.HitCodingLanguages(languageId);
            this.responseTuple = codingLanguagesObj.GetResponse();
            PrintNamesFromResponse(this.responseTuple.responseStr);
        }

        // Method to print only the "name" field from the response
        // Method to print only the "name" field from the response as a numbered list
        private void PrintNamesFromResponse(string jsonResponse)
        {
            using (JsonDocument document = JsonDocument.Parse(jsonResponse))
            {
                JsonElement root = document.RootElement;
                if (root.ValueKind == JsonValueKind.Array)
                {
                    int index = 1;
                    foreach (JsonElement element in root.EnumerateArray())
                    {
                        JsonElement nameElement;
                        if (element.TryGetProperty("name", out nameElement) && nameElement.ValueKind == JsonValueKind.String)
                        {
                            string name = nameElement.GetString();
                            Console.WriteLine($"{index}. {name}");
                            index++;
                        }
                    }
                }
            }
        }


        public void GetConstructTypes(string constructTypeId = "")
        {
            ConstructTypes constructTypesObj = new ConstructTypes(client);
            constructTypesObj.HitConstructTypes(constructTypeId);
            this.responseTuple = constructTypesObj.GetResponse();
            PrintConstructs(this.responseTuple.response, this.responseTuple.responseStr);
        }

        // Method to print only the "name" field from the response as a numbered list
        private void PrintConstructs(HttpResponseMessage response, string jsonResponse)
        {
            if (response.IsSuccessStatusCode)
            {
                using (JsonDocument document = JsonDocument.Parse(jsonResponse))
                {
                    JsonElement root = document.RootElement;
                    if (root.ValueKind == JsonValueKind.Array)
                    {
                        int index = 1;
                        foreach (JsonElement element in root.EnumerateArray())
                        {
                            JsonElement nameElement;
                            if (element.TryGetProperty("name", out nameElement) && nameElement.ValueKind == JsonValueKind.String)
                            {
                                string name = nameElement.GetString();
                                Console.WriteLine($"{index}. {name}");
                                index++;
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
        }

        public void GetLanguageConstruct(string langConstructId = "")
        {
            LanguageConstructs langConstructsObj = new LanguageConstructs(client);
            langConstructsObj.HitLanguageConstructs(langConstructId);
            this.responseTuple = langConstructsObj.GetResponse();
            PrintResponse(this.responseTuple.response, this.responseTuple.responseStr);
        }

        public void GetProfileLanguageConstructs(string profLangConstructId = "")
        {
            ProfileLanguageConstructs profLangConstructsObj = new ProfileLanguageConstructs(client);
            profLangConstructsObj.HitProfileLanguageConstructs(profLangConstructId);
            this.responseTuple = profLangConstructsObj.GetResponse();
            PrintResponse(this.responseTuple.response, this.responseTuple.responseStr);
        }

        public void GetProfileLanguageConstructsNotes(string profLangConstructId = "")
        {
            ProfileLanguageConstructs profLangConstructsObj = new ProfileLanguageConstructs(client);
            profLangConstructsObj.HitProfileLanguageConstructs(profLangConstructId);
            this.responseTuple = profLangConstructsObj.GetResponse();

            // Extract notes from the response
            var notes = ExtractNotesFromResponse(this.responseTuple.response);

            // Print the notes
            Console.WriteLine($"Notes: {notes}");
        }

        // Method to extract notes from the response
        private string ExtractNotesFromResponse(dynamic response)
        {
            // Assuming response is an array and we're interested in the first item's notes
            string notes = response[0].notes;
            return notes;
        }


        public void GetProfiles(string profileId = "")
        {
            Profiles profilesObj = new Profiles(client);
            profilesObj.HitProfiles(profileId);
            this.responseTuple = profilesObj.GetResponse();
            PrintResponse(this.responseTuple.response, this.responseTuple.responseStr);
        }

        public void FavouriteConstruct(int constructId, string note)
        {
            ProfileLanguageConstructs langObj = new ProfileLanguageConstructs(client);
            Payload payload = new Payload
            {
                constructId = constructId,
                note = note
            };
            langObj.PostNote(payload);
        }

        public void PrintResponse(HttpResponseMessage response, string responseStr)
        {
            if (response.IsSuccessStatusCode)
            {
                //Console.WriteLine("═══════════════════════════════════════════════════════════════════════════════════════════════════════════════════════");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Status code: ");
                Console.Write(response.StatusCode.ToString() + '\n');
                Console.ResetColor();
                Console.WriteLine($"Response received from GET on {response.RequestMessage.RequestUri}:");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(responseStr);
                Console.ResetColor();
                Console.WriteLine("═══════════════════════════════════════════════════════════════════════════════════════════════════════════════════════");
            }
            else
            {
                //Console.WriteLine("═══════════════════════════════════════════════════════════════════════════════════════════════════════════════════════");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Status code: ");
                Console.Write(response.StatusCode.ToString() + '\n');
                Console.ResetColor();
                Console.WriteLine("═══════════════════════════════════════════════════════════════════════════════════════════════════════════════════════");
            }
        }

        public void chooseLanguageConstruct()
        {

            Console.WriteLine("\n>> Please choose a coding language:\n");
            GetCodingLanguages();
            Console.Write("\n>> Enter the number corresponding to your choice: ");
            string languageChoice = Console.ReadLine();


            Console.WriteLine("\n>> Please choose a coding construct type:\n");
            GetConstructTypes();
            Console.Write("\n>> Enter the number corresponding to your choice: ");
            string constructChoice = Console.ReadLine();
            Console.Write("\n");


            // Now you can call the appropriate method to fetch the desired data based on the choices
            FetchData(languageChoice, constructChoice);
        }

        private Dictionary<int, string> languageConstructs = new Dictionary<int, string>();

        private void FetchData(string languageId, string constructTypeId)
        {
            // Call GetLanguageConstruct to fetch data and store response
            GetSpecifidLanguageConstruct(languageId, constructTypeId);
        }


        // Method to fetch language constructs and store response
        private void GetSpecifidLanguageConstruct(string langConstructId, string codeconstructid)
        {
            // Convert langConstructId and codeconstructid to integers
            int langId;
            int constructId;

            if (!int.TryParse(langConstructId, out langId))
            {
                // Handle the case where langConstructId cannot be parsed as an integer
                Console.WriteLine("Invalid langConstructId. Please provide a valid integer value.");
                return;
            }

            if (!int.TryParse(codeconstructid, out constructId))
            {
                // Handle the case where codeconstructid cannot be parsed as an integer
                Console.WriteLine("Invalid codeconstructid. Please provide a valid integer value.");
                return;
            }

            LanguageConstruct[] languageConstructs; // Define the array to hold language constructs

            LanguageConstructs langConstructsObj = new LanguageConstructs(client);
            langConstructsObj.HitLanguageConstructs();
            this.responseTuple = langConstructsObj.GetResponse();
            PrintLanguageConstruct(this.responseTuple.response, this.responseTuple.responseStr, langId, constructId);
        }


        private void PrintLanguageConstruct(HttpResponseMessage response, string jsonResponse, int langConstructId, int constructId)
        {
            if (response.IsSuccessStatusCode)
            {
                using (JsonDocument document = JsonDocument.Parse(jsonResponse))
                {
                    JsonElement root = document.RootElement;
                    if (root.ValueKind == JsonValueKind.Array)
                    {
                        int index = 1;
                        foreach (JsonElement element in root.EnumerateArray())
                        {
                            // Check if the JSON object has the expected structure
                            JsonElement codingLanguageIdElement;
                            JsonElement codeConstructIdElement;
                            if (element.TryGetProperty("codinglanguageid", out codingLanguageIdElement) && codingLanguageIdElement.ValueKind == JsonValueKind.Number &&
                                element.TryGetProperty("codeconstructid", out codeConstructIdElement) && codeConstructIdElement.ValueKind == JsonValueKind.Number)
                            {
                                int codingLanguageId = codingLanguageIdElement.GetInt32();
                                int codeConstructId = codeConstructIdElement.GetInt32();

                                // Check if the current object matches the provided criteria
                                if (codingLanguageId == langConstructId && codeConstructId == constructId)
                                {
                                    JsonElement nameElement;
                                    if (element.TryGetProperty("construct", out nameElement) && nameElement.ValueKind == JsonValueKind.String)
                                    {
                                        string name = nameElement.GetString();
                                        Console.WriteLine($"{index}. {name}");
                                        index++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
        }


    }
}
