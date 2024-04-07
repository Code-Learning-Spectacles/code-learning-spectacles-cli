using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static code_spectacles_client.Program;
using code_spectacles_client.AllEndPoints;
//using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using static code_spectacles_client.AllEndPoints.ProfileLanguageConstructs;
namespace code_spectacles_client
{

    internal class Endpoints
    {
        string baseUri = "https://localhost:7107/api/";
        public HttpClient client = new HttpClient();
        // All responses are stored in this tuple
        (HttpResponseMessage response, string responseStr) responseTuple;
        private readonly int profileId = 2; // THIS HAS TO BE UPDATED WITH LOGIN

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
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success");
            Console.ResetColor();
        }

        public void GetCodeConstructs(string constructId = "")
        {
            CodeConstructs codeConstructsObj = new CodeConstructs(this.client);
            codeConstructsObj.HitCodeConstructs(constructId);
            this.responseTuple = codeConstructsObj.GetResponse();
            PrintResponse(responseTuple.response, responseTuple.responseStr);
        }

        public void GetCodingLanguages(string languageId = "")
        {
            CodingLanguages codingLanguagesObj = new CodingLanguages(this.client);
            codingLanguagesObj.HitCodingLanguages(languageId);
            this.responseTuple = codingLanguagesObj.GetResponse();
            PrintNamesFromResponse(responseTuple.responseStr);
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
            ConstructTypes constructTypesObj = new ConstructTypes(this.client);
            constructTypesObj.HitConstructTypes(constructTypeId);
            this.responseTuple = constructTypesObj.GetResponse();
            PrintConstructs(responseTuple.response, responseTuple.responseStr);
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
            LanguageConstructs langConstructsObj = new LanguageConstructs(this.client);
            langConstructsObj.HitLanguageConstructs(langConstructId);
            this.responseTuple = langConstructsObj.GetResponse();
            PrintResponse(responseTuple.response, responseTuple.responseStr);
        }

        public void GetProfileLanguageConstructs(string profLangConstructId = "")
        {
            ProfileLanguageConstructs profLangConstructsObj = new ProfileLanguageConstructs(this.client);
            profLangConstructsObj.HitProfileLanguageConstructs(profLangConstructId);
            this.responseTuple = profLangConstructsObj.GetResponse();
            PrintResponse(responseTuple.response, responseTuple.responseStr);
        }

        public void GetProfileLanguageConstructsNotes(string profLangConstructId = "")
        {
            ProfileLanguageConstructs profLangConstructsObj = new ProfileLanguageConstructs(this.client);
            profLangConstructsObj.HitProfileLanguageConstructs(profLangConstructId);
            this.responseTuple = profLangConstructsObj.GetResponse();

            // Extract notes from the response
            var notes = ExtractNotesFromResponse(responseTuple.response);

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
            Profiles profilesObj = new Profiles(this.client);
            profilesObj.HitProfiles(profileId);
            this.responseTuple = profilesObj.GetResponse();
            PrintResponse(responseTuple.response, responseTuple.responseStr);
        }

        public void FavouriteConstruct(int constructId, string note)
        {
            ProfileLanguageConstructs langObj = new ProfileLanguageConstructs(this.client);
            Favourite payload = new Favourite
            {
                profileId = this.profileId,
                languageconstructId = constructId,
                notes = note
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


    }
}
