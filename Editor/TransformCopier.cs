using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace WhiteArrowEditor
{
    public class TransformCopier : EditorWindow
    {
        private ObjectField _sourceRootField;
        private ObjectField _targetRootField;



        [MenuItem("Tools/WhiteArrow/Editor/Copy Transforms Hierarchy")]
        public static void ShowWindow()
        {
            GetWindow<TransformCopier>("Copy Transforms");
        }


        private void CreateGUI()
        {
            try
            {
                var label = new Label("Copy Transforms From Source To Target");
                rootVisualElement.Add(label);

                _sourceRootField = new ObjectField("Source Root");
                _sourceRootField.objectType = typeof(Transform);
                rootVisualElement.Add(_sourceRootField);

                _targetRootField = new ObjectField("Target Root");
                _targetRootField.objectType = typeof(Transform);
                rootVisualElement.Add(_targetRootField);

                var button = new Button(OnButtonConfirmPressed);
                button.text = "Copy Transforms";
                rootVisualElement.Add(button);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }



        private void OnButtonConfirmPressed()
        {
            var sourceObject = _sourceRootField.value as Transform;
            var targetObject = _targetRootField.value as Transform;

            if (sourceObject == null || targetObject == null)
                Debug.LogException(new Exception("Fields are empty."));

            Undo.RegisterCompleteObjectUndo(targetObject, "Copy Transforms");
            CopyTransformsRecursive(sourceObject.transform, targetObject.transform);
            Debug.Log("Transform copying completed.");
        }

        private void CopyTransformsRecursive(Transform source, Transform target)
        {
            target.localPosition = source.localPosition;
            target.localRotation = source.localRotation;
            target.localScale = source.localScale;

            int count = Mathf.Min(source.childCount, target.childCount);
            for (int i = 0; i < count; i++)
            {
                CopyTransformsRecursive(source.GetChild(i), target.GetChild(i));
            }
        }
    }
}