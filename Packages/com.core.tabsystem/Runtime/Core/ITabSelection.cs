using System;
using System.Collections.Generic;

namespace Core.TabSystem
{
    public interface ITabSelection<TKey>
    {
        IReadOnlyList<TKey> Keys { get; }
        int SelectedIndex { get; }
        event Action SelectionChanged;
        bool Contains(TKey key);
        bool IsSelected(TKey key);
        bool TrySelect(TKey key);
        bool ClearSelection();
    }
}
