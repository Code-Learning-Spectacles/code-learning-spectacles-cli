using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace code_learning_spectacles_cli.Models
{
    public class LanguageConstruct
    {
        public int languageconstructid { get; set; }
        public int codinglanguageid { get; set; }
        public int codeconstructid { get; set; }
        public string construct { get; set; }
        public object codeconstruct { get; set; }
        public object codinglanguage { get; set; }
        public object[] profilelanguageconstructs { get; set; }
    }

}