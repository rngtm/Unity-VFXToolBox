// using UnityEditor;
// using UnityEditorInternal;
// using UnityEngine;
//
// namespace VfxTools.Shuriken.ShaderSetup
// {
//     [CustomEditor(typeof(ShaderToolConfig))]
//     public class ToolConfigInspector : Editor
//     {
//         private ReorderableList presetList;
//         
//         public override void OnInspectorGUI()
//         {
//             base.OnInspectorGUI();
//             
//             EditorGUILayout.Space();
//             if (presetList == null)
//             {
//                 CreateReorderableList();
//             }
//             
//             // 描画
//             presetList.DoLayoutList();
//             serializedObject.ApplyModifiedProperties();
//         }
//
//         private void CreateReorderableList()
//         {
//             var shaderPresets = serializedObject.FindProperty("materialPresets");
//             presetList = new ReorderableList(serializedObject, shaderPresets);
//
//             presetList.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Material Presets");
//
//             presetList.drawElementCallback = (rect, index, active, focused) =>
//             {
//                 var elementProperty = shaderPresets.GetArrayElementAtIndex(index);
//                 rect.height = EditorGUIUtility.singleLineHeight;
//                 EditorGUI.PropertyField( rect, elementProperty, new GUIContent("Preset " + index));
//             };
//         }
//     }
// }