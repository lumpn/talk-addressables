#  Unity Addressables for indies?
In this talk for [TGDF 2022](https://2022.tgdf.tw/en) I try to answer whether independent game developers should use [Addressables](https://docs.unity3d.com/Manual/com.unity.addressables.html) in their project and what alternatives are available.

## Slides
https://docs.google.com/presentation/d/1QdB46scuV9RLnFpo58OQaCEgxejfAu7zcufIClPeGPU

## Unity Asset Bundle Extractor
https://github.com/SeriousCache/UABE

Use UABE 3.0 (beta)

## Asset Bundle Browser
https://docs.unity3d.com/Manual/AssetBundles-Browser.html

Install via `com.unity.assetbundlebrowser`.

# Addressables configuration
Please [read my slides](https://docs.google.com/presentation/d/1QdB46scuV9RLnFpo58OQaCEgxejfAu7zcufIClPeGPU) on why using Addressables is a bad idea, except for when you are making an open world game or a live-ops game. If you want to make an open world game, I would strongly recommend using [Unreal Engine 5](https://www.unrealengine.com/en-US/unreal-engine-5) instead. UE5 is made for open world games and you won't have to deal with implementing your own streaming system.

If you must develop your open world game in Unity, this is how you should configure Addressables (version 1.21.12).

## AddressableAssetSettings

| Category | Name | Value | Notes |
|----------|------|-------|-------|
| Diagnostics | Send Profiler Events  | true | Important for profiling asset unloading and debugging memory leaks.
| Build | Build Addressables on Player Build | Build Addressables content on Player Build | Avoid bugs caused by outdated content. |
| Build | MonoScript Bundle Naming Prefix | Disable MonoScript Bundle Build | Scripts in Addressables is only for live-ops. |

## Default Local Group
Put all the prefabs that you want to stream at runtime into the default group. Do not put anything else in the group! The prefabs will automatically pull their textures, meshes, and other dependencies into their bundle.

| Category | Name | Value | Notes |
|----------|------|-------|-------|
| Content Packing & Loading | Build & Load Path  | Local | Remote is for live-ops.
| Content Packing & Loading | Asset Bundle Compression | LZ4 | Smaller bundles. |
| Content Packing & Loading | Include in Build | true | |
| Content Packing & Loading | Include Addresses in Catalog | true | This is how you load assets by string. |
| Content Packing & Loading | Include GUIDs in Catalog | false | Unless you load assets by [AssetReference](https://docs.unity3d.com/Packages/com.unity.addressables@1.21/manual/editor/AssetReferences.html). |
| Content Packing & Loading | Include Labels in Catalog | false | You're not going to load assets by label. |
| Content Packing & Loading | Bundle Mode | Pack Separately | Creates one bundle per prefab. Facilitates fine grained asset memory management. |
| Content Packing & Loading | Asset Load Mode | Requested Asset And Dependencies | Don't load more than you need. |

## Deduplication
Install the [de.lumpn.unity-deduplication](https://github.com/lumpn/unity-deduplication) package.

Go to Window &rarr; Asset Management &rarr; Addressables &rarr; Analyze. Select the "Group Duplicate Dependencies" rule, analyze selected rule, and fix selected rule.

Go to Window &rarr; Asset Management &rarr; Addressables &rarr; Groups. There now is a group called "Grouped Duplicate Asset Isolation". This group contains all shared dependencies of the default group, packed together by label into optimal bundles.

Whenever you make any changes to the default local group or its assets, just delete the entire "Grouped Duplicate Asset Isolation" group and run the "Group Duplicate Dependencies" rule again. The process is deterministic and will only produce different bundles if the dependencies changed.

## Streaming
Only use `Addressables.InstantiateAsync` to stream assets in. It takes care of loading all dependencies and tracks the handle using reference counting. Do not use any of the `Load` functions, as that causes untracked handles.

Only use `Addressables.ReleaseInstance` to stream assets out. It updates the reference count of the handle and takes care of unloading all dependencies when the handle is no longer in use. Do not call `Object.Destroy`, as that leaks tracked handles.
