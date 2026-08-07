using System;

namespace Core.SlotSystem
{
    [Serializable]
    public sealed class SlotSaveData
    {
        public int SelectedSlotIndex = -1;

        public List<SlotData> Slots =
            new List<SlotData>();

        public static SlotSaveData Create(
            SlotCollection slotCollection,
            int selectedSlotIndex)
        {
            SlotSaveData data =
                new SlotSaveData
                {
                    SelectedSlotIndex =
                        selectedSlotIndex
                };

            foreach (Slot slot in slotCollection.Slots)
            {
                data.Slots.Add(
                    new SlotData
                    {
                        Index = slot.Index,
                        IsUnlocked =
                            slot.IsUnlocked,
                        ItemId =
                            slot.ItemId
                    });
            }

            return data;
        }
    }
}