using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine;
using UnityEditor;
using VfxToolBox.Shuriken.TimeChanger;
using VfxToolBox.Utility;
using Object = System.Object;

namespace VfxToolBox
{
    internal class Config
    {
        public readonly GUILayoutOption[] ControlBoxOptions = {GUILayout.MinWidth(40)};
        public readonly GUILayoutOption[] ApplyButtonOptions = {GUILayout.Width(48)};
        public readonly GUILayoutOption[] LabelFieldOptions = {GUILayout.Width(48)};
        public readonly GUILayoutOption[] ValueFieldOptions = {GUILayout.ExpandWidth(true), GUILayout.Width(120)};
        public GUIStyle ButtonStyle { get; private set; }
        public GUIStyle ObjectFieldStyle { get; private set; }

        public void CreateStyleIfNull()
        {
            if (ButtonStyle == null)
            {
                ButtonStyle = new GUIStyle(EditorStyles.miniButton);
            }

            if (ObjectFieldStyle == null)
            {
                ObjectFieldStyle = new GUIStyle();
                ObjectFieldStyle.margin.bottom = 0;
            }
        }
    }

    public abstract class VfxControllerWindowBase : VfxWindowBase
    {
        [SerializeField] protected VfxControllerGuiState internalGuiState = new VfxControllerGuiState();
        private readonly Config config = new Config();

        protected virtual string controlBoxHeaderText
        {
            get => "■ Control";
        }

        protected virtual string targetBoxHeaderText
        {
            get => "■ Target";
        }

        /// <summary>
        /// ウィンドウ描画
        /// </summary>
        protected override void OnGUI()
        {
            base.OnGUI();

            config.CreateStyleIfNull();

            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                internalGuiState.IsDragAndDropRecursive = CustomUI.ToolbarWithToggle("■ Drag & Drop", "Recursive",
                    internalGuiState.IsDragAndDropRecursive);
                DrawDragAndDropBox();
                GUILayout.Space(2);
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                DrawControlBox();
                DrawTargetBox();
            }
        }

        /// <summary>
        /// 色の変更操作ボックス 表示
        /// </summary>
        private void DrawControlBox()
        {
            // using (new EditorGUILayout.VerticalScope(GUI.skin.box,  config.ControlBoxOptions))

            using (new EditorGUILayout.VerticalScope(config.ControlBoxOptions))
            {
                GUILayout.Space(4);

                // CustomUI.Toolbar("■ StartColor");
                // CustomUI.Toolbar(controlBoxHeaderText);
                // GUILayout.Space(2);

                // using (new EditorGUI.IndentLevelScope())
                {
                    // using (new EditorGUILayout.HorizontalScope(GUILayout.MinWidth(20), GUILayout.ExpandWidth(true)))
                    {
                        OnControlBoxGUI();
                    }
                }
            }
        }

        /// <summary>
        /// ControlBoxの描画
        /// </summary>
        protected virtual void OnControlBoxGUI()
        {
        }

