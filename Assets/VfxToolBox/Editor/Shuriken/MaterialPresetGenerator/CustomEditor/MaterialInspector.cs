// using System;
// using System.Reflection;
// using UnityEditor;
// using UnityEngine;
//
// namespace VfxTools.Shuriken.ShaderPresetGenerator
// {
//     [CustomEditor(typeof(Material))]
//     public class MaterialInspector : MaterialEditor
//     {
//         public override void OnInspectorGUI()
//         {
//             DrawButton();
//             base.OnInspectorGUI();
//             
//             // if (editor != null)
//             //     editor.OnInspectorGUI();
//             
//             // var defaultParticleSystemInspectorType
//             // DrawButton();
//             // base.OnInspectorGUI();
//         }
//
//         private void DrawButton()
//         {
//             EditorGUI.BeginDisabledGroup(VfxToolState.LastSelectionGameObject == null);
//             if (GUILayout.Button($"Apply to \"{VfxToolState.SelectionGameObjectName}\""))
//             {
//                 if (VfxToolState.LastSelectionGameObject != null)
//                 {
//                     var material = target as Material;
//                     var preset = ShaderPresetDatabase.Get()?.FindPreset(material.shader);
//                     PresetUtility.ApplyPreset(VfxToolState.LastSelectionGameObject, preset, material);
//                     Selection.activeObject = target;
//                 }
//             }
//             EditorGUI.EndDisabledGroup();
//         }    
//     }
// }