using System;

namespace Core.SaveSystem
{
    [Serializable]
    public sealed class SaveMetadata
    {
        public int Version = 1;

        public string ApplicationVersion;

        public string SaveDateUtc;

        public string Slot;

        public string Platform;
    }
}