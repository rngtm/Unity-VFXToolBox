using UnityEditor;
using UnityEngine;

namespace VfxToolBox
{
    public class VfxToolStyle
    {
        public static readonly Vector2 DragAndDropBoxSize = new Vector2(0f, 40f);

        public static readonly GUILayoutOption[] ExportButtonOption = { };
        
        public GUIStyle CommonHorizontalWrapperStyle { get; private set; }
        public GUIStyle CommonBoxStyle { get; private set; }
        public GUIStyle DragAndDropBoxStyle { get; private set; }
        public GUIStyle DragAndDropLabelStyle { get; private set; }
        public GUIStyle ExportButtonStyle { get; private set; }
        public GUIStyle CenterLabelStyle { get; private set; } // 中央ぞろえStyle
        public GUIStyle CenterWeakLabelStyle { get; private set; } // 中央ぞろえStyle
        public GUIStyle ApplyButtonStyle { get; private set; }
        public GUIStyle StartDelayButtonStyle { get; private set; }
        public GUIStyle StartDelayButtonBoxStyle { get; private set; }
        public GUIStyle StartDelayButtonRowBoxStyle { get; private set; }
        public static readonly GUILayoutOption[] DragAndDropLabelOption = new GUILayoutOption[]
        {
            GUILayout.ExpandWidth(true)
        };

        public static readonly GUILayoutOption[] DragAndDropBoxOptions = new GUILayoutOption[] { };

        public VfxToolStyle()
        {
            {
                DragAndDropLabelStyle = new GUIStyle(EditorStyles.miniLabel);
                var color = DragAndDropLabelStyle.normal.textColor;
                // color.a *= 0.5f;
                DragAndDropLabelStyle.normal.textColor = color;
                DragAndDropLabelStyle.alignment = TextAnchor.MiddleCenter;
            }
            
            {
                CommonHorizontalWrapperStyle = new GUIStyle();
                CommonHorizontalWrapperStyle.padding = new RectOffset(4, 4, 0, 0);
                // CommonHorizontalWrapperStyle.margin = new RectOffset(4, 4, 0, 0);
            }
            
            {
                CommonBoxStyle = new GUIStyle(GUI.skin.box);
                CommonBoxStyle.margin = new RectOffset(4, 4, 0, 0);
            }

            {
                DragAndDropBoxStyle = new GUIStyle();
                DragAndDropBoxStyle.margin = new RectOffset(6, 6, 4, 0);
            }

            {
                ExportButtonStyle = new GUIStyle(GUI.skin.button);
                ExportButtonStyle.margin.top = 4;
                // ExportButtonStyle.normal.background = config.GetButtonTexture();
            }

            {
                ApplyButtonStyle = new GUIStyle(EditorStyles.miniButton);
            }
            
            {
                CenterLabelStyle = new GUIStyle(EditorStyles.label);
                CenterLabelStyle.alignment = TextAnchor.MiddleCenter;
            }
            
            {
                CenterWeakLabelStyle = new GUIStyle(EditorStyles.label);
                CenterWeakLabelStyle.alignment = TextAnchor.MiddleCenter;
                var color = CenterWeakLabelStyle.normal.textColor;
                color.a *= 0.7f;
                CenterWeakLabelStyle.normal.textColor = color;
            }

            // ボタン列を囲むボックス
            {
                StartDelayButtonBoxStyle = new GUIStyle();
                // StartDelayButtonBoxStyle.padding.top = 4;
                // StartDelayButtonBoxStyle.padding.bottom = 4;
                // StartDelayButtonBoxStyle.padding.left = 18;
                // StartDelayButtonBoxStyle.padding.right = 2;
                //
                // StartDelayButtonBoxStyle.margin.right = 12;
            }
            
            // ボタンの列
            {
                StartDelayButtonRowBoxStyle = new GUIStyle(GUI.skin.box);
                StartDelayButtonRowBoxStyle.padding = new RectOffset(0, 0, 0, 0);
                StartDelayButtonRowBoxStyle.margin = new RectOffset(0, 0, 0, 1); // 外側
                // StartDelayButtonRowBoxStyle.padding.top = 4;
                // StartDelayButtonRowBoxStyle.padding.bottom = 4;
                // StartDelayButtonRowBoxStyle.padding.left = 18;
                // StartDelayButtonRowBoxStyle.padding.right = 2;
            }
            
            // ボタン
            {
                StartDelayButtonStyle = new GUIStyle(EditorStyles.miniButton);
                // StartDelayButtonStyle.padding = new RectOffset(0, 0, 0, 0);
                StartDelayButtonStyle.margin = new RectOffset(0, 0, 0, 0); // 外側
            }
            
        }
    }
}