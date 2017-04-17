using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LanguageServer.VsCode.JsonRpc
{
    /// <summary>
    /// Provides timeout functionality for console.
    /// </summary>
    /// <remarks>
    /// The stream get via Console.OpenStandardInput just get stuck if there's no enough user input.
    /// </remarks>
    internal class ConsoleInStream : Stream
    {
        public Stream BaseStream { get; }

        public ConsoleInStream() : this(Console.OpenStandardInput())
        {
            
        }

        internal ConsoleInStream(Stream baseStream)
        {
            if (baseStream == null) throw new ArgumentNullException(nameof(baseStream));
            BaseStream = baseStream;
        }

        /// <inheritdoc />
        public override void Flush()
        {
            BaseStream.Flush();
        }

        /// <inheritdoc />
        public override Task FlushAsync(CancellationToken cancellationToken)
        {
            return base.FlushAsync(cancellationToken);
        }

        /// <inheritdoc />
        public override int Read(byte[] buffer, int offset, int count)
        {
            
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override bool CanRead => true;

        /// <inheritdoc />
        public override bool CanSeek => false;

        /// <inheritdoc />
        public override bool CanWrite { get; }

        /// <inheritdoc />
        public override long Length => throw new NotSupportedException();

        /// <inheritdoc />
        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }
    }
}
