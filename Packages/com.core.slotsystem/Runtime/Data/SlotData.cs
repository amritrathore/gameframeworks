using System;

namespace Core.SlotSystem
{
    [Serializable]
    public sealed class SlotData
    {
        public int Index;

        public bool IsUnlocked;

        public string ItemId;
    }
}