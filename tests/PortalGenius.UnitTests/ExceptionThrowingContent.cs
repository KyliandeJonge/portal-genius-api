using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace PortalGenius.UnitTests
{
    public class ExceptionThrowingContent : HttpContent
    {
        private readonly Exception exception;
        public ExceptionThrowingContent(Exception exception)
        {
            this.exception = exception;
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            return Task.FromException(exception);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = 0L;
            return false;
        }
    }
}
