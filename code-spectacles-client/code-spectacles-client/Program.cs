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
            Console.WriteLine("****Testing Codeconstructs endpoint****");
            Endpoints codeSpecsEndpoints = new Endpoints();
            codeSpecsEndpoints.GetCodeConstructs();

            Console.WriteLine("done");
        }
    }
}
