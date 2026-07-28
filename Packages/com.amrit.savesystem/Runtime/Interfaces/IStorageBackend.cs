using System;

namespace Core.SaveSystem
{
    public interface IStorageBackend
    {
        void Write(string key, string content);

        string Read(string key);

        bool Exists(string key);

        void Delete(string key);

        void DeleteAll();
    }
}