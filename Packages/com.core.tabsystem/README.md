# Tab System

A generic single-selection tab package for Unity 2022.3+. The core has no Unity dependencies and supports strings, enums, integers, or custom key types. This repository includes the package as an embedded package under `Packages/com.core.tabsystem`.

## Inspector setup (uGUI)

1. Add `TabSystemBehaviour` to a persistent object under your Canvas.
2. Add `TabButton` to each uGUI Button. It automatically uses the Button on that object.
3. Add `TabView` for each tab. Assign its content panel and optional selected indicator. The optional boolean `Selection Changed` event can drive extra visuals or animations.
4. Populate the system's `Tabs` array with a unique nonblank key, a view, and an optional input for each tab.
5. Set `Initial Tab` to a registered key, or leave it empty to select the first tab.

Keep the system and tab buttons outside panels/indicators that views deactivate. Use distinct content panels and indicators per tab; do not point them at the group, buttons, or their ancestors. A view toggles its assigned GameObjects immediately. Use a custom `ITabView` for animated transitions or lazy content creation.

Call `SelectTab(string)` and `ClearSelection()` from scripts or UnityEvents. `Selection` is available after `Initialize()` (called automatically on enable). Disabling the system disconnects bindings but preserves selection and current visuals. Re-enabling reconnects inputs and refreshes all views. Inspector configuration is fixed after initialization. Empty groups are supported; invalid configuration logs an exception and disables the component.

## Code composition

```csharp
using Core.TabSystem;

public enum MenuTab { Inventory, Skills, Settings }

// In your composition root; inventoryView and inventoryInput implement the small interfaces.
var tabs = new TabController<MenuTab>(new[] {
    MenuTab.Inventory, MenuTab.Skills, MenuTab.Settings
});
var binding = new TabBinding<MenuTab>(tabs, MenuTab.Inventory,
    inventoryView, inventoryInput);
tabs.TrySelect(MenuTab.Inventory);

// Dispose every binding when its owner is disabled/destroyed.
binding.Dispose();
```

Create one binding for each view. Input is optional, and several bindings may observe the same key. Implement `ITabInput` for keyboard, UI Toolkit, or another input source, and `ITabView` for your presentation. Consumers reference the `Core.TabSystem` assembly; uGUI integration additionally references `Core.TabSystem.Unity`.

For access rules, inject an `ITabSelectionPolicy<TKey>` into the controller. The policy is evaluated on each attempt to select a different registered tab. A denied attempt preserves the previous selection. Changes to policy state do not automatically deselect a tab; call `ClearSelection()` if needed. The Inspector wrapper uses unrestricted selection; compose a controller and bindings in code to supply a policy or custom key comparer.

## Responsibilities and contracts

- `TabController<TKey>` owns ordered, immutable keys and exclusive selection. It starts with no selection (`SelectedIndex == -1`). Keys must be unique and non-null, with stable equality/hash codes.
- `ITabSelection<TKey>` exposes selection independently of its implementation. `ITabSelectionPolicy<TKey>` adds access rules through composition.
- `TabBinding<TKey>` connects selection, an `ITabView`, and an optional `ITabInput`. Disposal is idempotent and removes only its own subscriptions.
- `TabButton`, `TabView`, and `TabSystemBehaviour` handle uGUI input, GameObject presentation, and Inspector composition respectively.

`TrySelect` returns true only for an actual change. Null, unknown, denied, repeated, and reentrant selections return false. `ClearSelection` returns true only when it clears a selection. `SelectionChanged` fires synchronously after state is committed; inspect `SelectedIndex` and `Keys` inside the callback. Reentrant changes during policy evaluation or notification are rejected. Use on one thread (Unity's main thread for Unity views). Policies, views, and listeners should not throw: exceptions propagate, and a notification exception does not roll back committed selection or guarantee delivery to remaining listeners.

## Tests

Open **Window > General > Test Runner**, select **EditMode**, and run `Core.TabSystem.Tests`. Tests cover exclusive selection, invalid keys, access policies, custom comparers, reentrancy, clearing, view synchronization, and disposal/rebinding. If using the package outside this repository, install Unity Test Framework and add `com.core.tabsystem` to the project's manifest `testables` array to expose package tests when needed.
