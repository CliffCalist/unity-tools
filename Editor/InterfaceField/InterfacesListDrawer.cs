using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace WhiteArrowEditor
{
    [CustomPropertyDrawer(typeof(WhiteArrow.InterfacesList<>), true)]
    public class InterfacesListDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var interfaceType = fieldInfo.FieldType.GetGenericArguments()[0];
            var listProp = property.FindPropertyRelative("_items");

            var container = CreateStyledContainer();
            var dropZone = CreateDropZone(property, listProp, interfaceType);
            var listField = CreateListField(property, listProp);

            container.Add(dropZone);
            container.Add(listField);

            return container;
        }

        private VisualElement CreateStyledContainer()
        {
            var container = new VisualElement();
            container.style.marginBottom = 6;
            container.style.marginTop = 2;
            container.style.borderTopWidth = 1.5f;
            container.style.borderBottomWidth = 1.5f;
            container.style.borderLeftWidth = 1.5f;
            container.style.borderRightWidth = 1.5f;
            container.style.borderTopLeftRadius = 4;
            container.style.borderTopRightRadius = 4;
            container.style.borderBottomLeftRadius = 4;
            container.style.borderBottomRightRadius = 4;
            var borderColor = new Color(0.15f, 0.15f, 0.15f);
            container.style.borderTopColor = borderColor;
            container.style.borderBottomColor = borderColor;
            container.style.borderLeftColor = borderColor;
            container.style.borderRightColor = borderColor;
            return container;
        }

        private VisualElement CreateDropZone(SerializedProperty property, SerializedProperty listProp, Type interfaceType)
        {
            var dropZone = new VisualElement();
            dropZone.style.backgroundColor = new Color(0.15f, 0.15f, 0.15f);
            dropZone.style.paddingTop = 4;
            dropZone.style.paddingBottom = 4;
            dropZone.style.paddingLeft = 6;
            dropZone.style.paddingRight = 4;
            dropZone.style.marginBottom = 0;
            dropZone.style.borderBottomColor = new Color(0.25f, 0.25f, 0.25f);
            dropZone.style.borderBottomWidth = 1;

            var dropLabel = new Label("Drag objects here to add interface implementations");
            dropLabel.style.unityFontStyleAndWeight = FontStyle.Italic;
            dropLabel.style.unityTextAlign = TextAnchor.MiddleLeft;
            dropLabel.style.flexGrow = 1;
            dropZone.Add(dropLabel);

            dropZone.RegisterCallback<DragUpdatedEvent>(evt =>
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                evt.StopPropagation();
            });

            dropZone.RegisterCallback<DragPerformEvent>(evt =>
            {
                DragAndDrop.AcceptDrag();

                foreach (var draggedObject in DragAndDrop.objectReferences)
                {
                    if (draggedObject is GameObject go)
                    {
                        var comp = go.GetComponent(interfaceType);
                        if (comp != null)
                        {
                            AddElementToList(listProp, comp as UnityEngine.Object);
                        }
                    }
                    else if (draggedObject is Component c)
                    {
                        var comp = c.GetComponent(interfaceType);
                        if (comp != null)
                        {
                            AddElementToList(listProp, comp as UnityEngine.Object);
                        }
                    }
                }

                property.serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(property.serializedObject.targetObject);
                evt.StopPropagation();
            });

            return dropZone;
        }

        private PropertyField CreateListField(SerializedProperty property, SerializedProperty listProp)
        {
            var listField = new PropertyField(listProp, property.displayName);
            listField.style.marginTop = 4;
            listField.style.marginBottom = 4;
            listField.style.marginLeft = 6;
            listField.style.marginRight = 6;
            listField.BindProperty(listProp);
            return listField;
        }

        private void AddElementToList(SerializedProperty listProp, UnityEngine.Object value)
        {
            int newIndex = listProp.arraySize;
            listProp.InsertArrayElementAtIndex(newIndex);
            var newElement = listProp.GetArrayElementAtIndex(newIndex);
            var unityProp = newElement.FindPropertyRelative("_unityValue");
            unityProp.objectReferenceValue = value;
        }
    }
}