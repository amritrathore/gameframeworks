using System;
using UnityEngine;

namespace Core.SlotSystem.Unity
{
    public sealed class SlotSystemBehaviour : MonoBehaviour
    {
        [SerializeField]
        private SlotConfiguration configuration;

        private SlotManager slotManager;

        public SlotManager Manager =>
            slotManager;

        public bool IsInitialized =>
            slotManager != null;

        public event Action Initialized;

        private void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            if (IsInitialized)
                return;

            if (configuration == null)
            {
                Debug.LogError(
                    "SlotConfiguration is not assigned.",
                    this);

                return;
            }

            SlotCollection collection =
                CreateSlotCollection();

            slotManager =
                new SlotManager(collection);

            Initialized?.Invoke();
        }

        private SlotCollection CreateSlotCollection()
        {
            SlotCollection collection =
                new SlotCollection();

            for (int i = 0;
                 i < configuration.SlotCount;
                 i++)
            {
                bool unlocked =
                    i < configuration.InitiallyUnlocked;

                collection.Add(
                    new SlotDefinition(
                        i,
                        unlocked));
            }

            return collection;
        }
    }
}