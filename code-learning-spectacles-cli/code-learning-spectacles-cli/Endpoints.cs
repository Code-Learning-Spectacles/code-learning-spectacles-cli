using System.Net.Http.Headers;
using code_learning_spectacles_cli.AllEndPoints;
using System.Text.Json;
using System.Net.Http;
using System.Collections.Generic;
using System;
using code_learning_spectacles_cli.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace code_learning_spectacles_cli
{
    internal class Endpoints
    {
        string baseUri = "https://localhost:7107/api/v1/";
        // Static mean there is only ever one instance of client, no matter how many Endpoint objects you have
        public static HttpClient client = new HttpClient();
        // All responses are stored in this tuple
        (HttpResponseMessage response, string responseStr) responseTuple;
        Dictionary<int, int> dictConstructTypes = new Dictionary<int, int>();

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

        public void GetCodeConstructsByType(string constructTypeId = "")
        {
            CodeConstructs codeConstructsByTypeObj = new CodeConstructs(client);
            codeConstructsByTypeObj.HitCodeConstructsByConstructTypeId(constructTypeId);
            this.responseTuple = codeConstructsByTypeObj.GetResponse();
            PrintConstructTypes(this.responseTuple.response, this.responseTuple.responseStr);
            //Console.WriteLine("adasd");
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
                        // <key, value> --> <index, constructTypeId>
                        foreach (JsonElement element in root.EnumerateArray())
                        {
                            JsonElement nameElement;
                            JsonElement constructTypeIdElement;
                            element.TryGetProperty("codeconstructid", out constructTypeIdElement);
                            if (element.TryGetProperty("name", out nameElement) && nameElement.ValueKind == JsonValueKind.String)
                            {
                                string name = nameElement.GetString();
                                Console.WriteLine($"{index}. {name}");
                                this.dictConstructTypes.Add(index, constructTypeIdElement.GetInt32());
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

        public void GetLanguageConstructByLanguageIdByConstructId(string langId, string constructId)
        {
            LanguageConstructs langConstructsObj = new LanguageConstructs(client);
            langConstructsObj.HitLanguageConstructsByLangIdByConstructId(langId, constructId);
            this.responseTuple = langConstructsObj.GetResponse();
            Console.WriteLine($"LanguageId: {langId}, codeConstructId: {constructId}");
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

        public void PrintResponse(HttpResponseMessage response, string responseStr, bool print = true)
        {
            if (!print) return;
            if (response.IsSuccessStatusCode)
            {
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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Status code: ");
                Console.Write(response.StatusCode.ToString() + '\n');
                Console.ResetColor();
                Console.WriteLine("═══════════════════════════════════════════════════════════════════════════════════════════════════════════════════════");
            }
        }

        public void GetSingleConstruct()
        {

            Console.WriteLine("\n>> Please choose a coding language:\n");
            GetCodingLanguages();
            Console.Write("\n>> Enter the number corresponding to your choice: ");
            string languageChoice = Console.ReadLine();
            while (!int.TryParse(languageChoice, out _))
            {
                Console.Write("\n>> Enter the number corresponding to your choice: ");
                languageChoice = Console.ReadLine();
            }

            Console.WriteLine("\n>> Please choose a coding construct type:\n");
            GetConstructTypes();
            Console.Write("\n>> Enter the number corresponding to your choice: ");
            string constructType = Console.ReadLine();
            while (!int.TryParse(constructType, out _))
            {
                Console.Write("\n>> Enter the number corresponding to your choice: ");
                constructType = Console.ReadLine();
            }

            Console.WriteLine("\n>> Please choose a specific coding construct:\n");
            GetCodeConstructsByType(constructType);
            Console.Write("\n>> Enter the number corresponding to your construct choice: ");
            string constructId = Console.ReadLine();
            int construct;
            while (!int.TryParse(constructId, out construct))
            {
                Console.Write("\n>> Enter the number corresponding to your choice: ");
                constructId = Console.ReadLine();
            }
            int codeConstructId = this.dictConstructTypes[construct];


            Console.Write("\n");
            // Now you can call the appropriate method to fetch the desired data based on the choices
            FetchData(languageChoice, codeConstructId.ToString());
            PrintAConstruct(this.responseTuple.responseStr);
            this.dictConstructTypes.Clear();
        }

        private Dictionary<int, string> languageConstructs = new Dictionary<int, string>();

        private void FetchData(string languageId, string constructId)
        {
            GetLanguageConstructByLanguageIdByConstructId(languageId, constructId);
            // Call GetLanguageConstruct to fetch data and store response
            //GetSpecifidLanguageConstruct(languageId, constructTypeId, constructId);
        }


        // Method to fetch language constructs and store response
        private void GetSpecifidLanguageConstruct(string langConstructId, string codeconstructTypeid, string constructId)
        {
            // Convert langConstructId and codeconstructid to integers
            int constructTypeId;
            int langId;
            int constructIdAsint;
            if (!int.TryParse(langConstructId, out langId))
            {
                // Handle the case where langConstructId cannot be parsed as an integer
                Console.WriteLine("Invalid langConstructId. Please provide a valid integer value.");
                return;
            }

            if (!int.TryParse(codeconstructTypeid, out constructTypeId))
            {
                // Handle the case where codeconstructid cannot be parsed as an integer
                Console.WriteLine("Invalid codeconstructid. Please provide a valid integer value.");
                return;
            }

            if (!int.TryParse(constructId, out constructIdAsint))
            {
                // Handle the case where constructId cannot be parsed as an integer
                Console.WriteLine("Invalid constructId. Please provide a valid integer value.");
                return;
            }

            LanguageConstruct[] languageConstructs; // Define the array to hold language constructs

            LanguageConstructs langConstructsObj = new LanguageConstructs(client);
            langConstructsObj.HitLanguageConstructs();
            this.responseTuple = langConstructsObj.GetResponse();
            //PrintLanguageConstruct(this.responseTuple.response, this.responseTuple.responseStr, langId, constructTypeId, constructIdAsint);
        }

        private void PrintAConstruct(string jsonString)
        {
            // Parse the JSON string
            JArray jsonArray = JArray.Parse(jsonString);

            // Extract the "construct" part
            string extractConstruct = jsonArray[0]["construct"].ToString();

            // Print the extracted construct
            Console.WriteLine(extractConstruct);
        }


        private void PrintLanguageConstruct(HttpResponseMessage response, string jsonResponse, int langConstructId, int constructTypeId, int constructIdAsint)
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
                            JsonElement codeConstructTypeIdElement;
                            //JsonElement codeConstructIdElement;
                            if (element.TryGetProperty("codinglanguageid", out codingLanguageIdElement) && codingLanguageIdElement.ValueKind == JsonValueKind.Number &&
                                element.TryGetProperty("codeconstructid", out codeConstructTypeIdElement) && codeConstructTypeIdElement.ValueKind == JsonValueKind.Number)
                            {
                                int codingLanguageId = codingLanguageIdElement.GetInt32();
                                int codeConstructId = codeConstructTypeIdElement.GetInt32();

                                // Check if the current object matches the provided criteria
                                if (codingLanguageId == langConstructId && codeConstructId == constructTypeId)
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
