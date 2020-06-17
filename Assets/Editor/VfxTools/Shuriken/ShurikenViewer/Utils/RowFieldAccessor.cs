namespace VfxTools.ShurikenViewer
{
    using UnityEngine;

    public class RowFieldAccessor
    {
        public string Title = "";
        public float Width = 100f;
        public System.Action<Rect, ParticleSystem> ObjectField = null;

        public RowFieldAccessor(string title, float width, System.Action<Rect, ParticleSystem> extractValue)
        {
            Title = title;
            Width = width;
            //Width = EditorStyles.label.CalcSize(new GUIContent(title)).x;

            ObjectField = extractValue;
        }
    }
}