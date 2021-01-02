using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace VfxToolBox.Utility
{
    public class CustomUI
    {
        public static bool Toggle(bool on,  Action drawAction)
        {
            EditorGUILayout.BeginHorizontal();
            on = EditorGUILayout.Toggle(on, GUILayout.Width(25));
            drawAction.Invoke();
            EditorGUILayout.EndHorizontal();

            return on;
        }
        
        /// <summary>
        /// ツールバー
        /// </summary>
        public static void Toolbar(string label)
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                EditorGUILayout.LabelField(label);
            }
        }
        
        /// <summary>
        /// チェックボックス付きツールバー
        /// </summary>
        public static bool ToolbarWithToggle(string label, string toggleLabel, bool toggleOn)
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                EditorGUILayout.LabelField(label, GUILayout.MinWidth(32));
                GUILayout.FlexibleSpace();
                toggleOn = GUILayout.Toggle(toggleOn, toggleLabel);
                GUILayout.Space(6);

                return toggleOn;
            }
        }
        
        /// <summary>
        /// ボタン付き付きツールバー(クリックされたらtrueを返す)
        /// </summary>
        public static bool ToolbarWithButton(string label, string buttonLabel)
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                bool click = false;
                EditorGUILayout.LabelField(label, GUILayout.MinWidth(32));
                GUILayout.FlexibleSpace();
                click = GUILayout.Button(buttonLabel, EditorStyles.toolbarButton);

                return click;
            }
        }
        
        /// <summary>
        /// 折りたたみ付きツールバー
        /// </summary>
        public static bool ToolbarWithFoldout(string label, bool isOpen)
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                // EditorGUILayout.LabelField(label, GUILayout.MinWidth(32));
                isOpen = EditorGUILayout.Foldout(isOpen, label);
                GUILayout.FlexibleSpace();
                // click = GUILayout.Button(buttonLabel, EditorStyles.toolbarButton);

                return isOpen;
            }
        }
        
        /// <summary>
        /// ドラッグアンドドロップボックス
        /// </summary>
        public static void DrawDragAndDropBox(string label, Texture frameTexture, VfxToolStyle style, Action<Object[]> dragAction)
        {
            var evt = Event.current;
            var dropArea = GUILayoutUtility.GetRect(
                VfxToolStyle.DragAndDropBoxSize.x,
                VfxToolStyle.DragAndDropBoxSize.y,
                style.DragAndDropBoxStyle,
                GUILayout.ExpandWidth(true)
            );

            GUI.DrawTexture(dropArea, frameTexture);
            // EditorGUI.LabelField(dropArea, "[Drag & Drop Materials Here]", style.DragAndDropLabelStyle);
            EditorGUI.LabelField(dropArea, label, style.DragAndDropLabelStyle);
            switch (evt.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!dropArea.Contains(evt.mousePosition)) break;

                    //マウスの形状
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (evt.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();
                        dragAction.Invoke(DragAndDrop.objectReferences);
                        DragAndDrop.activeControlID = 0;
                    }

                    Event.current.Use();
                    break;
            }
        }

    }
}