# Unity Tools by WhiteArrow
A library of runtime and editor utilities to simplify common tasks in Unity development.

## Features

### Runtime Utilities
- **UnityCheck** — utility for safe null checks when accessing UnityEngine.Object instances through interfaces
- **InterfaceField<T> & InterfacesList<T>** — support for serializing interface references with Inspector integration
- **InlinePropertyAttribute** — inline display of nested serialized objects
- **ReadOnlyAttribute** — mark fields as read-only in the Inspector
- **Coroutines** — run coroutines without MonoBehaviours
- **RuntimeTicker** — tick/update logic for non-MonoBehaviour objects

### Editor Utilities
- **TransformCopier** — copy transform hierarchy from one object to another
- **RenameSelectedObjectsWindow** — batch rename selected GameObjects
- **RemoveMissingScriptsEditor** — remove all missing script references from scenes or prefabs

## Installing

To install via UPM, use "Install package from git URL" and add the following:

```
https://github.com/CliffCalist/Unity-Tools.git
```

## Usage

### UnityCheck
Handles safe null checks when referencing UnityEngine.Object-derived instances via interfaces.  
Since Unity overrides `== null` for destroyed objects only in UnityEngine.Object types, checks like this can fail:

```csharp
IMyInterface obj = destroyedMonoBehaviour;
if (obj == null) { /* won't be true */ }
```

Use UnityCheck instead:

```csharp
if (UnityCheck.IsDestroyed(obj)) return;
```

### InterfaceField<T> & InterfacesList<T>
These are generic types that allow referencing and serializing interfaces with full Inspector support.

```csharp
[SerializeField]
private InterfaceField<IMyService> _service;

[SerializeField]
private InterfacesList<IMySystem> _systems;
```

- `InterfaceField<T>` behaves like a wrapper around an interface implementation.
- `InterfacesList<T>` is a list variant with a custom Inspector to support serialized interface collections.
- Both are implicitly castable to `T`, so you can use them naturally in code.

### InlinePropertyAttribute
Displays the content of nested `SerializedObject` fields inline:

```csharp
[InlineProperty]
public MySubConfig Config;
```

### ReadOnlyAttribute
Allows marking serialized fields as read-only in the Inspector.

```csharp
[SerializeField, ReadOnly] private int _debugId;
```

### Coroutines
Run coroutines outside MonoBehaviour via a static utility:

```csharp
Coroutines.Launch(MyCoroutine());
```

### RuntimeTicker
Register `ITickable` objects to receive tick updates every frame:

```csharp
public class MyLogic : ITickable {
    public void Tick(float deltaTime) { /* ... */ }
}

// Register
RuntimeTicker.Register(myLogic);

// Unregister when done
RuntimeTicker.Unregister(myLogic);
```

### TransformCopier
Use from menu:  
`Tools/WhiteArrow/Editor/Copy Transforms Hierarchy`  
Copies local position, rotation, and scale from source to target hierarchy.

### RenameSelectedObjectsWindow
Use from menu:  
`Tools/WhiteArrow/Editor/Rename Selected Objects`  
Simple UI tool for bulk renaming selected GameObjects in the scene.

### RemoveMissingScriptsEditor
Use from menu:  
`Tools/WhiteArrow/Editor/Remove Missing Scripts`  
Removes all missing MonoBehaviour references from the current scene or prefab assets.

## Roadmap

- [ ] Add more editor tools
- [ ] Split the package into focused sub-packages to reduce unnecessary overhead
- [ ] Refactor UnityCheck into an extension method for `System.Object`
