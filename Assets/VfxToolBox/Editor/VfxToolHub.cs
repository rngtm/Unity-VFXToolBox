using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using VfxToolBox.Shuriken.MaterialPresetAttacher;
using VfxToolBox.Shuriken.ShaderPresetGenerator;
using VfxToolBox.Shuriken.ShurikenColorChanger;
using VfxToolBox.Shuriken.ShurikenRandomizer;
using VfxToolBox.Shuriken.TimeChanger;
using VfxToolBox.ShurikenCopyPaste;
using VfxToolBox.ShurikenViewer;
using VfxToolBox.Utility;

namespace VfxToolBox
{
    public class VfxToolHub : EditorWindow
    {
        /// <summary>
        /// ウィンドウを開く
        /// </summary>
        [MenuItem(VfxMenuConfig.ToolHubMenuName, false, VfxMenuConfig.ToolHubMenuPriority)]
        public static void Open()
        {
            var window = GetWindow<VfxToolHub>();
            window.titleContent = VfxMenuConfig.ToolHubTitleContent;
            window.Show();
            
            PackageConfig.CheckInstall();
        }

        /// <summary>
        /// ウィンドウ描画
        /// </summary>
        private void OnGUI()
        {
            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                EditorGUILayout.LabelField("■ Create");
                DrawCreateButton();
            }
            
            // プリセット系ツール
            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                EditorGUILayout.LabelField("■ Presets");
                
                if (GUILayout.Button(VfxMenuConfig.MaterialPresetGeneratorButtonName))
                {
                    MaterialPresetGeneratorWindow.Open();
                }
                
                if (GUILayout.Button(VfxMenuConfig.MaterialPresetAttacherButtonName))
                {
                    MaterialPresetAttachWindow.Open();
                }
            }
            
            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                EditorGUILayout.LabelField("■ Editor");

                if (GUILayout.Button(VfxMenuConfig.TimeChangerButtonName))
                {
                    ShurikenTimeChangerWindow.Open();
                }
                
                if (GUILayout.Button(VfxMenuConfig.RandomizerButtonName))
                {
                    ShurikenRandomizeWindow.Open();
                }
                
                if (GUILayout.Button(VfxMenuConfig.ShurikenColorChangeButtonName))
                {
                    ShurikenColorChangeWindow.Open();
                }
            }
            
            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                EditorGUILayout.LabelField("■ Loop");
                if (GUILayout.Button(VfxMenuConfig.LoopButtonName))
                {
                    foreach (var ps in ParticleSystemUtility.GetSelectionParticleSystemRecursive())
                    {
                        var main = ps.main;
                        main.loop = true;
                    }
                }
                
                if (GUILayout.Button(VfxMenuConfig.UnloopButtonName))
                {
                    foreach (var ps in ParticleSystemUtility.GetSelectionParticleSystemRecursive())
                    {
                        var main = ps.main;
                        main.loop = false;
                    }
                }
            }


            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                EditorGUILayout.LabelField("■ Copy/Paste");
                if (GUILayout.Button(VfxMenuConfig.CopyPasteButtonName))
                {
                    ShurikenCopyPasteWindow.Open();
                }
            }

            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                EditorGUILayout.LabelField("■ Viewer");
                if (GUILayout.Button(VfxMenuConfig.ShurikenViewerButtonName))
                {
                    ShurikenViewerWindow.Open();
                }
            }
        }
        
        private void DrawUnGroupingButton()
        {
            if (GUILayout.Button("UnGrouping (Shift + Ctrl + G)"))
            {
                GroupingUtility.UngroupSelected();
            }
        }

        private void DrawGroupingButton()
        {
            if (GUILayout.Button("Grouping (Ctrl + G)"))
            {
                GroupingUtility.GroupSelected();
            }
        }

        /// <summary>
        /// 空のエフェクトを作成
        /// </summary>
        private static void DrawCreateButton()
        {
            if (GUILayout.Button("ルート作成"))
            {
                var ps = ParticleSystemUtility.CreateEmptyParticleSystem("Empty ParticleSystem");
                var childPs = ParticleSystemUtility.CreateEmptyParticleSystem("root");
                childPs.transform.SetParent(ps.transform);

                var childChildPs = new GameObject("empty", typeof(ParticleSystem));
                childChildPs.transform.SetParent(childPs.transform);

                if (Selection.activeTransform != null)
                {
                    ps.gameObject.transform.SetParent(Selection.activeTransform);
                }

                Selection.activeGameObject = childChildPs.gameObject;
            }
            
            if (GUILayout.Button("新規作成"))
            {
                var ps = ParticleSystemUtility.CreateParticleSystem("Empty ParticleSystem");
                if (Selection.activeTransform != null)
                {
                    ps.gameObject.transform.SetParent(Selection.activeTransform.parent);
                }

                Selection.activeGameObject = ps.gameObject;
            }
        }
    }
}