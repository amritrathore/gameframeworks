using System;

namespace Core.SaveSystem
{
    public sealed class JsonSaveProvider : ISaveProvider
    {
        private readonly ISaveSerializer serializer;
        private readonly IStorageBackend storage;

        public JsonSaveProvider(
            ISaveSerializer serializer,
            IStorageBackend storage)
        {
            this.serializer = serializer;
            this.storage = storage;
        }

        public void Save(
            string key,
            object data,
            Type dataType)
        {
            string json = serializer.Serialize(data);

            storage.Write(key, json);
        }

        public object Load(
            string key,
            Type dataType)
        {
            if (!storage.Exists(key))
                return null;

            string json = storage.Read(key);

            return serializer.Deserialize(json, dataType);
        }

        public bool Exists(string key)
        {
            return storage.Exists(key);
        }

        public void Delete(string key)
        {
            storage.Delete(key);
        }

        public void DeleteAll()
        {
            storage.DeleteAll();
        }
    }
}