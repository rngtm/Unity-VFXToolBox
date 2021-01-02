using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using VfxToolBox.Utility;

namespace VfxToolBox.Shuriken.TimeChanger
{
    using UnityEditor;

    public class ShurikenTimeChangerWindow : VfxControllerWindowBase
    {
        [SerializeField] private GuiState guiState = new GuiState();
        private readonly TimeChangeToolConfig timeChangeToolConfig = new TimeChangeToolConfig();

        protected override string controlBoxHeaderText
        {
            get => "■ StartDelay";
        }

        [MenuItem(VfxMenuConfig.TimeChangerMenuName, false, VfxMenuConfig.TimeChangerMenuPriority)]
        public static void Open()
        {
            var window = GetWindow<ShurikenTimeChangerWindow>();
            window.titleContent = VfxMenuConfig.TimeChangerTitleContent;
            window.Show();
        }

        /// <summary>
        /// ParticleSystemのstartDelayを表示
        /// </summary>
        protected override void DrawParticleSystemParameter(int index, ParticleSystem ps)
        {
            base.DrawParticleSystemParameter(index, ps);
            if (ps == null) return;

            using (new EditorGUI.DisabledScope(true))
            {
                // Duration
                GUILayout.Space(1);
                EditorGUILayout.FloatField("Duration", ps.main.duration);

                // Start Lifetime
                GUILayout.Space(1);
                var startLifetime = ps.main.startLifetime;
                // EditorGUILayout.FloatField("Start Lifetime", ps.main.startLifetime);
                DrawMinMaxCurve("Start Lifetime", startLifetime);

                // Start Delay
                GUILayout.Space(1);
                var startDelay = ps.main.startDelay;
                DrawMinMaxCurve("Start Delay", startDelay);
            }
        }

        private static void DrawMinMaxCurve(string label, ParticleSystem.MinMaxCurve startDelay)
        {
            switch (startDelay.mode)
            {
                case ParticleSystemCurveMode.Constant:
                {
                    EditorGUILayout.FloatField($"{label}(Constant)", startDelay.constant);
                    break;
                }
                case ParticleSystemCurveMode.Curve:
                    break;
                case ParticleSystemCurveMode.TwoCurves:
                    break;
                case ParticleSystemCurveMode.TwoConstants:
                {
                    EditorGUILayout.FloatField($"{label}(Constant Min)", startDelay.constantMin);
                    EditorGUILayout.FloatField($"{label}(Constant Max)", startDelay.constantMax);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// ControlBoxの表示
        /// </summary>
        protected override void OnControlBoxGUI()
        {
            base.OnControlBoxGUI();

            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                // guiState.IsOpenDuration = CustomUI.ToolbarWithFoldout("■ Duration", guiState.IsOpenDuration);
                guiState.IsOpenDuration = CustomUI.ToolbarWithFoldout("Duration", guiState.IsOpenDuration);
                if (guiState.IsOpenDuration)
                {
                    GUILayout.Space(2);
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        GUILayout.Space(2);
                        guiState.InputDuration = EditorGUILayout.FloatField(guiState.InputDuration);

                        if (GUILayout.Button("Apply"))
                        {
                            ApplyDuration();
                        }
                    }
                }
            }


            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                guiState.IsOpenStartLifetime =
                    CustomUI.ToolbarWithFoldout("Start Lifetime", guiState.IsOpenStartLifetime);

                if (guiState.IsOpenStartLifetime)
                {
                    GUILayout.Space(2);
                    DrawStartLifetimeButtons(); // ボタン表示
                }
            }

            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                guiState.IsOpenStartDelay = CustomUI.ToolbarWithFoldout("Star Delay", guiState.IsOpenStartDelay);

                if (guiState.IsOpenStartDelay)
                {
                    GUILayout.Space(2);
                    DrawDelayButtons(); // ボタン表示
                }
            }
        }

        /// <summary>
        /// Durationの適用
        /// </summary>
        private void ApplyDuration()
        {
            foreach (var ps in base.internalGuiState.GetTargetParticleSystems())
            {
                var main = ps.main;

                bool needActivate = ps.gameObject.activeSelf;

                Undo.RegisterCompleteObjectUndo(ps, "Change StartDelay");
                ps.gameObject.SetActive(false);
                main.duration = guiState.InputDuration;

                if (needActivate)
                {
                    ps.gameObject.SetActive(true);
                }

                ps.Play();
            }
        }


