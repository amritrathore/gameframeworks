using System;

namespace Amrit.SaveSystem
{
    public sealed class BinarySaveProvider : ISaveProvider
    {
        private readonly ISaveSerializer serializer;
        private readonly IStorageBackend storage;

        public BinarySaveProvider(
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
            string binary = serializer.Serialize(data);

            storage.Write(key, binary);
        }

        public object Load(
            string key,
            Type dataType)
        {
            if (!storage.Exists(key))
                return null;

            string binary = storage.Read(key);

            return serializer.Deserialize(binary, dataType);
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