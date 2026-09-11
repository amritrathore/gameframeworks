using System;
using System.Collections.Generic;

namespace Core.TabSystem
{
    /// <summary>Owns exclusive selection for a fixed, ordered set of keys.</summary>
    public sealed class TabController<TKey> : ITabSelection<TKey>
    {
        private readonly Dictionary<TKey, int> indices;
        private readonly ITabSelectionPolicy<TKey> policy;
        private bool changing;

        public IReadOnlyList<TKey> Keys { get; }
        public int SelectedIndex { get; private set; } = -1;
        public event Action SelectionChanged;

        public TabController(IEnumerable<TKey> keys,
            ITabSelectionPolicy<TKey> policy = null,
            IEqualityComparer<TKey> comparer = null)
        {
            if (keys == null)
                throw new ArgumentNullException(nameof(keys));

            this.policy = policy;
            indices = new Dictionary<TKey, int>(comparer);
            var ordered = new List<TKey>();
            foreach (TKey key in keys)
            {
                if (key is null)
                    throw new ArgumentException("Tab keys cannot be null.", nameof(keys));
                if (indices.ContainsKey(key))
                    throw new ArgumentException("Tab keys must be unique.", nameof(keys));

                indices.Add(key, ordered.Count);
                ordered.Add(key);
            }
            Keys = ordered.AsReadOnly();
        }

        public bool Contains(TKey key) => !(key is null) && indices.ContainsKey(key);

        public bool IsSelected(TKey key) =>
            !(key is null) && indices.TryGetValue(key, out int index) && index == SelectedIndex;

        /// <summary>Returns true only when selection changes. Nested changes are rejected.</summary>
        public bool TrySelect(TKey key)
        {
            if (changing || key is null || !indices.TryGetValue(key, out int index)
                || index == SelectedIndex)
                return false;

            changing = true;
            try
            {
                if (policy != null && !policy.CanSelect(key))
                    return false;

                SelectedIndex = index;
                SelectionChanged?.Invoke();
                return true;
            }
            finally
            {
                changing = false;
            }
        }

        public bool ClearSelection()
        {
            if (changing || SelectedIndex < 0)
                return false;

            changing = true;
            try
            {
                SelectedIndex = -1;
                SelectionChanged?.Invoke();
                return true;
            }
            finally
            {
                changing = false;
            }
        }
    }
}
