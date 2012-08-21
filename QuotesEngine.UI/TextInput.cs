using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Swinton.QuotesEngine.UI
{
    public interface ITextInput
    {
        int CurrentLineNo { get; }
        string CurrentLine { get; }
        string GetLine();
        void Clear();
    }

    public class TextInput : ITextInput
    {
        private List<string> _input;

        int _currentLineIndex;
        
        public TextInput(List<string> input)
        {
            _input = input;
        }

        public int CurrentLineNo { get; private set; }
        public string CurrentLine { get; private set; }


        public string GetLine()
        {
            if (_currentLineIndex >= _input.Count)
            {
                return null;
            }
            CurrentLine = _input[_currentLineIndex++];
            CurrentLineNo = _currentLineIndex;
            return CurrentLine;
        }

        public void Clear()
        {
            _input.Clear();
        }
    }
}
