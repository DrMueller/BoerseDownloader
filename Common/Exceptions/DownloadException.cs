using System;
using System.Runtime.Serialization;

namespace MMU.BoerseDownloader.Common.Exceptions
{
    [Serializable]
    public class DownloadException : Exception
    {
        public DownloadException()
        {
        }

        public DownloadException(string message)
            : base(message)
        {
        }

        public DownloadException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected DownloadException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}