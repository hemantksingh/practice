using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DecisionEvaluator;

namespace DecisionEvaluator
{
    class StatementParser
    {
        private ILanguageTranslator _translator;
        private readonly Dictionary<string, string> _variableData;
        private StringBuilder _output;

        public StatementParser(ILanguageTranslator translator, Dictionary<string, string> variableData)
        {
            _translator = translator;
            _variableData = variableData;
            _output = new StringBuilder();
        }

        /// <summary>
        /// Writes the processed line to a (simulated) output stream.
        /// </summary>
        /// <param name="line">Line to be written to output</param>
        void Output(string line)
        {
            _output.AppendLine(line);
            Console.WriteLine(line);
        }

        /// <summary>
        /// Starts the parsing process.
        /// </summary>
        public string Parse()
        {
            // Get first symbol and start parsing
            _translator.GetSymbol();
            if (this.IsLineSequence(true))
            {
                // TODO: OK do something with the processed sql
            }
            else
            {
                Output("*** ABORTED ***");
            }

            ICondition condition = new IsEqualTo<string>("2", "2");
            condition.IsSatisfied();

            return _output.ToString();
        }

        // The following methods parse according the the EBNF syntax.

        bool IsLineSequence(bool writeOutput)
        {
            // EBNF:  LineSequence = { TextLine | IfStatement }.
            while (_translator.CurrentSymbol == Symbol.Text || _translator.CurrentSymbol == Symbol.NumberIf)
            {
                if (_translator.CurrentSymbol == Symbol.Text)
                {
                    if (!this.IsTextLine(writeOutput))
                    {
                        return false;
                    }
                }
                else
                { // _translator.CurrentSymbol == Symbol.NumberIf
                    if (!this.IsIfStatement(writeOutput))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        bool IsTextLine(bool writeOutput)
        {
            // EBNF:  TextLine = <string>.
            if (writeOutput)
            {
                Output(_translator.IdentifierOrText);
            }
            _translator.GetSymbol();
            return true;
        }

        bool IsIfStatement(bool writeOutput)
        {
            // EBNF:  IfStatement = IfLine LineSequence { ElseIfLine LineSequence } [ ElseLine LineSequence ] EndLine.
            bool result;
            if (IfLine(out result) && this.IsLineSequence(writeOutput && result))
            {
                writeOutput &= !result; // Only one section can produce an output.
                while (_translator.CurrentSymbol == Symbol.NumberElse)
                {
                    _translator.GetSymbol();
                    if (_translator.CurrentSymbol == Symbol.If)
                    { // We have an #else if
                        if (!ElseIfLine(out result))
                        {
                            return false;
                        }
                        if (!this.IsLineSequence(writeOutput && result))
                        {
                            return false;
                        }
                        writeOutput &= !result; // Only one section can produce an output.
                    }
                    else
                    { // We have a simple #else
                        if (!this.IsLineSequence(writeOutput))
                        {
                            return false;
                        }
                        break; // We can have only one #else statement.
                    }
                }
                if (_translator.CurrentSymbol != Symbol.NumberEnd)
                {
                    _translator.Error("'#end' expected");
                    return false;
                }
                _translator.GetSymbol();
                return true;
            }
            return false;
        }

        bool IfLine(out bool result)
        {
            // EBNF:  IfLine = "#if" "(" Condition ")".
            result = false;
            _translator.GetSymbol();
            if (_translator.CurrentSymbol != Symbol.LPar)
            {
                _translator.Error("'(' expected");
                return false;
            }
            _translator.GetSymbol();
            if (!this.IsCondition(out result))
            {
                return false;
            }
            if (_translator.CurrentSymbol != Symbol.RPar)
            {
                _translator.Error("')' expected");
                return false;
            }
            _translator.GetSymbol();
            return true;
        }

        private bool IsCondition(out bool result)
        {
            // EBNF:  Condition = Identifier "=" Identifier.
            string variable;
            string expectedValue;
            string variableValue;

            result = false;
            // Identifier "=" Identifier
            if (_translator.CurrentSymbol != Symbol.Identifier)
            {
                _translator.Error("Identifier expected");
                return false;
            }
            variable = _translator.IdentifierOrText; // The first identifier is a variable.
            _translator.GetSymbol();
            if (_translator.CurrentSymbol != Symbol.Equals)
            {
                _translator.Error("'=' expected");
                return false;
            }
            _translator.GetSymbol();
            if (_translator.CurrentSymbol != Symbol.Identifier)
            {
                _translator.Error("Value expected");
                return false;
            }
            expectedValue = _translator.IdentifierOrText;  // The second identifier is a value.

            // Search the variable
            if (_variableData.TryGetValue(variable, out variableValue))
            {
                //result = variableValue == expectedValue; // Perform the comparison.
                result = Evaluate<string>(variableValue, expectedValue, (x, y) => x == y);
            }
            else
            {
                _translator.Error("Variable '{0}' not found", variable);
                return false;
            }

            _translator.GetSymbol();
            return true;
        }

        static bool Evaluate<T>(T a, T y, Func<T, T, bool> function)
        {
            return function.Invoke(a, y);
        }

        bool ElseIfLine(out bool result)
        {
            // EBNF:  ElseIfLine = "#else" "if" "(" Condition ")".
            result = false;
            _translator.GetSymbol(); // "#else" already processed here, we are only called if the symbol is "if"
            if (_translator.CurrentSymbol != Symbol.LPar)
            {
                _translator.Error("'(' expected");
                return false;
            }
            _translator.GetSymbol();
            if (!this.IsCondition(out result))
            {
                return false;
            }
            if (_translator.CurrentSymbol != Symbol.RPar)
            {
                _translator.Error("')' expected");
                return false;
            }
            _translator.GetSymbol();
            return true;
        }
    }
}
