namespace FourierBox
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Text;

    public class ExpressionReader
    {
        private StringReader _reader;
        private StringBuilder sb = new StringBuilder();
        public ExpressionReader(string s)
        {
            if (string.IsNullOrEmpty(s))
                throw new FormatException("Input string was not in a correct format.");
            _reader = new StringReader(s);
        }

        public void MoveToContent()
        {
            while (_reader.Peek() != -1 && char.IsWhiteSpace(PeekChar()))
            {
                _reader.Read();
            }
        }
        public int ReadSign()
        {
            MoveToContent();
            var c = PeekChar();
            if (char.IsNumber(c) || c == 'x')
            {
                return 1;
            }
            if (c == '+')
            {
                _reader.Read();
                return 1;
            }
            if (c == '-')
            {
                _reader.Read();
                return -1;
            }
            throw new FormatException("Input string was not in a correct format.");
        }
        public double ReadDouble()
        {
            MoveToContent();
            this.sb.Clear();
            var c = PeekChar();
            if (!char.IsNumber(c))
                throw new FormatException("Input string was not in a correct format.");

            while (char.IsNumber(c) || c == '.' || c == 'e' || c == 'E')
            {
                c = ReadChar();
                sb.Append(c);
                if (IsEos)
                    break;
                c = PeekChar();
            }
            var value = double.Parse(sb.ToString(), CultureInfo.InvariantCulture);
            sb.Clear();
            return value;
        }
        public int ReadInt()
        {
            MoveToContent();
            this.sb.Clear();
            var c = PeekChar();
            while (char.IsNumber(c))
            {
                c = ReadChar();
                sb.Append(c);
                if (IsEos)
                    break;
                c = PeekChar();
            }
            var value = int.Parse(sb.ToString());
            sb.Clear();
            return value;
        }
        public int ReadDegree()
        {
            MoveToContent();
            var c = PeekChar();
            if (c == 'x')
            {
                _reader.Read();
                MoveToContent();
                c = PeekChar();
                if (c != '^')
                {
                    return 1;
                }
                _reader.Read();
                return ReadInt();
            }
            if (c == '*')
            {
                _reader.Read();
                MoveToContent();
                c = PeekChar();
                if (char.IsNumber(c) || c == 'x')
                    return ReadDegree();
                throw new FormatException("Input string was not in a correct format.");
            }
            return 0;
        }
        public double ReadCoefficient()
        {
            MoveToContent();
            var c = PeekChar();
            if (c == 'x')
            {
                return 1;
            }
            return ReadDouble();
        }
        public bool IsEos
        {
            get
            {
                MoveToContent();
                return _reader.Peek() == -1;
            }
        }
        public void ReadLeftSide()
        {
            while (PeekChar() != '=' && _reader.Peek() != -1)
            {
                _reader.Read();
            }
            var c = ReadChar();
            if (c == '=')
                return;
            throw new FormatException("Input string was not in a correct format.");
        }
        public bool HasLeftSide
        {
            get
            {
                MoveToContent();
                return PeekChar() == 'y';
            }
        }
        public char PeekChar()
        {
            return (char)_reader.Peek();
        }
        public char ReadChar()
        {
            return (char)_reader.Read();
        }
    }
}