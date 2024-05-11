using System;

namespace TipItService.Domain
{
    public class SourceUrl
    {
        public string Value { get; }
        public SourceUrl(
            string url)
        {
            if (!url.StartsWith("http"))
                throw new ArgumentException(
                    "protocol missing");
            Value = url;
        }
    }
}
