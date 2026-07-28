using System;

namespace Core.SaveSystem
{
    [Serializable]
    public sealed class SaveFile
    {
        public SaveMetadata Metadata;

        public object Data;
    }
}