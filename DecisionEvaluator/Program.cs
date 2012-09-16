using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace DecisionEvaluator
{
    class Program
    {
        static void Main(string[] args)
        {
            var variableValues = new Dictionary<string, string>
                {
                    { "VariableA", "Case5" },
                    { "VariableB", "CaseX" }
                };

            var inputTxt = string.Empty;
            using (StreamReader reader = new StreamReader("Input.txt"))
            {
                 inputTxt = reader.ReadToEnd();
            }
            ILanguageTranslator translator = new LanguageTranslator(
                new TextReader(inputTxt));
              

            var parser = new StatementParser(translator, variableValues);

            parser.Parse();
        }
    }
}
