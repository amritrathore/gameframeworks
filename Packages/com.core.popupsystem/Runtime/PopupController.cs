using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.PopupSystem
{
    public class PopupController : MonoBehaviour
    {
        [SerializeField] private Popup[] popups;
        private Dictionary<Type, IPopup> popupKeyvaluepairs = new();
        private readonly Stack<IPopup> popupStack = new();
        private IPopup currentPopup;


        public bool IsInitialized { get; private set; } = false;

        public void Init()
        {
            if (IsInitialized)
                return;

            if (popups.Length == 0)
            {
                Debug.LogError("Empty popup list");
                return;
            }

            IsInitialized = true;

            foreach (Popup popup in popups)
            {
                if (popupKeyvaluepairs.TryAdd(popup.Name, popup))
                {
                    popup.Init(this);
                    popup.Hide();
                }
                else
                    Debug.LogError($"Duplicate popup name: {popup.Name}");
            }
        }


        public virtual void ShowPopup<T>(object data, bool stacked = false) where T : class, IPopup
        {
            Init();

            if (stacked)
            {
                // popup stack true
                if (currentPopup != null)
                    popupStack.Push(currentPopup);

                if (!popupKeyvaluepairs.TryGetValue(typeof(T), out currentPopup))
                    Debug.LogError($"Popup '{nameof(T)}' not registered.");

                currentPopup.Show(data, popupStack.Count + 1);
            }
            else
            {
                // popup stack false
                Hide();

                if (!popupKeyvaluepairs.TryGetValue(typeof(T), out currentPopup))
                    Debug.LogError($"Popup '{nameof(T)}' not registered.");

                currentPopup.Show(data, 1);
            }
        }

        public virtual void Hide()
        {
            if (currentPopup == null)
            {
                Debug.Log("current popup is null");
                return;
            }
            currentPopup.Hide();

            if (popupStack.Count > 0)
            {
                currentPopup = popupStack.Pop();
                currentPopup?.Refresh();
            }
        }

        public virtual void HideAll()
        {
            while (popupStack.Count > 0)
            {
                IPopup popup = popupStack.Pop();
                popup.Hide();
            }

            popupStack.Clear();
        }
    }
}