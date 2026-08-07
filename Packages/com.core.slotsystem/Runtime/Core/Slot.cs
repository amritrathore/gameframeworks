
using System;

namespace Core.SlotSystem
{
    public sealed class Slot
    {
        private string itemId;
        private bool isUnlocked;

        private readonly ISlotUnlockStrategy unlockStrategy;

        public int Index { get; }

        public bool IsUnlocked => isUnlocked;

        public string ItemId => itemId;

        public bool IsEmpty =>
            string.IsNullOrEmpty(itemId);

        public SlotState State
        {
            get
            {
                if (!IsUnlocked)
                    return SlotState.Locked;

                return IsEmpty
                    ? SlotState.Empty
                    : SlotState.Occupied;
            }
        }

        public Slot(
            int index,
            bool unlocked,
            ISlotUnlockStrategy unlockStrategy = null)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(index));

            Index = index;
            isUnlocked = unlocked;
            this.unlockStrategy = unlockStrategy;
        }

        public bool Assign(string itemId)
        {
            if (!IsUnlocked)
                return false;

            if (string.IsNullOrWhiteSpace(itemId))
                return false;

            this.itemId = itemId;

            return true;
        }

        public bool Clear()
        {
            if (!IsUnlocked)
                return false;

            itemId = null;

            return true;
        }

        public bool CanUnlock()
        {
            if (IsUnlocked)
                return true;

            return unlockStrategy != null &&
                   unlockStrategy.CanUnlock(this);
        }

        public bool TryUnlock()
        {
            if (IsUnlocked)
                return true;

            if (unlockStrategy == null)
                return false;

            return unlockStrategy.TryUnlock(this);
        }

        internal bool MarkUnlocked()
        {
            if (isUnlocked)
                return false;

            isUnlocked = true;

            return true;
        }

        internal void Restore(
            bool unlocked,
            string itemId)
        {
            isUnlocked = unlocked;

            this.itemId = unlocked
                ? itemId
                : null;
        }
    }
}