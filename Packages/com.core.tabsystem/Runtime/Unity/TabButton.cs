using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core.TabSystem.Unity
{
    [RequireComponent(typeof(Button))]
    public sealed class TabButton : MonoBehaviour, ITabInput
    {
        private Button button;
        public event Action SelectionRequested;

        private void OnEnable()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(RequestSelection);
        }

        private void OnDisable()
        {
            if (button != null)
                button.onClick.RemoveListener(RequestSelection);
        }

        private void RequestSelection() => SelectionRequested?.Invoke();
    }
}
