using System;
using Newtonsoft.Json;

namespace Amrit.SaveSystem
{
    public sealed class NewtonsoftSerializer : ISaveSerializer
    {
        private readonly JsonSerializerSettings settings;

        public NewtonsoftSerializer()
        {
            settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.None,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }

        public string Serialize(object data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            return JsonConvert.SerializeObject(
                data,
                settings);
        }

        public object Deserialize(
            string serializedData,
            Type type)
        {
            if (string.IsNullOrWhiteSpace(serializedData))
            {
                return null;
            }

            return JsonConvert.DeserializeObject(
                serializedData,
                type,
                settings);
        }
    }
}