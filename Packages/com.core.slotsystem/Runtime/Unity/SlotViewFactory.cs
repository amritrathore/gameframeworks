using UnityEngine;

namespace Core.SlotSystem.Unity
{
    public sealed class SlotViewFactory : MonoBehaviour
    {
        [SerializeField]
        private SlotSystemBehaviour slotSystem;

        [SerializeField]
        private SlotView slotPrefab;

        [SerializeField]
        private Transform container;

        private SlotView[] views;

        private void Start()
        {
            CreateViews();
        }

        public void CreateViews()
        {
            if (slotSystem == null)
            {
                Debug.LogError(
                    "SlotSystemBehaviour is not assigned.",
                    this);

                return;
            }

            if (slotPrefab == null)
            {
                Debug.LogError(
                    "Slot prefab is not assigned.",
                    this);

                return;
            }

            if (container == null)
            {
                Debug.LogError(
                    "Slot container is not assigned.",
                    this);

                return;
            }

            ClearViews();

            SlotCollection slots =
                slotSystem.Manager.Slots;

            views =
                new SlotView[slots.Count];

            for (int i = 0;
                 i < slots.Count;
                 i++)
            {
                Slot slot =
                    slots.Get(i);

                SlotView view =
                    Instantiate(
                        slotPrefab,
                        container);

                view.Bind(slot);

                views[i] = view;
            }
        }

        private void ClearViews()
        {
            if (container == null)
                return;

            for (int i = container.childCount - 1;
                 i >= 0;
                 i--)
            {
                Destroy(
                    container.GetChild(i).gameObject);
            }

            views = null;
        }

        public SlotView GetView(int index)
        {
            if (views == null)
                return null;

            if (index < 0 ||
                index >= views.Length)
            {
                return null;
            }

            return views[index];
        }
    }
}