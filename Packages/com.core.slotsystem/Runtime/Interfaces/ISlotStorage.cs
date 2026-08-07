namespace Core.SlotSystem
{
    public interface ISlotStorage
    {
        SlotSaveData Load();

        void Save(SlotSaveData data);
    }
}