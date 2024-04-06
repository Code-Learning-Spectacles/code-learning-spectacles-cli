using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static code_spectacles_client.Program;
using code_spectacles_client.AllEndPoints;

namespace code_spectacles_client
{
    internal class Endpoints
    {
        string baseUri = "https://localhost:7107/api/";
        public HttpClient client = new HttpClient();
        // All responses are stored in this tuple
        (HttpResponseMessage response, string responseStr) responseTuple;

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

        // GET all code constructs
        public void GetCodeConstructs(string constructId = "")
        {
            CodeConstructs codeConstructsObj = new CodeConstructs(this.client);
            codeConstructsObj.HitCodeConstructs(constructId);
            this.responseTuple = codeConstructsObj.GetResponse();
            PrintResponse(responseTuple.response, responseTuple.responseStr);
        }
        // GET specific code construct
        //public void GetCodeConstructsByConstructId(string constructId)
        //{
        //    CodeConstructs codeConstructsObj = new CodeConstructs(this.client);
        //    codeConstructsObj.HitCodeConstructsByConstructId(constructId);
        //    this.responseTuple = codeConstructsObj.GetResponse();
        //    PrintResponse(responseTuple.response, responseTuple.responseStr);
        //}

        // GET all coding languages
        public void GetCodingLanguages(string languageId = "")
        {
            CodingLanguages codingLanguagesObj = new CodingLanguages(this.client);
            codingLanguagesObj.HitCodingLanguages(languageId);
            this.responseTuple = codingLanguagesObj.GetResponse();
            PrintResponse(responseTuple.response, responseTuple.responseStr);
        }

        // GET coding language by Id
        //public void GetCodingLanguageById(string languageId)
        //{
        //    CodingLanguages codingLanguagesObj = new CodingLanguages(this.client);
        //    codingLanguagesObj.HitCodingLanguageById(languageId);
        //    this.responseTuple = codingLanguagesObj.GetResponse();
        //    PrintResponse(responseTuple.response, responseTuple.responseStr);
        //}

        public void GetConstructTypes(string constructTypeId = "")
        {
            ConstructTypes constructTypesObj = new ConstructTypes(this.client);
            constructTypesObj.HitConstructTypes(constructTypeId);
            this.responseTuple = constructTypesObj.GetResponse();
            PrintResponse(responseTuple.response, responseTuple.responseStr);
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

        public void GetProfiles(string profileId = "")
        {
            Profiles profilesObj = new Profiles(this.client);
            profilesObj.HitProfiles(profileId);
            this.responseTuple = profilesObj.GetResponse();
            PrintResponse(responseTuple.response, responseTuple.responseStr);
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
