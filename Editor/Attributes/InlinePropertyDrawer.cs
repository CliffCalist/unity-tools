using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using WhiteArrow;

namespace WhiteArrowEditor
{
    [CustomPropertyDrawer(typeof(InlinePropertyAttribute))]
    public class InlinePropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement();

            var iterator = property.Copy();
            var end = iterator.GetEndProperty();

            iterator.NextVisible(true);
            while (!SerializedProperty.EqualContents(iterator, end))
            {
                var field = new PropertyField(iterator.Copy());
                field.BindProperty(iterator.Copy());
                root.Add(field);
                iterator.NextVisible(false);
            }

            return root;
        }
    }
}