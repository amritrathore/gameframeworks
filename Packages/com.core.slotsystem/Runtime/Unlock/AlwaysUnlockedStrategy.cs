namespace Core.SlotSystem
{
    public sealed class AlwaysUnlockedStrategy
        : ISlotUnlockStrategy
    {
        public bool CanUnlock(Slot slot)
        {
            return true;
        }

        public bool TryUnlock(Slot slot)
        {
            return slot.MarkUnlocked();
        }
    }
}