namespace Core.SlotSystem
{
    public interface ISlotUnlockStrategy
    {
        bool CanUnlock(Slot slot);

        bool TryUnlock(Slot slot);
    }
}