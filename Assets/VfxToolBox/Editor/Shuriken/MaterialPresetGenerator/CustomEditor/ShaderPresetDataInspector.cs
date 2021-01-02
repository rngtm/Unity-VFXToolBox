using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace VfxToolBox.Shuriken.ShaderPresetGenerator
{
    [CustomEditor(typeof(ShaderPresetData))]
    public class ShaderPresetDataInspector : Editor
    {
        private GUIStyle previewTextureStyle;
        private ReorderableList vertexStreamList;
        private ReorderableList customDataList;

        private void CreateStyleIfNull()
        {
            if (previewTextureStyle == null)
            {
                previewTextureStyle = new GUIStyle {alignment = TextAnchor.MiddleLeft};
            }
        }

        public override void OnInspectorGUI()
        {
            DrawShader();

            // GUILayout.Space(2f);
            EditorGUILayout.Space();
            if (vertexStreamList == null)
            {
                CreateReorderableList();
            }

            // 描画
            customDataList.DoLayoutList();
            EditorGUILayout.Space();
            vertexStreamList.DoLayoutList();

            // // テクスチャ表示
            // var preset = target as ShaderPresetData;
            // var texture = preset.GetShader().mainTexture;
            // if (texture != null)
            // {
            //     EditorGUILayout.BeginHorizontal();
            //     int textureSize = 120;
            //
            //     // 黒背景で表示
            //     using (new EditorGUILayout.VerticalScope())
            //     {
            //         EditorGUILayout.LabelField("Preview (Alpha)");
            //         var rect = EditorGUILayout.GetControlRect(false, textureSize);
            //         GUI.DrawTexture(rect, Texture2D.blackTexture, ScaleMode.ScaleToFit, alphaBlend: false);
            //         GUI.DrawTexture(rect, texture, ScaleMode.ScaleToFit, true);
            //     }
            //
            //     // 背景無しで表示
            //     using (new EditorGUILayout.VerticalScope())
            //     {
            //         EditorGUILayout.LabelField("Preview (No Alpha)");
            //         var rect = EditorGUILayout.GetControlRect(false, textureSize);
            //         GUI.DrawTexture(rect, texture, ScaleMode.ScaleToFit, false);
            //     }
            //
            //     // EditorGUI.DrawPreviewTexture(rect, preset.GetMaterial().mainTexture);
            //     EditorGUILayout.EndHorizontal();
            // }

            // 変更の反映
            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();
            
            base.OnInspectorGUI();
        }

        /// <summary>
        /// プリセットが持つシェーダーを表示
        /// </summary>
        private void DrawShader()
        {
            var preset = target as ShaderPresetData;
            if (preset == null) return;
            
            var shader = preset.GetShader();
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField("Shader", shader, typeof(Shader), false);
            EditorGUI.EndDisabledGroup();
        }
        //
        // private void DrawButton()
        // {
        //     EditorGUI.BeginDisabledGroup(VfxToolState.LastSelectionGameObject == null);
        //     if (GUILayout.Button($"Apply to \"{VfxToolState.SelectionGameObjectName}\""))
        //     {
        //         if (VfxToolState.LastSelectionGameObject != null)
        //         {
        //             PresetUtility.ApplyPreset(VfxToolState.LastSelectionGameObject, target as ShaderPresetData);
        //             Selection.activeObject = target;
        //         }
        //     }
        //     EditorGUI.EndDisabledGroup();
        // }


        private void CreateReorderableList()
        {
            // values
            {
                var values = serializedObject.FindProperty("values");
                customDataList = new ReorderableList(serializedObject, values);
                // customDataList.elementHeight = 66;
                customDataList.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "CustomData");
                customDataList.drawElementCallback = (rect, index, active, focused) =>
                {
                    var elementProperty = values.GetArrayElementAtIndex(index);
                    rect.height = EditorGUIUtility.singleLineHeight;
                    rect.height -= 4;
                    // rect.y += 1;
                    // EditorGUI.LabelField(rect, new GUIContent("Float" + index));
                    rect.height += EditorGUIUtility.singleLineHeight;
                    EditorGUI.PropertyField(rect, elementProperty);
                };
            }

            // vertexStreams
            {
                var vertexSteams = serializedObject.FindProperty("vertexStreams");
                vertexStreamList = new ReorderableList(serializedObject, vertexSteams);
                vertexStreamList.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Vertex Streams");
                vertexStreamList.drawElementCallback = (rect, index, active, focused) =>
                {
                    var elementProperty = vertexSteams.GetArrayElementAtIndex(index);
                    rect.height = EditorGUIUtility.singleLineHeight;
                    rect.height -= 4;
                    rect.y += 2;
                    EditorGUI.PropertyField(rect, elementProperty, new GUIContent("Preset " + index));
                };
            }
        }
    }
}