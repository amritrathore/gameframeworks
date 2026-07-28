using System;

namespace Amrit.SaveSystem
{
    [Serializable]
    public sealed class SaveFile
    {
        public SaveMetadata Metadata;

        public object Data;
    }
}