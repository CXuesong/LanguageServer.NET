using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace VSCode.JsonRpc
{
    internal class MessageBuffer
    {
        private const string _ContentLength = "Content-Length";
        private const string _HeaderContentSeperator = "\r\n\r\n";
        private const string _HeaderDelimiter = "\r\n";
        private const char _HeaderKeyValueSeperator = ':';

        private List<byte> _buffer;

        internal MessageBuffer()
        {
            _buffer = new List<byte>();

            Encoding = Encoding.UTF8;
        }

        internal MessageBuffer(Encoding encoding)
            : this()
        {
            Encoding = encoding;
        }

        internal Encoding Encoding { get; private set; }

        internal string RawMessage
        {
            get
            {
                return Encoding.GetString(_buffer.ToArray());
            }
        }

        internal bool Valid
        {
            get
            {
                int length = -1;
                int.TryParse(TryReadHeaders()?["Content-Length"], out length);

                string content = TryReadContent();

                if (!string.IsNullOrWhiteSpace(content))
                {
                    return (length == content.Length);
                }

                return false;
            }
        }

        internal void Append(byte[] chunk)
        {
            if (chunk == null)
            {
                return;
            }

            _buffer.AddRange(chunk);
        }

        internal void Append(string chunk)
        {
            if (string.IsNullOrEmpty(chunk))
            {
                return;
            }

            Append(Encoding.GetBytes(chunk));
        }

        internal string TryReadContent(int length)
        {
            if (length < 1)
            {
                throw new ArgumentException("Length must be greater than 0.", "length");
            }

            string raw = _GetBufferAsString();
            string[] parts = raw.Split(new string[] { _HeaderContentSeperator }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length < 2)
            {
                return null;
            }

            string content = string.Join(string.Empty, parts.Skip(1));
            int safeLength = (length > content.Length) ? content.Length : length;

            return content.Substring(0, safeLength); ;
        }

        internal string TryReadContent()
        {
            return TryReadContent(int.MaxValue);
        }

        internal Dictionary<string, string> TryReadHeaders()
        {
            Dictionary<string, string> results = new Dictionary<string, string>();

            string raw = _GetBufferAsString();
            string[] parts = raw.Split(new string[] { _HeaderContentSeperator }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length < 1)
            {
                return null;
            }

            string[] headers = parts[0].Split(new string[] { _HeaderDelimiter }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string header in headers)
            {
                string[] pair = header.Split(_HeaderKeyValueSeperator);

                if (pair.Length < 2)
                {
                    continue;
                }

                results.Add(pair[0], pair[1].Trim());
            }

            return results;
        }

        private string _GetBufferAsString()
        {
            return Encoding.GetString(_buffer.ToArray());
        }
    }
}
