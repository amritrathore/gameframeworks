using System;
using Core.SaveSystem;

namespace Core.SlotSystem
{
    public sealed class SlotManager : ISaveable
    {
        private readonly SlotCollection slots;

        private int selectedSlotIndex = -1;

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

        public string SaveKey => "SlotSystem";

        public Type StateType => typeof(SlotSaveData);

        public SlotManager(
            SlotCollection slots)
        {
            this.slots =
                slots ??
                throw new ArgumentNullException(
                    nameof(slots));
        }

        public bool Assign(
            int slotIndex,
            string itemId)
        {
            Slot slot = slots.Get(slotIndex);

            if (!slot.Assign(itemId))
                return false;

            SlotChanged?.Invoke(slot);

            return true;
        }

        public bool Clear(int slotIndex)
        {
            Slot slot = slots.Get(slotIndex);

            if (!slot.Clear())
                return false;

            SlotChanged?.Invoke(slot);

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

            return true;
        }

        public bool Select(int slotIndex)
        {
            Slot slot = slots.Get(slotIndex);

            if (!slot.IsUnlocked)
                return false;

            selectedSlotIndex = slotIndex;

            SlotSelected?.Invoke(slotIndex);

            return true;
        }

        public bool AddSlot(
            SlotDefinition definition)
        {
            slots.Add(definition);

            Slot slot = slots.Get(slots.Count - 1);

            SlotAdded?.Invoke(slot);

            return true;
        }

        public bool RemoveLastSlot()
        {
            if (selectedSlotIndex == slots.Count - 1)
                selectedSlotIndex = -1;

            if (!slots.RemoveLast())
                return false;

            SlotRemoved?.Invoke(slots.Count);

            return true;
        }

        public object CaptureState()
        {
            SlotSaveData data =
                SlotSaveData.Create(
                    slots,
                    selectedSlotIndex);

            return data;
        }

        public void RestoreState(object state)
        {
            SlotSaveData data = (SlotSaveData)state;

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