using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace code_spectacles_client
{
    class Program
    {
        static void Main(string[] args)
        {
            Endpoints codeSpecsEndpoints = new Endpoints();

            Console.WriteLine("****Testing Codeconstructs endpoint****");
            codeSpecsEndpoints.GetCodeConstructs();
            codeSpecsEndpoints.GetCodeConstructs("2");

            Console.WriteLine("****Testing Codinglanguages endpoint****");
            codeSpecsEndpoints.GetCodingLanguages();
            codeSpecsEndpoints.GetCodingLanguages("3");

            Console.WriteLine("****Testing Constructtypes endpoint****");
            codeSpecsEndpoints.GetConstructTypes();
            codeSpecsEndpoints.GetConstructTypes("1");

            Console.WriteLine("****Testing Languageconstructs endpoint****");
            codeSpecsEndpoints.GetLanguageConstruct();
            codeSpecsEndpoints.GetLanguageConstruct("20");

            Console.WriteLine("****Testing Profilelanguageconstructs endpoint****");
            codeSpecsEndpoints.GetProfileLanguageConstructs();
            codeSpecsEndpoints.GetProfileLanguageConstructs("1");

            Console.WriteLine("****Testing Profiles endpoint****");
            codeSpecsEndpoints.GetProfiles();
            codeSpecsEndpoints.GetProfiles("2");

            Console.WriteLine("Done");
        }
    }
}
