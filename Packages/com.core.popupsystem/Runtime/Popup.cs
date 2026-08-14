using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core.PopupSystem
{
    public abstract class Popup : MonoBehaviour, IPopup
    {
        public Type Name => this.GetType();
        [SerializeField] protected Canvas canvas;
        [SerializeField] protected CanvasGroup canvasGroup;
        [SerializeField] protected Button closeButton;
        protected PopupController popupController;

        public virtual void Init(PopupController controller)
        {
            popupController = controller;
        }

        public virtual void Show(object data, int layer)
        {
            SetActive(true, layer);
            closeButton.onClick.AddListener(OnClose);
        }

        public virtual void Hide()
        {
            closeButton.onClick.RemoveListener(OnClose);
            SetActive(false);
        }

        private void SetActive(bool active, int layer = 0)
        {
            gameObject.SetActive(active);
            canvas.enabled = active;
            canvasGroup.alpha = active ? 1 : 0;
            canvasGroup.blocksRaycasts = active;
            canvas.sortingOrder = layer;
        }

        public virtual void Refresh() { }

        protected virtual void OnClose()
        {
            popupController.Hide();
        }
    }
}