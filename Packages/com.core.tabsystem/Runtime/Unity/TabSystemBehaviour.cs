using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.TabSystem.Unity
{
    /// <summary>Inspector composition root. Configuration is fixed after initialization.</summary>
    public sealed class TabSystemBehaviour : MonoBehaviour
    {
        [Serializable]
        private sealed class Entry
        {
            public string key;
            public TabView view;
            public TabButton input;
        }

        [SerializeField] private Entry[] tabs = Array.Empty<Entry>();
        [Tooltip("Empty selects the first tab. An empty tab list has no selection.")]
        [SerializeField] private string initialTab;

        private readonly List<TabBinding<string>> bindings = new List<TabBinding<string>>();
        public ITabSelection<string> Selection { get; private set; }

        private void OnEnable()
        {
            try
            {
                Initialize();
                foreach (Entry tab in tabs)
                    bindings.Add(new TabBinding<string>(Selection, tab.key, tab.view, tab.input));
            }
            catch (Exception exception)
            {
                ReleaseBindings();
                Debug.LogException(exception, this);
                enabled = false;
            }
        }

        public void Initialize()
        {
            if (Selection != null)
                return;

            if (tabs == null)
                throw new InvalidOperationException("Tab entries cannot be null.");

            var keys = new List<string>();
            var views = new HashSet<TabView>();
            var inputs = new HashSet<TabButton>();
            foreach (Entry tab in tabs)
            {
                if (tab == null || string.IsNullOrWhiteSpace(tab.key) || tab.view == null)
                    throw new InvalidOperationException("Each tab requires a nonblank key and a view.");
                if (!views.Add(tab.view) || (tab.input != null && !inputs.Add(tab.input)))
                    throw new InvalidOperationException("Each tab requires its own view and input.");
                keys.Add(tab.key);
            }

            var controller = new TabController<string>(keys);
            if (!string.IsNullOrEmpty(initialTab))
            {
                if (!controller.Contains(initialTab))
                    throw new InvalidOperationException("The initial tab key is not registered.");
                controller.TrySelect(initialTab);
            }
            else if (keys.Count > 0)
            {
                controller.TrySelect(keys[0]);
            }
            Selection = controller;
        }

        // These methods can also be wired to UnityEvents in the Inspector.
        public void SelectTab(string key)
        {
            Initialize();
            Selection.TrySelect(key);
        }

        public void ClearSelection()
        {
            Initialize();
            Selection.ClearSelection();
        }

        private void OnDisable() => ReleaseBindings();

        private void ReleaseBindings()
        {
            foreach (TabBinding<string> binding in bindings)
                binding.Dispose();
            bindings.Clear();
        }
    }
}
