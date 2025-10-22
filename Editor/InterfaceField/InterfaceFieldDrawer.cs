using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using WhiteArrow;

namespace WhiteArrowEditor
{
    [CustomPropertyDrawer(typeof(InterfaceField<>), true)]
    public class InterfaceFieldDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var unityValueProp = property.FindPropertyRelative("_unityValue");
            var simpleValueProp = property.FindPropertyRelative("_simpleValue");

            var interfaceType = DefineInterfaceType();

            bool foldoutState = false;

            var box = CreateBox();
            var header = CreateHeader(property.displayName, interfaceType, ref foldoutState);
            var contentBody = CreateContentBody();

            box.Add(header);
            box.Add(contentBody);

            var objectField = CreateObjectField(unityValueProp, interfaceType);
            var simpleField = CreateSimpleField(simpleValueProp);

            contentBody.Add(objectField);
            contentBody.Add(simpleField);

            UpdateFieldVisibility(IsSimpleValue(simpleValueProp), foldoutState, contentBody, objectField, simpleField, header);

            header.RegisterCallback<MouseDownEvent>(_ =>
            {
                foldoutState = !foldoutState;
                UpdateFoldoutArrow(header, foldoutState);
                UpdateFieldVisibility(IsSimpleValue(simpleValueProp), foldoutState, contentBody, objectField, simpleField, header);
            });

            objectField.RegisterValueChangedCallback(evt =>
            {
                var validated = ValidateObject(evt.newValue, interfaceType);
                unityValueProp.objectReferenceValue = validated;
                unityValueProp.serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(unityValueProp.serializedObject.targetObject);

                objectField.SetValueWithoutNotify(validated);

                UpdateFieldVisibility(IsSimpleValue(simpleValueProp), foldoutState, contentBody, objectField, simpleField, header);
            });

