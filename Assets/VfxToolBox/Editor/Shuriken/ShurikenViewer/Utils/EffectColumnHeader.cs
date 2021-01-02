namespace VfxToolBox.ShurikenViewer
{
    using UnityEditor.IMGUI.Controls;
    using UnityEngine;

    /// <summary>
    /// MultiColumnHeaderStateのオーバーライド
    /// </summary>
    class EffectHeaderState : MultiColumnHeaderState
    {
        public EffectHeaderState(Column[] columns) : base(columns)
        {
        }
    }

    /// <summary>
    /// MultiColumnHeaderState.Columnのオーバーライド
    /// </summary>
    class EffectHeaderStateColumn : MultiColumnHeaderState.Column
    {
        public EffectHeaderStateColumn(string header, float width)
        {
            headerContent = new GUIContent(header);
            this.width = width;
            autoResize = false;
        }
    }

    /// <summary>
    /// MultiColumnHeaderのオーバーライド
    /// </summary>
    public class EffectColumnHeader : MultiColumnHeader
    {
        public static readonly float headerHeight = 36f;
        static readonly float labelY = 4f; // ラベル位置
        private GUIStyle style;

        public EffectColumnHeader(MultiColumnHeaderState state) : base(state)
        {
            height = headerHeight; // ヘッダーの高さ 上書き
        }

        protected override void ColumnHeaderGUI(MultiColumnHeaderState.Column column, Rect headerRect, int columnIndex)
        {
            if (canSort && column.canSort)
            {
                SortingButton(column, headerRect, columnIndex);
            }

            if (style == null)
            {
                style = new GUIStyle(DefaultStyles.columnHeader);
                style.alignment = TextAnchor.LowerLeft;
            }

            float labelHeight = headerHeight;
            Rect labelRect = new Rect(headerRect.x, headerRect.yMax - labelHeight - labelY, headerRect.width, labelHeight);
            GUI.Label(labelRect, column.headerContent, style);
        }
    }
}