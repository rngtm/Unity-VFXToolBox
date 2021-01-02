using UnityEditor;
using UnityEngine;

namespace VfxToolBox.Shuriken.ShurikenColorChanger
{
    public class Config
    {
        public readonly GUILayoutOption[] ColorControlBoxOptions = { GUILayout.MinWidth(40) };
        public readonly GUILayoutOption[] ApplyButtonOptions = {GUILayout.Width(48)};
        public readonly GUILayoutOption[] LabelFieldOptions = { GUILayout.Width(64)};
        public readonly GUILayoutOption[] ValueFieldOptions = { GUILayout.ExpandWidth(true), GUILayout.Width(120) };
        public GUIStyle ButtonStyle { get; private set; }
        
        public void CreateStyleIfNull()
        {
            if (ButtonStyle == null)
            {
                ButtonStyle = new GUIStyle(EditorStyles.miniButton);
            }
        }
    }
}