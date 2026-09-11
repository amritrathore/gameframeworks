using UnityEngine;
using UnityEngine.Events;

namespace Core.TabSystem.Unity
{
    public sealed class TabView : MonoBehaviour, ITabView
    {
        [SerializeField] private GameObject content;
        [SerializeField] private GameObject selectedIndicator;
        [SerializeField] private UnityEvent<bool> selectionChanged = new UnityEvent<bool>();

        public bool IsSelected { get; private set; }
        private bool initialized;

        public void SetSelected(bool selected)
        {
            bool changed = !initialized || IsSelected != selected;
            initialized = true;
            IsSelected = selected;
            if (content != null)
                content.SetActive(selected);
            if (selectedIndicator != null)
                selectedIndicator.SetActive(selected);
            if (changed)
                selectionChanged.Invoke(selected);
        }
    }
}
