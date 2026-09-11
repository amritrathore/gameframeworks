using System;
using NUnit.Framework;

namespace Core.TabSystem.Tests
{
    public sealed class TabSystemTests
    {
        private sealed class Policy : ITabSelectionPolicy<string>
        {
            public bool Allowed;
            public bool CanSelect(string key) => key != "locked" || Allowed;
        }

        private sealed class Input : ITabInput
        {
            public event Action SelectionRequested;
            public void Click() => SelectionRequested?.Invoke();
        }

        private sealed class View : ITabView
        {
            public bool Selected;
            public int Updates;
            public void SetSelected(bool selected)
            {
                Selected = selected;
                Updates++;
            }
        }

        [Test]
        public void SelectionIsExclusiveAndNotifiesAfterStateChanges()
        {
            var controller = new TabController<int>(new[] { 0, 1 });
            int notifications = 0;
            controller.SelectionChanged += () =>
            {
                notifications++;
                Assert.That(controller.IsSelected(controller.Keys[controller.SelectedIndex]), Is.True);
            };
            Assert.That(controller.SelectedIndex, Is.EqualTo(-1));
            Assert.That(controller.TrySelect(0), Is.True);
            Assert.That(controller.TrySelect(1), Is.True);
            Assert.That(controller.IsSelected(0), Is.False);
            Assert.That(notifications, Is.EqualTo(2));
        }

        [Test]
        public void InvalidAndRepeatedSelectionsDoNotNotify()
        {
            var controller = new TabController<string>(new[] { "a" });
            controller.TrySelect("a");
            int notifications = 0;
            controller.SelectionChanged += () => notifications++;
            Assert.That(controller.TrySelect("a"), Is.False);
            Assert.That(controller.TrySelect("missing"), Is.False);
            Assert.That(controller.TrySelect(null), Is.False);
            Assert.That(controller.Contains(null), Is.False);
            Assert.That(notifications, Is.Zero);
            Assert.That(controller.IsSelected("a"), Is.True);
        }

        [Test]
        public void PolicyCanChangeWithoutReplacingController()
        {
            var policy = new Policy();
            var controller = new TabController<string>(new[] { "open", "locked" }, policy);
            controller.TrySelect("open");
            Assert.That(controller.TrySelect("locked"), Is.False);
            Assert.That(controller.IsSelected("open"), Is.True);
            policy.Allowed = true;
            Assert.That(controller.TrySelect("locked"), Is.True);
        }

        [Test]
        public void ClearSelectionNotifiesOnlyOnce()
        {
            var controller = new TabController<int>(new[] { 0 });
            controller.TrySelect(0);
            int notifications = 0;
            controller.SelectionChanged += () => notifications++;
            Assert.That(controller.ClearSelection(), Is.True);
            Assert.That(controller.SelectedIndex, Is.EqualTo(-1));
            Assert.That(controller.IsSelected(0), Is.False);
            Assert.That(controller.ClearSelection(), Is.False);
            Assert.That(notifications, Is.EqualTo(1));
        }

        [Test]
        public void EmptyControllerIsValid()
        {
            var controller = new TabController<int>(Array.Empty<int>());
            Assert.That(controller.Keys, Is.Empty);
            Assert.That(controller.TrySelect(0), Is.False);
            Assert.That(controller.ClearSelection(), Is.False);
        }

        [Test]
        public void KeysAreValidatedAndCopied()
        {
            Assert.Throws<ArgumentNullException>(() => new TabController<string>(null));
            Assert.Throws<ArgumentException>(() => new TabController<string>(new[] { "a", null }));
            Assert.Throws<ArgumentException>(() => new TabController<int>(new[] { 1, 1 }));
            var keys = new[] { "a" };
            var controller = new TabController<string>(keys);
            keys[0] = "changed";
            Assert.That(controller.Keys[0], Is.EqualTo("a"));
        }

        [Test]
        public void CustomComparerAppliesToLookupAndUniqueness()
        {
            var controller = new TabController<string>(new[] { "Inventory" },
                comparer: StringComparer.OrdinalIgnoreCase);
            Assert.That(controller.TrySelect("INVENTORY"), Is.True);
            Assert.That(controller.IsSelected("inventory"), Is.True);
            Assert.Throws<ArgumentException>(() => new TabController<string>(new[] { "a", "A" },
                comparer: StringComparer.OrdinalIgnoreCase));
        }

        [Test]
        public void ReentrantChangesAreRejected()
        {
            var controller = new TabController<int>(new[] { 1, 2 });
            controller.SelectionChanged += () =>
            {
                Assert.That(controller.TrySelect(2), Is.False);
                Assert.That(controller.ClearSelection(), Is.False);
            };
            Assert.That(controller.TrySelect(1), Is.True);
            Assert.That(controller.IsSelected(1), Is.True);
        }

        [Test]
        public void ListenerExceptionDoesNotLeaveControllerLocked()
        {
            var controller = new TabController<int>(new[] { 1, 2 });
            Action listener = () => throw new InvalidOperationException();
            controller.SelectionChanged += listener;
            Assert.Throws<InvalidOperationException>(() => controller.TrySelect(1));
            controller.SelectionChanged -= listener;
            Assert.That(controller.TrySelect(2), Is.True);
        }

        [Test]
        public void BindingsSynchronizeViewsFromInputAndClear()
        {
            var controller = new TabController<int>(new[] { 1, 2 });
            var first = new View();
            var second = new View();
            var input = new Input();
            controller.TrySelect(1);
            using (new TabBinding<int>(controller, 1, first))
            using (new TabBinding<int>(controller, 2, second, input))
            {
                Assert.That(first.Selected, Is.True);
                Assert.That(second.Selected, Is.False);
                input.Click();
                Assert.That(first.Selected, Is.False);
                Assert.That(second.Selected, Is.True);
                controller.ClearSelection();
                Assert.That(second.Selected, Is.False);
            }
        }

        [Test]
        public void DisposalUnsubscribesBothDirectionsAndRebindingRestoresView()
        {
            var controller = new TabController<int>(new[] { 1 });
            var view = new View();
            var input = new Input();
            var binding = new TabBinding<int>(controller, 1, view, input);
            binding.Dispose();
            binding.Dispose();
            input.Click();
            Assert.That(controller.SelectedIndex, Is.EqualTo(-1));
            controller.TrySelect(1);
            Assert.That(view.Updates, Is.EqualTo(1));
            using (new TabBinding<int>(controller, 1, view, input))
                Assert.That(view.Selected, Is.True);
        }

        [Test]
        public void InvalidBindingsFailBeforeSubscribing()
        {
            var controller = new TabController<int>(new[] { 1 });
            var view = new View();
            Assert.Throws<ArgumentNullException>(() => new TabBinding<int>(null, 1, view));
            Assert.Throws<ArgumentNullException>(() => new TabBinding<int>(controller, 1, null));
            Assert.Throws<ArgumentException>(() => new TabBinding<int>(controller, 2, view));
            Assert.That(view.Updates, Is.Zero);
        }
    }
}