        /// <summary>
        /// StartDelayの変更ボタンの描画
        /// </summary>
        private void DrawDelayButtons()
        {
            using (new EditorGUILayout.VerticalScope(style.StartDelayButtonBoxStyle))
            {
                foreach (var dataTable in timeChangeToolConfig.DelayButtonDataTableArray)
                {
                    DrawButtonRow(dataTable, ApplyStartDelay);
                }
            }
        }

        /// <summary>
        /// Lifetimeの変更ボタンの描画
        /// </summary>
        private void DrawStartLifetimeButtons()
        {
            using (new EditorGUILayout.VerticalScope(style.StartDelayButtonBoxStyle))
            {
                foreach (var dataTable in timeChangeToolConfig.LifetimeButtonDataTableArray)
                {
                    DrawButtonRow(dataTable, ApplyStartLifetime);
                }
            }
        }

        /// <summary>
        /// ボタン列の描画
        /// </summary>
        private void DrawButtonRow(ButtonData[] dataArray, Action<ButtonData> onClickAction)
        {
            using (new EditorGUILayout.HorizontalScope(style.StartDelayButtonRowBoxStyle))
            {
                foreach (var data in dataArray)
                {
                    if (GUILayout.Button(data.ButtonLabel, style.StartDelayButtonStyle))
                    {
                        onClickAction.Invoke(data);
                        // ApplyStartDelay(data);
                    }
                }
            }
        }

        private void ChangeStartDelay(ParticleSystem ps, Func<float, float> delayConvert)
        {
            Undo.RegisterCompleteObjectUndo(ps, "ChangeStartDelay");

            bool needReplay = ps.isPlaying;
            if (ps.isPlaying)
            {
                ps.Stop();
            }

            var main = ps.main;
            var startDelay = main.startDelay;
            switch (startDelay.mode)
            {
                case ParticleSystemCurveMode.Constant:
                    startDelay.constant = delayConvert.Invoke(startDelay.constant);
                    break;
                case ParticleSystemCurveMode.Curve:
                    break;
                case ParticleSystemCurveMode.TwoCurves:
                    break;
                case ParticleSystemCurveMode.TwoConstants:
                    startDelay.constantMin = delayConvert.Invoke(startDelay.constantMin);
                    startDelay.constantMax = delayConvert.Invoke(startDelay.constantMax);

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            main.startDelay = startDelay;

            if (needReplay)
            {
                ps.Play();
            }

            EditorSceneManager.MarkSceneDirty(ps.gameObject.scene);
        }

        private void ChangeStartLifetime(ParticleSystem ps, Func<float, float> delayConvert)
        {
            Undo.RegisterCompleteObjectUndo(ps, "ChangeStartLifetime");

            bool needReplay = ps.isPlaying;
            if (ps.isPlaying)
            {
                ps.Stop();
            }

            var main = ps.main;
            var startLifetime = main.startLifetime;
            switch (startLifetime.mode)
            {
                case ParticleSystemCurveMode.Constant:
                    startLifetime.constant = delayConvert.Invoke(startLifetime.constant);
                    break;
                case ParticleSystemCurveMode.Curve:
                    break;
                case ParticleSystemCurveMode.TwoCurves:
                    break;
                case ParticleSystemCurveMode.TwoConstants:
                    startLifetime.constantMin = delayConvert.Invoke(startLifetime.constantMin);
                    startLifetime.constantMax = delayConvert.Invoke(startLifetime.constantMax);

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            main.startLifetime = startLifetime;

            if (needReplay)
            {
                ps.Play();
            }

            EditorSceneManager.MarkSceneDirty(ps.gameObject.scene);
        }


        /// <summary>
        /// StartDelayの変更
        /// </summary>
        private void ApplyStartDelay(ButtonData data)
        {
            foreach (var ps in base.internalGuiState.GetTargetParticleSystems())
            {
                ChangeStartDelay(ps, data.Callback);
            }
        }

        /// <summary>
        /// StartDelayの変更
        /// </summary>
        private void ApplyStartLifetime(ButtonData data)
        {
            foreach (var ps in base.internalGuiState.GetTargetParticleSystems())
            {
                ChangeStartLifetime(ps, data.Callback);
            }
        }

        /// <summary>
        /// スクロールの状態
        /// </summary>
        [Serializable]
        private class GuiState
        {
            [SerializeField] private float inputDuration = 5f;

            public float InputDuration
            {
                get => inputDuration;
                set => inputDuration = value;
            }

            public bool IsOpenDuration = true;
            public bool IsOpenStartLifetime = true;
            public bool IsOpenStartDelay = true;

            public Vector2 TargetUiScrollPosition { get; set; } = new Vector2();
            public Vector2 BeforeAfterScrollPosition { get; set; } = new Vector2();
        }
    }
}