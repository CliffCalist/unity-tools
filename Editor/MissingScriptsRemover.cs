using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace WhiteArrowEditor
{
    public static class RemoveMissingScriptsEditor
    {
        [MenuItem("GameObject/WhiteArrow/Remove Missing Scripts")]
        private static void FindAndRemoveMissingInSelected()
        {
            var allObjects = GetAllChildren(Selection.gameObjects);
            var count = RemoveMissingScriptsFrom(allObjects);
            if (count == 0) return;
            EditorUtility.DisplayDialog("Remove Missing Scripts", $"Removed {count} missing scripts.\n\nCheck console for details", "ok");
        }



        [MenuItem("Assets/WhiteArrow/Remove Missing Scripts")]
        private static void FindAndRemoveMissingInSelectedAssets()
        {
            FindAndRemoveMissingInSelected();
        }



        [MenuItem("Assets/WhiteArrow/Remove Missing Scripts", true)]
        private static bool FindAndRemoveMissingInSelectedAssetsValidate()
        {
            return Selection.objects.OfType<GameObject>().Any();
        }



        [MenuItem("Tools/WhiteArrow/Editor/Remove Missing Scripts From Prefabs")]
        private static void RemoveFromPrefabs()
        {
            var allPrefabGuids = AssetDatabase.FindAssets("t:Prefab");
            var allPrefabsPath = allPrefabGuids.Select(AssetDatabase.GUIDToAssetPath);
            var allPrefabsObjects = allPrefabsPath.Select(AssetDatabase.LoadAssetAtPath<GameObject>);

            var prefabsToSave = new List<GameObject>();
            foreach (var prefab in allPrefabsObjects)
            {
                if (prefab != null && CanEditPrefab(prefab))
                {
                    ProcessPrefab(prefab);
                    prefabsToSave.Add(prefab);
                }
            }

            foreach (var prefab in prefabsToSave)
            {
                if (prefab != null)
                    PrefabUtility.SavePrefabAsset(prefab);
            }

            Debug.Log($"Removed All Missing Scripts from Prefabs");
        }

        private static void ProcessPrefab(GameObject prefab)
        {
            if (prefab == null) return;

            RemoveMissingScriptsFrom(prefab.transform.GetComponentsInChildren<Transform>(true)
                .Select(t => t.gameObject)
                .ToArray());

            PrefabUtility.SavePrefabAsset(prefab);
            RemoveMissingScriptsFrom(prefab);
        }

        private static bool CanEditPrefab(GameObject prefab)
        {
            var path = AssetDatabase.GetAssetPath(prefab);
            return !string.IsNullOrEmpty(path) && AssetDatabase.IsOpenForEdit(path);
        }



        private static int RemoveMissingScriptsFrom(params GameObject[] objects)
        {
            var forceSave = new List<GameObject>();
            var removedCounter = 0;

            foreach (GameObject current in objects)
            {
                if (current == null) continue;

                var missingCount = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(current);
                if (missingCount == 0) continue;

                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(current);
                EditorUtility.SetDirty(current);

                if (EditorUtility.IsPersistent(current) && PrefabUtility.IsAnyPrefabInstanceRoot(current))
                    forceSave.Add(current);

                Debug.Log($"Removed {missingCount} Missing Scripts from {current.name}", current);
                removedCounter += missingCount;
            }

            return removedCounter;
        }



        private static GameObject[] GetAllChildren(GameObject[] selection)
        {
            var transforms = new List<Transform>();

            foreach (var o in selection)
                transforms.AddRange(o.GetComponentsInChildren<Transform>(true));

            return transforms.Distinct().Select(t => t.gameObject).ToArray();
        }
    }
}