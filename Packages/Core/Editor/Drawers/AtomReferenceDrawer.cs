using UnityEditor;
using UnityEngine;

namespace UnityAtoms.Editor
{
    /// <summary>
    /// A custom property drawer for References. Makes it possible to choose between a value, Variable, Constant or a Variable Instancer.
    /// </summary>

    [CustomPropertyDrawer(typeof(AtomReferenceBase), true)]
    public class AtomReferenceDrawer : PropertyDrawer
    {
        private static readonly string[] _popupOptions =
            { "Use Value", "Use Constant", "Use Variable", "Use Variable Instancer" };
        private static GUIStyle _popupStyle;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty usage = property.FindPropertyRelative("_usage");
            SerializedProperty value = property.FindPropertyRelative("_value");
            SerializedProperty constant = property.FindPropertyRelative("_constant");
            SerializedProperty variable = property.FindPropertyRelative("_variable");
            SerializedProperty variableInstancer = property.FindPropertyRelative("_variableInstancer");

            return EditorGUI.GetPropertyHeight(GetPropToUse(usage, value, constant, variable, variableInstancer), label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_popupStyle == null)
            {
                _popupStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions"));
                _popupStyle.imagePosition = ImagePosition.ImageOnly;
            }

            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);

            EditorGUI.BeginChangeCheck();

            // Get properties
            SerializedProperty usage = property.FindPropertyRelative("_usage");
            SerializedProperty value = property.FindPropertyRelative("_value");
            SerializedProperty constant = property.FindPropertyRelative("_constant");
            SerializedProperty variable = property.FindPropertyRelative("_variable");
            SerializedProperty variableInstancer = property.FindPropertyRelative("_variableInstancer");

            // Calculate rect for configuration button
            Rect buttonRect = new Rect(position);
            buttonRect.yMin += _popupStyle.margin.top;
            buttonRect.width = _popupStyle.fixedWidth + _popupStyle.margin.right;
            position.xMin = buttonRect.xMax;

            // Store old indent level and set it to 0, the PrefixLabel takes care of it
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            usage.intValue = EditorGUI.Popup(buttonRect, usage.intValue, _popupOptions, _popupStyle);

            EditorGUI.PropertyField(position,
                GetPropToUse(usage, value, constant, variable, variableInstancer),
                GUIContent.none);

            if (EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        private SerializedProperty GetPropToUse(SerializedProperty usage, SerializedProperty value, SerializedProperty constant, SerializedProperty variable, SerializedProperty variableInstancer)
        {
            var usageIntVal = (AtomReferenceBase.Usage)usage.intValue;
            if (usageIntVal == AtomReferenceBase.Usage.Constant)
            {
                return constant;
            }
            else if (usageIntVal == AtomReferenceBase.Usage.Variable)
            {
                return variable;
            }
            else if (usageIntVal == AtomReferenceBase.Usage.VariableInstancer)
            {
                return variableInstancer;
            }
            else  // if (usageIntVal == AtomReferenceBase.Usage.Value)
            {
                return value;
            }
        }


    }
}
