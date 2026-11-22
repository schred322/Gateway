using Newtonsoft.Json;
using System.IO;

namespace Gateway.Providers
{
    public class FileServiceProvider
    {
        public dynamic LoadJsonFile(string filePath)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("The JSON file was not found.", filePath);
            }

            using StreamReader fileStream = File.OpenText(filePath);
            using JsonTextReader jsonReader = new(fileStream);
            return new JsonSerializer().Deserialize<dynamic>(jsonReader)
                ?? throw new JsonException($"The JSON file '{filePath}' is empty or invalid.");
        }
    }
}