        /// <summary>
        /// 変更対象の表示
        /// </summary>
        private void DrawTargetBox()
        {
            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                internalGuiState.TargetBoxScrollPosition =
                    EditorGUILayout.BeginScrollView(internalGuiState.TargetBoxScrollPosition);

                if (CustomUI.ToolbarWithButton(targetBoxHeaderText, "Clear"))
                {
                    internalGuiState.Clear();
                }

                GUILayout.Space(2);

                // using (new EditorGUI.IndentLevelScope())
                {
                    // EditorGUI.BeginDisabledGroup(true); 
                    for (int i = 0; i < internalGuiState.GetParticleSystemCount(); i++)
                    {
                        var ps = internalGuiState.GetTargetParticleSystem(i);
                        if (ps == null) continue;

                        EditorGUILayout.BeginHorizontal();
                        bool toggle = EditorGUILayout.Toggle("", internalGuiState.GetToggle(i), GUILayout.Width(18));
                        internalGuiState.SetToggle(i, toggle);
                        
                        EditorGUILayout.ObjectField(ps, typeof(ParticleSystem), true);
                        EditorGUILayout.EndHorizontal();

                        using (new EditorGUI.IndentLevelScope())
                        {
                            DrawParticleSystemParameter(i, ps);
                        }

                        GUILayout.Space(4);
                    }

                    // EditorGUI.EndDisabledGroup();
                }

                GUILayout.Space(2);
                EditorGUILayout.EndScrollView();
            }
        }

        protected virtual void DrawParticleSystemParameter(int index, ParticleSystem ps)
        {
        }


        /// <summary>
        /// Drag & Drop ボックス描画
        /// </summary>
        private void DrawDragAndDropBox()
        {
            if (toolConfig == null) return;
            
            CustomUI.DrawDragAndDropBox(
                "[Drag & Drop ParticleSystems Here]",
                toolConfig.GetFrameTexture(),
                style,
                (objs) =>
                {
                    if (internalGuiState.IsDragAndDropRecursive)
                    {
                        var particleSystems = objs.Select(obj => obj as GameObject)
                            .Where(go => go != null)
                            .Select(go => go.GetComponentsInChildren<ParticleSystem>())
                            .SelectMany(ps => ps)
                            .Where(ps => ps != null)
                            .Distinct()
                            .ToArray();
                        internalGuiState.SetParticleSystems(particleSystems);
                    }
                    else
                    {
                        var particleSystems = objs.Select(obj => obj as GameObject)
                            .Where(go => go != null)
                            .Select(go => go.GetComponent<ParticleSystem>())
                            .Where(ps => ps != null)
                            .ToArray();
                        internalGuiState.SetParticleSystems(particleSystems);
                    }
                });
        }
        
        /// <summary>
        /// ウィンドウの状態
        /// </summary>
        [Serializable]
        protected class VfxControllerGuiState
        {
            [SerializeField] private ParticleSystem[] targetParticleSystems = new ParticleSystem[0];
            [SerializeField] private bool[] isToggleOn = new bool[0];
            [SerializeField] private float hueShift = 0f;
            [SerializeField] private Color tagetRgb = Color.white;
            [SerializeField] private bool isDragAndDropRecursive = true; // ドラッグアンドドロップ時に子オブジェクトを取り出すか

            public Vector2 ControlBoxScrollPosition { get; set; } = new Vector2(0, 0);
            public Vector2 TargetBoxScrollPosition { get; set; } = new Vector2(0, 0);

            public bool IsDragAndDropRecursive
            {
                get => isDragAndDropRecursive;
                set => isDragAndDropRecursive = value;
            }

            public Color TagetRGB
            {
                get => tagetRgb;
                set => tagetRgb = value;
            }

            public float HueShift
            {
                get => hueShift;
                set => hueShift = value;
            }

            public ParticleSystem GetTargetParticleSystem(int index)
            {
                return targetParticleSystems[index];
            }

            public bool GetToggle(int index)
            {
                return isToggleOn[index];
            }

            public void SetToggle(int index, bool value)
            {
                isToggleOn[index] = value;
            }

            public IReadOnlyCollection<ParticleSystem> GetTargetParticleSystems()
            {
                return targetParticleSystems
                    .Where((ps, index) => isToggleOn[index])
                    .ToArray();
            }

            public int GetParticleSystemCount()
            {
                return targetParticleSystems.Length;
            }

            public void SetParticleSystems(ParticleSystem[] particleSystems)
            {
                targetParticleSystems = particleSystems;
                isToggleOn = new bool [particleSystems.Length];
                for (int i = 0; i < isToggleOn.Length; i++)
                {
                    isToggleOn[i] = true;
                }
            }

            public void Clear()
            {
                targetParticleSystems = new ParticleSystem[0];
            }
        }
    }
}