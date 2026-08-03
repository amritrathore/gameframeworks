using UnityEngine;

namespace Core.PopupSystem
{
    public interface IPopup
    {
        string Name { get; }
        void Show(object data, int layer);
        void Hide();
    }
}