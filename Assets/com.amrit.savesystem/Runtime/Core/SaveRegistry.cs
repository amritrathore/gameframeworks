using System;
using System.Collections.Generic;

namespace Amrit.SaveSystem
{
    public sealed class SaveRegistry
    {
        private readonly Dictionary<string, ISaveable> saveables =
            new();

        public void Register(ISaveable saveable)
        {
            if (saveable == null)
            {
                throw new ArgumentNullException(nameof(saveable));
            }

            saveables[saveable.SaveKey] = saveable;
        }

        public void Unregister(string key)
        {
            saveables.Remove(key);
        }

        public bool TryGet(
            string key,
            out ISaveable saveable)
        {
            return saveables.TryGetValue(
                key,
                out saveable);
        }

        public IEnumerable<ISaveable> GetAll()
        {
            return saveables.Values;
        }

        public void Clear()
        {
            saveables.Clear();
        }
    }
}