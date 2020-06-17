using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEditor.VersionControl;
using UnityEditorInternal;
using UnityEngine;
using VfxTools.Shuriken.MaterialPresetAttacher;
using VfxTools.Utility;

namespace VfxTools.Shuriken.ShaderPresetGenerator
{
    /// <summary>
    /// CustomData, Vertex Streamsを一括設定する, 
    /// </summary>
    public class MaterialPresetGeneratorWindow : VfxWindowBase
    {
        private const int RepaintInterval = 8;
        [SerializeField] private GuiState guiState = new GuiState();
        [SerializeField] private Vector2 windowScrollPosition = new Vector2();
        [SerializeField] private Vector2 generatorScrollPosition = new Vector2();
        [SerializeField] private Vector2 materialScrollPosition = new Vector2();
        [SerializeReference] private bool isOpenGenerator = false;

        /// <summary>
        /// ウィンドウを開く
        /// </summary>
        [UnityEditor.MenuItem(VfxMenuConfig.MaterialPresetGeneratorMenuName, false, VfxMenuConfig.MaterialPresetGeneratorMenuPriority)]
        public static void Open()
        {
            var window = GetWindow<MaterialPresetGeneratorWindow>();
            window.titleContent = VfxMenuConfig.MaterialPresetGeneratorTitleContent;
            window.Show();
        }

        /// <summary>
        /// ウィンドウ描画処理
        /// </summary>
        protected override void OnGUI()
        {
            base.OnGUI();

            DrawHeader();
            windowScrollPosition = EditorGUILayout.BeginScrollView(windowScrollPosition);


            DrawMaterialBox();
            DrawPresetGenerateBox();
            EditorGUILayout.EndScrollView();
        }

        /// <summary>
        /// マテリアルボックス表示
        /// </summary>
        private void DrawMaterialBox()
        {
            // D&Dされたマテリアル プリセット作成GUI
            using (new EditorGUILayout.VerticalScope(GUI.skin.box, GUILayout.Height(124)))
            {
                materialScrollPosition = EditorGUILayout.BeginScrollView(materialScrollPosition);
                EditorGUILayout.LabelField("■ Materials");
                DrawDragAndDropBox();

                using (new EditorGUILayout.VerticalScope(GUI.skin.box))
                {
                    generatorScrollPosition = EditorGUILayout.BeginScrollView(generatorScrollPosition);
                    foreach (var material in guiState.GetShaders())
                    {
                        DrawShaderGUI(material);
                    }
                    EditorGUILayout.EndScrollView();
                }
                EditorGUILayout.EndScrollView();
            }
        }

        /// <summary>
        /// プリセット作成ボックス
        /// </summary>
        private void DrawPresetGenerateBox()
        {
            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                EditorGUILayout.LabelField("■ Preset Generator");

                using (new EditorGUI.IndentLevelScope())
                {
                    guiState.ExportDirectory
                        = EditorGUILayout.TextField("Export Directory", guiState.ExportDirectory);
                }
                
                DrawPresetCreateButton();
            }
            
        }

        /// <summary>
        /// プリセット作成ボタン
        /// </summary>
        private void DrawPresetCreateButton()
        {
            // すべてのマテリアル作成
            EditorGUI.BeginDisabledGroup(guiState.Empty);
            if (GUILayout.Button("Create Presets", style.ExportButtonStyle, MyStyle.ExportButtonOption))
            {
                foreach (var material in guiState.GetShaders())
                {
                    CreateShaderPreset(material, guiState);
                }

                // フォルダ強調表示
                // var folder =
                //     AssetDatabase.LoadAssetAtPath(generatorState.ExportDirectory, typeof(DefaultAsset));
                // EditorGUIUtility.PingObject(folder);
            }

            EditorGUI.EndDisabledGroup();
        }

        /// <summary>
        /// マテリアル作成GUIの表示
        /// </summary>
        private void DrawShaderGUI(Shader shader)
        {
            EditorGUI.BeginDisabledGroup(true);

            // マテリアル
            EditorGUILayout.ObjectField(shader, typeof(Material), false);

            EditorGUI.EndDisabledGroup();
        }
        
        /// <summary>
        /// マテリアルプリセット作成
        /// </summary>
        private static void CreateShaderPreset(Shader shader, GuiState state, Action<ShaderPresetData> onCompleted = null)
        {
            var presetName = $"Preset_{shader.name.Replace("/", "-")}";
            var path = Path.Combine(state.ExportDirectory, presetName) + ".asset";
            var instance = ScriptableObject.CreateInstance<ShaderPresetData>();
            instance.SetShader(shader);
            AssetDatabase.CreateAsset(instance, path);

            if (onCompleted != null)
            {
                var asset = AssetDatabase.LoadAssetAtPath(path, typeof(ShaderPresetData)) as ShaderPresetData;
                if (asset != null)
                {
                    onCompleted.Invoke(asset);
                    // EditorGUIUtility.PingObject(asset);
                }
            }
        }

        /// <summary>
        /// ドラッグアンドドロップボックス
        /// </summary>
        private void DrawDragAndDropBox()
        {
            CustomUI.DrawDragAndDropBox(
                "[Drag & Drop Shaders Here]", 
                VfxToolConfig.Get().GetFrameTexture(), style, objs =>
                {
                    var shaders = DragAndDrop.objectReferences
                        .Select(o => o as Shader)
                        .Where(s => s != null)
                        .ToArray();
                    guiState.SetShaders(shaders);
                });
        }

        /// <summary>
        /// ヘッダ描画
        /// </summary>
        private void DrawHeader()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                
                if (GUILayout.Button("Open Attacher", EditorStyles.toolbarButton))
                {
                    MaterialPresetAttachWindow.Open();
                }

                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Config", EditorStyles.toolbarButton))
                {
                    // EditorGUIUtility.PingObject(toolConfig);
                    Selection.activeObject = toolConfig;
                }
            }
        }

        /// <summary>
        /// プリセットの選択
        /// </summary>
        private void SelectPreset(ShaderPresetData preset)
        {
            Selection.activeObject = preset;
        }

        /// <summary>
        /// 選択情報の表示
        /// </summary>
        private void DrawSelectionGameObjectBox()
        {
            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                EditorGUILayout.LabelField("■ Selection");
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.ObjectField(VfxToolState.LastSelectionGameObject, typeof(GameObject), true);
                EditorGUI.EndDisabledGroup();
            }
        }

        [Serializable]
        public class GuiState
        {
            // プリセットのエクスポート先ディレクトリ
            [SerializeField] private string exportDirectory = VfxToolConfig.DefaultPresetExportDirectory;

            private Shader[] targetShaders = new Shader[0];

            public string ExportDirectory
            {
                get => exportDirectory;
                set => exportDirectory = value;
            }

            public bool Empty => targetShaders.Length == 0;

            public void SetShaders(Shader[] shaders)
            {
                targetShaders = shaders;
            }

            public Shader GetMaterial(int index)
            {
                return targetShaders[index];
            }

            public IReadOnlyCollection<Shader> GetShaders()
            {
                return targetShaders;
            }

            public void Clear()
            {
                targetShaders = new Shader[0];
            }
        }
    }
}