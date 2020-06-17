using System;
using UnityEditor;
using UnityEngine;
using VfxTools.Utility;

namespace VfxTools.Shuriken.ShurikenColorChanger
{
    /// <summary>
    /// Shuriken 色変更ウィンドウ
    /// </summary>
    public class ShurikenColorChangeWindow : VfxControllerWindowBase
    {
        [SerializeField] private GuiState guiState = new GuiState();
        [SerializeField] private bool isRecursive = true;
        private Vector2 selectionScrollPosition = new Vector2();

        private readonly Config config = new Config();
        // protected override string controlBoxHeaderText { get => "■ Control"; }

        [MenuItem(VfxMenuConfig.ShurikenColorChangeMenuName)]
        public static void Open()
        {
            var window = GetWindow<ShurikenColorChangeWindow>();
            window.titleContent = VfxMenuConfig.ShurikenColorChangeTitleContent;
            window.Show();
        }

        protected override void OnControlBoxGUI()
        {
            base.OnControlBoxGUI();

            config.CreateStyleIfNull();

            // GUILayout.Space(2);

            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                guiState.IsAutoApplyRGB = CustomUI.ToolbarWithToggle("■ Start Color", "Auto", guiState.IsAutoApplyRGB);

                using (new EditorGUI.IndentLevelScope())
                {
                    GUILayout.Space(2);
                    using (new EditorGUILayout.HorizontalScope(GUILayout.MinWidth(20), GUILayout.ExpandWidth(true)))
                    {
                        EditorGUILayout.LabelField("RGB", config.LabelFieldOptions);

                        EditorGUI.BeginChangeCheck();
                        guiState.TagetRGB = EditorGUILayout.ColorField(new GUIContent(""), guiState.TagetRGB, true,
                            false, false, config.ValueFieldOptions);
                        if (EditorGUI.EndChangeCheck() && guiState.IsAutoApplyRGB)
                        {
                            ApplyRGB(guiState.TagetRGB);
                        }

                        GUILayout.Space(4);
                        if (GUILayout.Button("Apply", config.ButtonStyle, config.ApplyButtonOptions))
                        {
                            ApplyRGB(guiState.TagetRGB);
                        }
                    }
                }
            }
        }

        protected override void DrawParticleSystemParameter(int index, ParticleSystem ps)
        {
            base.DrawParticleSystemParameter(index, ps);

            var main = ps.main;
            var startColor = main.startColor;

            EditorGUI.BeginChangeCheck();
            switch (startColor.mode)
            {
                case ParticleSystemGradientMode.Color:
                    startColor.color = EditorGUILayout.ColorField("Color", startColor.color);
                    break;
                case ParticleSystemGradientMode.Gradient:
                    if (startColor.gradient != null)
                        EditorGUILayout.GradientField("Gradient", startColor.gradient);
                    break;
                case ParticleSystemGradientMode.TwoColors:
                    startColor.colorMax = EditorGUILayout.ColorField("Color(Max)", startColor.colorMax);
                    startColor.colorMin = EditorGUILayout.ColorField("Color(Min)", startColor.colorMin);
                    break;
                case ParticleSystemGradientMode.TwoGradients:
                    if (startColor.gradientMin != null)
                        EditorGUILayout.GradientField("Gradient(Min)", startColor.gradientMin);
                    if (startColor.gradientMax != null)
                        EditorGUILayout.GradientField("Gradient(Max)", startColor.gradientMax);
                    break;
                case ParticleSystemGradientMode.RandomColor:
                    EditorGUILayout.GradientField("Random Color", startColor.gradient);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RegisterCompleteObjectUndo(ps, "Change StartColor");
                main.startColor = startColor;
            }
        }

        /// <summary>
        /// RGBの適用
        /// </summary>
        private void ApplyRGB(Color rgb)
        {
            if (base.internalGuiState.GetParticleSystemCount() == 0) return;
            
            foreach (var ps in base.internalGuiState.GetTargetParticleSystems())
            {
                if (ps == null) continue;
                
                Undo.RegisterCompleteObjectUndo(ps, "Change StartColor");

                var main = ps.main;
                var startColor = main.startColor;
                switch (startColor.mode)
                {
                    case ParticleSystemGradientMode.Color:
                    {
                        startColor.color = new Color(rgb.r, rgb.g, rgb.b, startColor.color.a);
                        main.startColor = startColor;
                        break;
                    }
                    case ParticleSystemGradientMode.Gradient:
                    {
                        break;
                    }
                    case ParticleSystemGradientMode.TwoColors:
                    {
                        startColor.colorMin = new Color(rgb.r, rgb.g, rgb.b, startColor.colorMin.a);
                        startColor.colorMax = new Color(rgb.r, rgb.g, rgb.b, startColor.colorMax.a);
                        main.startColor = startColor;
                        break;
                    }
                    case ParticleSystemGradientMode.TwoGradients:
                    {
                        break;
                    }
                    case ParticleSystemGradientMode.RandomColor:
                    {
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                main.startColor = startColor;
            }
        }

        /// <summary>
        /// ウィンドウの状態
        /// </summary>
        [Serializable]
        private class GuiState
        {
            [SerializeField] private float hueShift = 0f;
            [SerializeField] private Color tagetRgb = Color.white;

            // RGB変更時に自動で色を反映する
            [SerializeField] private bool isAutoApplyRgb = false;

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

            public bool IsAutoApplyRGB
            {
                get => isAutoApplyRgb;
                set => isAutoApplyRgb = value;
            }

            // public IReadOnlyCollection<ParticleSystem> GetTargetParticleSystems()
            // {
            //     return targetParticleSystems;
            // }
            //
            // public void SetParticleSystems(ParticleSystem[] particleSystems)
            // {
            //     targetParticleSystems = particleSystems;
            // }
            //
            // public void Clear()
            // {
            //     targetParticleSystems = new ParticleSystem[0];
            // }
        }
    }
}