using System;

namespace Core.SaveSystem
{
    public sealed class CloudSaveProvider : ISaveProvider
    {
        public void Save(
            string key,
            object data,
            Type dataType)
        {
            throw new NotImplementedException();
        }

        public object Load(
            string key,
            Type dataType)
        {
            throw new NotImplementedException();
        }

        public bool Exists(string key)
        {
            throw new NotImplementedException();
        }

        public void Delete(string key)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll()
        {
            throw new NotImplementedException();
        }
    }
}