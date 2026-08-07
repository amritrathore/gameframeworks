using UnityEngine;

namespace Core.SlotSystem.Unity
{
    [CreateAssetMenu(
        fileName = "SlotConfiguration",
        menuName = "Core/Slot System/Slot Configuration")]
    public sealed class SlotConfiguration : ScriptableObject
    {
        [Header("Slot Configuration")]
        [SerializeField]
        [Min(1)]
        private int slotCount = 3;

        [SerializeField]
        [Min(0)]
        private int initiallyUnlocked = 3;

        public int SlotCount => slotCount;

        public int InitiallyUnlocked =>
            initiallyUnlocked;

        private void OnValidate()
        {
            slotCount = Mathf.Max(1, slotCount);

            initiallyUnlocked =
                Mathf.Clamp(
                    initiallyUnlocked,
                    0,
                    slotCount);
        }
    }
}