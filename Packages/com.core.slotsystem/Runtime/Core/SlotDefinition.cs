namespace Core.SlotSystem
{
    public sealed class SlotDefinition
    {
        public int Index { get; }

        public bool UnlockedByDefault { get; }

        public ISlotUnlockStrategy UnlockStrategy { get; }

        public SlotDefinition(
            int index,
            bool unlockedByDefault,
            ISlotUnlockStrategy unlockStrategy = null)
        {
            if (index < 0)
                throw new System.ArgumentOutOfRangeException(
                    nameof(index));

            Index = index;
            UnlockedByDefault = unlockedByDefault;
            UnlockStrategy = unlockStrategy;
        }
    }
}