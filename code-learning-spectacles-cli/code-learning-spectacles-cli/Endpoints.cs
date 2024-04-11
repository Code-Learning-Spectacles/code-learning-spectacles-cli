using System.Net.Http.Headers;
using code_learning_spectacles_cli.AllEndPoints;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net;
namespace code_learning_spectacles_cli
{
    internal class Endpoints
    {
        // Static mean there is only ever one instance of client, no matter how many Endpoint objects you have
        public static HttpClient client = new HttpClient()
        {
            BaseAddress = new Uri("http://api-env.eba-8bvi8xmn.eu-west-1.elasticbeanstalk.com/api/v1/")
        };
        // All responses are stored in this tuple
        (HttpResponseMessage response, string responseStr) responseTuple;
        Dictionary<int, int> dictConstructTypes = new Dictionary<int, int>();
        Dictionary<string, string> dictLanguages = new Dictionary<string, string>();
        int codeConstructId = -1;
        int codeLanguageId = -1;
        int codeLanguageConstructId = -1;
        string currentConstruct = "";
        public struct Payload
        {
            public int languageconstructId;
            public string note;
        }

        public Endpoints()
        {
            dictLanguages.Add("1", "JavaScript");
            dictLanguages.Add("2", "C#");
            dictLanguages.Add("3", "Python");
            ConnectClient();
        }
        private void ConnectClient()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            CodeConstructs codeConstructsObj = new CodeConstructs(client);
        }
        public void clearCurrentState() 
        { 
            dictConstructTypes.Clear(); 
            codeConstructId = -1;
            codeLanguageId = -1;
            codeLanguageConstructId = -1;
            currentConstruct = "";
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
                            JsonElement constructIdElement;
                            element.TryGetProperty("codeconstructid", out constructIdElement);
                            if (element.TryGetProperty("name", out nameElement) && nameElement.ValueKind == JsonValueKind.String)
                            {
                                string name = nameElement.GetString();
                                Console.WriteLine($"{index}. {name}");
                                this.dictConstructTypes.Add(index, constructIdElement.GetInt32());
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
        }

        public void GetLanguageConstructByLanguageIdByConstructId(string langId, string constructId)
        {
            LanguageConstructs langConstructsObj = new LanguageConstructs(client);
            langConstructsObj.HitLanguageConstructsByLangIdByConstructId(langId, constructId);
            this.responseTuple = langConstructsObj.GetResponse();
        }

        public void FavouriteConstruct()
        {
            if (codeConstructId == -1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("View a construct first with 'get-construct'");
                Console.ResetColor();
                return;
            }
            Console.WriteLine(">> Enter some notes below:");
            string note = Console.ReadLine();
            ProfileLanguageConstructs langObj = new ProfileLanguageConstructs(client);
            Payload payload = new Payload
            {
                languageconstructId = codeLanguageConstructId,
                note = note
            };
            langObj.HitPostNote(payload);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Note Posted, view notes with command 'view-notes'");
            Console.ResetColor();
        }
        public void ViewNotes()
        {
            ProfileLanguageConstructs profLangConstructsObj = new ProfileLanguageConstructs(client);
            profLangConstructsObj.HitViewNotes();
            this.responseTuple = profLangConstructsObj.GetResponse();
            PrintNotes(responseTuple.responseStr);
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
            codeLanguageId = Int32.Parse(languageChoice);

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
            codeConstructId = this.dictConstructTypes[construct];

            Console.Write("\n");
            // Now you can call the appropriate method to fetch the desired data based on the choices
            FetchData(languageChoice, codeConstructId.ToString());
            PrintAConstruct(this.responseTuple.responseStr);
        }

        private void FetchData(string languageId, string constructId)
        {
            GetLanguageConstructByLanguageIdByConstructId(languageId, constructId);
            // Call GetLanguageConstruct to fetch data and store response
            //GetSpecifidLanguageConstruct(languageId, constructTypeId, constructId);
        }

        private void PrintAConstruct(string jsonString)
        {
            // Parse the JSON string
            JArray jsonArray = JArray.Parse(jsonString);

            // Extract the "construct" part
            string extractConstruct = jsonArray[0]["construct"].ToString();
            currentConstruct = extractConstruct;
            codeLanguageConstructId = (int)jsonArray[0]["languageconstructid"];
            // Print the extracted construct
            Console.WriteLine(extractConstruct);
        }

        private void PrintNotes(string jsonString)
        {
            // Parse the JSON string
            JArray jsonArray = JArray.Parse(jsonString);

            foreach (JObject jsonObject in jsonArray) 
            {
                string note = jsonObject["notes"].ToString();
                string languageconstructid = jsonObject["languageconstructid"].ToString();
                GetLanguageConstruct(languageconstructid);
                string codingLanguage = HelperGetCodingLanguage(responseTuple.responseStr);
                string construct = HelperGetConstruct(responseTuple.responseStr);
                Console.WriteLine($"Language: {codingLanguage}\nConstruct: {construct}");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Note: {note}");
                Console.WriteLine("═══════════════════════════════════════════════════════════════════");

                Console.ResetColor();
            }
        }

        private string HelperGetCodingLanguage(string jsonString)
        {
            var jsonArray = JObject.Parse(jsonString);
            JObject jsonObject = JObject.Parse(jsonString);
            int codingLanguageId = (int)jsonObject["codinglanguageid"];
            return dictLanguages[codingLanguageId.ToString()];
        }

        private string HelperGetConstruct(string jsonString)
        {
            var jsonArray = JObject.Parse(jsonString);
            JObject jsonObject = JObject.Parse(jsonString);
            return (string)jsonObject["construct"];
        }
    }
}
