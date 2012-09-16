using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Windows;
using System.Windows.Forms;

namespace DecisionEvaluator
{
    public enum Symbol
    {
        None,
        LPar,
        RPar,
        Equals,
        Text,
        NumberIf,
        If,
        NumberElse,
        NumberEnd,
        Identifier
    }

   

    public class LanguageTranslator : ILanguageTranslator
    {
        private ITextReader _input;
        private Queue<string> _textQueue;

        public LanguageTranslator(ITextReader input)
        {
            _input = input;
            _textQueue = new Queue<string>();
        }
        
        public Symbol CurrentSymbol { get; private set; }
        public string IdentifierOrText { get; private set; }

        public void GetSymbol()
        {
            string s;
            if (_textQueue.Count > 0)
            { // Buffered text parts available, use one from these.
                s = _textQueue.Dequeue();
                switch (s.ToLower())
                {
                    case "(":
                        CurrentSymbol = Symbol.LPar;
                        break;
                    case ")":
                        CurrentSymbol = Symbol.RPar;
                        break;
                    case "=":
                        CurrentSymbol = Symbol.Equals;
                        break;
                    case "if":
                        CurrentSymbol = Symbol.If;
                        break;
                    default:
                        CurrentSymbol = Symbol.Identifier;
                        IdentifierOrText = s;
                        break;
                }
                return;
            }

            // Get next line from input.
            s = _input.ReadLine();
            if (s == null)
            {
                CurrentSymbol = Symbol.None;
                return;
            }

            s = s.Trim(' ', '\t');
            if (s[0] == '#')
            { // We have a preprocessor directive.
                // Split the line in order to be able get its symbols.
                string[] parts = Regex.Split(s, @"\b|[^#_a-zA-Z0-9()=]");
                // parts[0] = #
                // parts[1] = if, else, end
                switch (parts[1].ToLower())
                {
                    case "if":
                        CurrentSymbol = Symbol.NumberIf;
                        break;
                    case "else":
                        CurrentSymbol = Symbol.NumberElse;
                        break;
                    case "end":
                        CurrentSymbol = Symbol.NumberEnd;
                        break;
                    default:
                        Error("Invalid symbol #{0}", parts[0]);
                        break;
                }

                // Store the remaining parts for later.
                for (int i = 2; i < parts.Length; i++)
                {
                    string part = parts[i].Trim(' ', '\t');
                    if (part != "")
                    {
                        _textQueue.Enqueue(part);
                    }
                }
            }
            else
            { // We have an ordinary text line.
                CurrentSymbol = Symbol.Text;
                IdentifierOrText = s;
            }
        }

        public void Error(string message, params  object[] args)
        {
            // Make sure parsing stops here
            CurrentSymbol = Symbol.None;
            _textQueue.Clear();
            _input.Clear();

            message = String.Format(message, args) +
                      String.Format(" in line {0}\r\n\r\n{1}", _input.CurrentLineNo, _input.CurrentLine);
            Output("------");
            Output(message);
            MessageBox.Show(message, "Error");
        }

        /// <summary>
        /// Writes the processed line to a (simulated) output stream.
        /// </summary>
        /// <param name="line">Line to be written to output</param>
        void Output(string line)
        {
            Console.WriteLine(line);
        }
    }
}
