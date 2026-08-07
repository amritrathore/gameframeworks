using System;
using System.Collections.Generic;

namespace Core.SlotSystem
{
    public sealed class SlotCollection
    {
        private readonly List<Slot> slots =
            new List<Slot>();

        public IReadOnlyList<Slot> Slots => slots;

        public int Count => slots.Count;

        public Slot Get(int index)
        {
            ValidateIndex(index);

            return slots[index];
        }

        public void Add(SlotDefinition definition)
        {
            if (definition == null)
                throw new ArgumentNullException(
                    nameof(definition));

            if (definition.Index != slots.Count)
            {
                throw new InvalidOperationException(
                    $"Slot index must be {slots.Count}.");
            }

            Slot slot = new Slot(
                definition.Index,
                definition.UnlockedByDefault,
                definition.UnlockStrategy);

            slots.Add(slot);
        }

        public bool RemoveLast()
        {
            if (slots.Count == 0)
                return false;

            Slot lastSlot = slots[^1];

            if (!lastSlot.IsEmpty)
                return false;

            slots.RemoveAt(slots.Count - 1);

            return true;
        }

        private void ValidateIndex(int index)
        {
            if (index < 0 || index >= slots.Count)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(index));
            }
        }
    }
}