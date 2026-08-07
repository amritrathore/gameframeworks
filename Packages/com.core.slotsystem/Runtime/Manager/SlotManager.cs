using System;

namespace Core.SlotSystem
{
    public sealed class SlotManager
    {
        private readonly SlotCollection slots;

        private int selectedSlotIndex = -1;

        private readonly ISlotStorage storage;

        public event Action<Slot> SlotChanged;

        public event Action<Slot> SlotUnlocked;

        public event Action<int> SlotSelected;

        public event Action<Slot> SlotAdded;

        public event Action<int> SlotRemoved;

        public SlotCollection Slots => slots;

        public int SelectedSlotIndex =>
            selectedSlotIndex;

        public Slot SelectedSlot
        {
            get
            {
                if (selectedSlotIndex < 0)
                    return null;

                return slots.Get(selectedSlotIndex);
            }
        }

        public SlotManager(
            SlotCollection slots,
            ISlotStorage storage = null)
        {
            this.slots =
                slots ??
                throw new ArgumentNullException(
                    nameof(slots));

            this.storage = storage;
        }

        public bool Assign(
            int slotIndex,
            string itemId)
        {
            Slot slot = slots.Get(slotIndex);

            if (!slot.Assign(itemId))
                return false;

            SlotChanged?.Invoke(slot);

            Save();

            return true;
        }

        public bool Clear(int slotIndex)
        {
            Slot slot = slots.Get(slotIndex);

            if (!slot.Clear())
                return false;

            SlotChanged?.Invoke(slot);

            Save();

            return true;
        }

        public bool CanUnlock(int slotIndex)
        {
            Slot slot = slots.Get(slotIndex);

            return slot.CanUnlock();
        }

        public bool Unlock(int slotIndex)
        {
            Slot slot = slots.Get(slotIndex);

            if (!slot.TryUnlock())
                return false;

            SlotUnlocked?.Invoke(slot);
            SlotChanged?.Invoke(slot);

            Save();

            return true;
        }

        public bool Select(int slotIndex)
        {
            Slot slot = slots.Get(slotIndex);

            if (!slot.IsUnlocked)
                return false;

            selectedSlotIndex = slotIndex;

            SlotSelected?.Invoke(slotIndex);

            Save();

            return true;
        }

        public bool AddSlot(
            SlotDefinition definition)
        {
            slots.Add(definition);

            Slot slot = slots.Get(slots.Count - 1);

            SlotAdded?.Invoke(slot);

            Save();

            return true;
        }

        public bool RemoveLastSlot()
        {
            if (selectedSlotIndex == slots.Count - 1)
                selectedSlotIndex = -1;

            if (!slots.RemoveLast())
                return false;

            SlotRemoved?.Invoke(slots.Count);

            Save();

            return true;
        }

        public void Save()
        {
            if (storage == null)
                return;

            SlotSaveData data =
                SlotSaveData.Create(
                    slots,
                    selectedSlotIndex);

            storage.Save(data);
        }

        public void Load()
        {
            if (storage == null)
                return;

            SlotSaveData data =
                storage.Load();

            if (data == null)
                return;

            Restore(data);
        }

        public void Restore(
            SlotSaveData data)
        {
            if (data == null)
                return;

            foreach (SlotData slotData in data.Slots)
            {
                if (slotData.Index < 0 ||
                    slotData.Index >= slots.Count)
                {
                    continue;
                }

                Slot slot =
                    slots.Get(slotData.Index);

                slot.Restore(
                    slotData.IsUnlocked,
                    slotData.ItemId);

                SlotChanged?.Invoke(slot);
            }

            if (data.SelectedSlotIndex >= 0 &&
                data.SelectedSlotIndex < slots.Count)
            {
                Slot selected =
                    slots.Get(
                        data.SelectedSlotIndex);

                if (selected.IsUnlocked)
                {
                    selectedSlotIndex =
                        data.SelectedSlotIndex;

                    SlotSelected?.Invoke(
                        selectedSlotIndex);
                }
            }
        }
    }
}