            return box;
        }

        private VisualElement CreateBox()
        {
            var box = new VisualElement();

            box.style.borderTopWidth = 1.5f;
            box.style.borderBottomWidth = 1.5f;
            box.style.borderLeftWidth = 1.5f;
            box.style.borderRightWidth = 1.5f;

            box.style.borderTopLeftRadius = 4;
            box.style.borderTopRightRadius = 4;
            box.style.borderBottomLeftRadius = 4;
            box.style.borderBottomRightRadius = 4;

            var borderColor = new Color(0.15f, 0.15f, 0.15f);
            box.style.borderTopColor = borderColor;
            box.style.borderBottomColor = borderColor;
            box.style.borderLeftColor = borderColor;
            box.style.borderRightColor = borderColor;

            box.style.marginBottom = 6;
            box.style.marginTop = 2;
            return box;
        }

        private VisualElement CreateHeader(string labelText, Type interfaceType, ref bool foldoutState)
        {
            var headerContainer = new VisualElement();
            headerContainer.style.backgroundColor = new Color(0.15f, 0.15f, 0.15f);
            headerContainer.style.marginBottom = 0;

            headerContainer.style.paddingTop = 4;
            headerContainer.style.paddingBottom = 4;
            headerContainer.style.paddingLeft = 6;
            headerContainer.style.paddingRight = 4;

            headerContainer.style.borderBottomColor = new Color(0.25f, 0.25f, 0.25f);
            headerContainer.style.borderBottomWidth = 1;


            var headerRow = new VisualElement();
            headerRow.style.flexDirection = FlexDirection.Row;
            headerRow.style.alignItems = Align.Center;

            var foldoutArrow = new VisualElement();
            foldoutArrow.style.width = 13;
            foldoutArrow.style.height = 13;
            foldoutArrow.style.backgroundImage = EditorGUIUtility.IconContent("IN foldout").image as Texture2D;
            foldoutArrow.style.marginRight = 6;
            headerRow.Add(foldoutArrow);

            UpdateFoldoutArrow(headerContainer, foldoutState);

            var label = new Label($"{labelText} <size=9><i>as {interfaceType.Name}</i></size>");
            label.style.unityFontStyleAndWeight = FontStyle.Bold;
            label.style.unityTextAlign = TextAnchor.MiddleLeft;
            label.style.flexGrow = 1;
            headerRow.Add(label);

            headerContainer.Add(headerRow);

            headerContainer.userData = foldoutArrow;

            return headerContainer;
        }

        private bool IsSimpleValue(SerializedProperty simpleValueProp) => simpleValueProp.managedReferenceValue != null;

        private void UpdateFoldoutArrow(VisualElement headerContainer, bool foldoutState)
        {
            var foldoutArrow = headerContainer.userData as VisualElement;
            if (foldoutArrow != null)
            {
                foldoutArrow.style.rotate = foldoutState ? new StyleRotate(new Rotate(90)) : new StyleRotate();
                foldoutArrow.style.display = DisplayStyle.Flex;
            }
        }

        private VisualElement CreateContentBody()
        {
            var contentBody = new VisualElement();

            contentBody.style.marginTop = 6;
            contentBody.style.marginBottom = 4;
            contentBody.style.marginLeft = 6;
            contentBody.style.marginRight = 6;

            return contentBody;
        }

        private PropertyField CreateSimpleField(SerializedProperty simpleValueProp)
        {
            var field = new PropertyField(simpleValueProp, string.Empty);
            field.BindProperty(simpleValueProp);
            field.RegisterValueChangeCallback(_ => { EditorUtility.SetDirty(simpleValueProp.serializedObject.targetObject); });

            return field;
        }

        private ObjectField CreateObjectField(SerializedProperty unityValueProp, Type interfaceType)
        {
            var objectField = new ObjectField
            {
                objectType = typeof(UnityEngine.Object),
                value = unityValueProp.objectReferenceValue,
                tooltip = $"Must implement {interfaceType.Name}"
            };

            objectField.label = string.Empty;
            objectField.style.marginTop = 4;
            objectField.style.marginBottom = 4;
            objectField.style.marginLeft = 6;
            objectField.style.marginRight = 6;

            return objectField;
        }

        private void UpdateFieldVisibility(bool isSimpleValue, bool foldoutState, VisualElement contentBody, ObjectField objectField, PropertyField simpleField, VisualElement header)
        {
            var foldoutArrow = header.userData as VisualElement;
            if (isSimpleValue)
            {
                if (foldoutArrow != null)
                {
                    foldoutArrow.style.rotate = foldoutState ? new StyleRotate(new Rotate(90)) : new StyleRotate();
                    foldoutArrow.style.display = DisplayStyle.Flex;
                }

                contentBody.style.display = foldoutState ? DisplayStyle.Flex : DisplayStyle.None;

                objectField.style.display = DisplayStyle.None;
                simpleField.style.display = DisplayStyle.Flex;
            }
            else
            {
                if (foldoutArrow != null)
                {
                    foldoutArrow.style.display = DisplayStyle.None;
                }
                contentBody.style.display = DisplayStyle.Flex;

                objectField.style.display = DisplayStyle.Flex;
                simpleField.style.display = DisplayStyle.None;
            }
        }

        private Type DefineInterfaceType()
        {
            var typeArg = fieldInfo.FieldType.GetGenericArguments()[0];
            var interfaceType = typeArg;

            if (typeArg.IsGenericType && typeArg.GetGenericTypeDefinition() == typeof(InterfaceField<>))
                interfaceType = typeArg.GetGenericArguments()[0];

            return interfaceType;
        }


        private UnityEngine.Object ValidateObject(UnityEngine.Object obj, Type interfaceType)
        {
            if (obj == null)
                return null;

            if (interfaceType.IsAssignableFrom(obj.GetType()))
                return obj;

            if (obj is GameObject go)
            {
                var component = go.GetComponent(interfaceType);
                return component;
            }

            if (obj is Component c)
            {
                var component = c.GetComponent(interfaceType);
                return component;
            }

            return null;
        }
    }
}