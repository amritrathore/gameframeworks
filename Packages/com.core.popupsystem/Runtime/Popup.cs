using System;
using UnityEngine;

namespace Core.PopupSystem
{
    public abstract class Popup : MonoBehaviour, IPopup
    {
        public Type Name => this.GetType();
        [SerializeField] protected Canvas canvas;
        [SerializeField] protected CanvasGroup canvasGroup;

        public virtual void Show(object data, int layer)
        {
            SetActive(true);
        }

        public virtual void Hide()
        {
            SetActive(false);
        }

        private void SetActive(bool active)
        {
            gameObject.SetActive(active);
            canvas.enabled = active;
            canvasGroup.alpha = active ? 1 : 0;
            canvasGroup.blocksRaycasts = active;
        }

        public virtual void Refresh() { }
    }
}