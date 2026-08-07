using UnityEngine;
using UnityEngine.UI;

namespace Core.SlotSystem.Unity
{
    public class SlotView : MonoBehaviour
    {
        [SerializeField]
        private Button button;

        [SerializeField]
        private GameObject lockedView;

        [SerializeField]
        private GameObject emptyView;

        [SerializeField]
        private GameObject occupiedView;

        private Slot boundSlot;

        public int SlotIndex =>
            boundSlot != null
                ? boundSlot.Index
                : -1;

        public Slot BoundSlot =>
            boundSlot;

        public Button Button =>
            button;

        public virtual void Bind(Slot slot)
        {
            if (slot == null)
            {
                Debug.LogError(
                    "Cannot bind null slot.",
                    this);

                return;
            }

            boundSlot = slot;

            Refresh();
        }

        public virtual void Refresh()
        {
            if (boundSlot == null)
                return;

            bool locked =
                boundSlot.State ==
                SlotState.Locked;

            bool empty =
                boundSlot.State ==
                SlotState.Empty;

            bool occupied =
                boundSlot.State ==
                SlotState.Occupied;

            if (lockedView != null)
                lockedView.SetActive(locked);

            if (emptyView != null)
                emptyView.SetActive(empty);

            if (occupiedView != null)
                occupiedView.SetActive(occupied);

            if (button != null)
                button.interactable =
                    !locked;
        }
    }
}