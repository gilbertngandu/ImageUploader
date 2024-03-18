using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ImageUploader.Common.Extension
{
    public static class ConfigurationSectionExtension
    {
        public static string AsJsonString(this IConfigurationSection section)
        {
            // Create a dictionary to hold the key-value pairs
            var values = new Dictionary<string, string>();

            // Iterate over child key-value pairs and add them to the dictionary
            foreach (var child in section.GetChildren())
            {
                values[child.Key] = child.Value;
            }

            return JsonConvert.SerializeObject(values);
        }
    }
}
