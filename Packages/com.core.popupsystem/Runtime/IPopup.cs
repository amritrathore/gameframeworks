using System;

namespace Core.PopupSystem
{
    public interface IPopup
    {
        Type Name { get; }
        void Init(PopupController controller);
        void Show(object data, int layer);
        void Hide();
        void Refresh();
    }
}