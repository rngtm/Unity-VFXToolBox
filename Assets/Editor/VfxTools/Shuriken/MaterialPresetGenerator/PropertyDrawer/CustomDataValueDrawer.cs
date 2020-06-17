using UnityEditor;
using UnityEngine;

namespace VfxTools.Shuriken.ShaderPresetGenerator
{
    [CustomPropertyDrawer(typeof(CustomDataValue))]
    public class CustomDataValueDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            //元は 1 つのプロパティーであることを示すために PropertyScope で囲む
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                position.height = EditorGUIUtility.singleLineHeight;

                //各プロパティーの Rect を求める
                Rect nameRect = new Rect(position)
                {
                    // y = position.y + EditorGUIUtility.singleLineHeight
                };

                Rect valueRect = new Rect(nameRect)
                {
                    y = nameRect.y + EditorGUIUtility.singleLineHeight + 2
                };
                
                //各プロパティーの SerializedProperty を求める
                SerializedProperty nameProperty = property.FindPropertyRelative("name");
                // SerializedProperty valueProperty = property.FindPropertyRelative("minMaxCurve");

                nameProperty.stringValue = EditorGUI.TextField(nameRect, "Name", nameProperty.stringValue);
                // EditorGUI.PropertyField(valueRect, valueProperty);
            }
            
        }
    }
}