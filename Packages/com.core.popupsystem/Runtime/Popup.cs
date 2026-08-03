using UnityEngine;
using UnityEngine.UI;

namespace Core.PopupSystem
{
    public abstract class Popup : MonoBehaviour, IPopup
    {
        public string Name => this.GetType().Name;
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
    }
}