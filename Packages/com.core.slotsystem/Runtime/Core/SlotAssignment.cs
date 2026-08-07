namespace Core.SlotSystem
{
    public readonly struct SlotAssignment
    {
        public int SlotIndex { get; }

        public string ItemId { get; }

        public SlotAssignment(
            int slotIndex,
            string itemId)
        {
            SlotIndex = slotIndex;
            ItemId = itemId;
        }
    }
}