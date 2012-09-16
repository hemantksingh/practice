using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DecisionEvaluator
{
    public class TextReader : ITextReader
    {
        private string[] _input;

        int _currentLineIndex;
        
        public TextReader(string inputTxt)
        {
            _input = inputTxt.Split(Environment.NewLine.ToCharArray(),
                StringSplitOptions.RemoveEmptyEntries);
        }

        public int CurrentLineNo { get; private set; }
        public string CurrentLine { get; private set; }


        public string ReadLine()
        {
            if (_currentLineIndex >= _input.Length)
            {
                return null;
            }
            CurrentLine = _input[_currentLineIndex++];
            CurrentLineNo = _currentLineIndex;
            return CurrentLine;
        }

        public void Clear()
        {
            Array.Clear(_input, 0, _input.Length);
        }
    }
}
