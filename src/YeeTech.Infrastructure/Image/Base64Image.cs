using System;
using System.Linq;

namespace YeeTech.Infrastructure.Image
{
    public class Base64Image
    {
        public string ContentType { get; set; }

        public byte[] FileContents { get; set; }

        public static Base64Image Parse(string base64Content)
        {
            if (string.IsNullOrEmpty(base64Content)) throw new ArgumentNullException(nameof(base64Content));

            var indexOfSemiColon = base64Content.IndexOf(";", StringComparison.OrdinalIgnoreCase);
            var dataLabel = base64Content.Substring(0, indexOfSemiColon);
            var contentType = dataLabel.Split(':').Last();
            var startIndex = base64Content.IndexOf("base64,", StringComparison.OrdinalIgnoreCase) + 7;
            var fileContents = base64Content.Substring(startIndex);
            var bytes = Convert.FromBase64String(fileContents);

            return new Base64Image
            {
                ContentType = contentType,
                FileContents = bytes
            };
        }

        public override string ToString()
        {
            return $"data:{ContentType};base64,{Convert.ToBase64String(FileContents)}";
        }
    }
}