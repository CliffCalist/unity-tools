using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace WhiteArrowEditor
{
    public class RenameSelectedObjectsWindow : EditorWindow
    {
        private TextField _inputField;


        [MenuItem("Tools/WhiteArrow/Editor/Rename Selected Objects")]
        public static void ShowWindow()
        {
            GetWindow<RenameSelectedObjectsWindow>("Rename Selected Objects");
        }


        private void CreateGUI()
        {
            try
            {
                _inputField = new TextField();
                rootVisualElement.Add(_inputField);

                var confirmButton = new Button(OnButtonConfirmPressed);
                confirmButton.text = "Rename";
                rootVisualElement.Add(confirmButton);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void OnButtonConfirmPressed()
        {
            var namePreset = _inputField.value;
            if (string.IsNullOrEmpty(namePreset))
                Debug.LogException(new Exception("Incorrect name."));
            else
            {
                var selectedObjects = Selection.gameObjects;
                var isChanged = false;
                var index = -1;
                foreach (var selectedObject in selectedObjects)
                {
                    string newName = string.Empty;
                    if (index >= 0)
                        newName = $"{namePreset} ({index})";
                    else newName = namePreset;
                    selectedObject.name = newName;
                    index++;
                    isChanged = true;
                }

                if (isChanged)
                {
                    var currentScene = SceneManager.GetActiveScene();
                    EditorSceneManager.MarkSceneDirty(currentScene);
                }

                Close();
            }
        }
    }
}