using System;

namespace Core.TabSystem
{
    /// <summary>Connects one key to a view and optional input without owning either.</summary>
    public sealed class TabBinding<TKey> : IDisposable
    {
        private readonly ITabSelection<TKey> selection;
        private readonly TKey key;
        private readonly ITabView view;
        private readonly ITabInput input;
        private bool disposed;

        public TabBinding(ITabSelection<TKey> selection, TKey key, ITabView view,
            ITabInput input = null)
        {
            this.selection = selection ?? throw new ArgumentNullException(nameof(selection));
            this.view = view ?? throw new ArgumentNullException(nameof(view));
            if (!selection.Contains(key))
                throw new ArgumentException("The key is not registered with the selection.", nameof(key));
            this.key = key;
            this.input = input;

            Refresh();
            selection.SelectionChanged += Refresh;
            if (input != null)
                input.SelectionRequested += Select;
        }

        private void Select()
        {
            if (!disposed)
                selection.TrySelect(key);
        }

        private void Refresh()
        {
            if (!disposed)
                view.SetSelected(selection.IsSelected(key));
        }

        public void Dispose()
        {
            if (disposed)
                return;

            disposed = true;
            selection.SelectionChanged -= Refresh;
            if (input != null)
                input.SelectionRequested -= Select;
        }
    }
}
