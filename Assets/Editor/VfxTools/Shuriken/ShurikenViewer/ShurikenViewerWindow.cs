namespace VfxTools.ShurikenViewer
{
    using UnityEngine;
    using UnityEditor.IMGUI.Controls;
    using UnityEditor;
    using System.Collections.Generic;
    using System.Linq;

    // TreeViewを表示するWindow
    class ShurikenViewerWindow : VfxWindowBase
    {
        // 解析対象のエフェクト
        [SerializeField] private ParticleSystem _inputParticleSystem = null;

        // Stateはシリアライズする（Unity再起動しても状態を保持するため）
        [SerializeField] private TreeViewState _treeViewState = null;

        [SerializeField] private EffectColumnHeader header;

        private string[] assetPaths;

        private EffectTreeView _treeView;
        private SearchField _searchField;


        /// <summary>
        /// ウィンドウを開く
        /// </summary>
        [MenuItem(VfxMenuConfig.ShurikenViewerMenuName, false, VfxMenuConfig.ShurikenViewerMenuPriority)]
        public static void Open()
        {
            var window = GetWindow<ShurikenViewerWindow>(ObjectNames.NicifyVariableName(typeof(ShurikenViewerWindow).Name));
            window.titleContent = VfxMenuConfig.ShurikenViewerTitleContent;
        }

        private void OnEnable()
        {
            UpdateTreeView();
        }

        /// <summary>
        /// ウィンドウ描画
        /// </summary>
        protected override void OnGUI()
        {
            base.OnGUI();
            
            //using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            //{
            //    GUILayout.FlexibleSpace();
            //    GUILayout.Button("Refresh", EditorStyles.toolbarButton);
            //}

            EditorGUI.BeginChangeCheck();
            _inputParticleSystem = EditorGUILayout.ObjectField(_inputParticleSystem, typeof(ParticleSystem), true) as ParticleSystem;
            if (EditorGUI.EndChangeCheck())
            {
                UpdateTreeView();
            }

            // TreeViewを描画
            var rect = EditorGUILayout.GetControlRect(false, GUILayout.ExpandHeight(true));
            _treeView.OnGUI(rect);
        }

        /// <summary>
        /// TreeView更新
        /// </summary>
        private void UpdateTreeView()
        {
            _treeViewState = _treeViewState ?? new TreeViewState();
            _treeView = _treeView ?? new EffectTreeView(_treeViewState);

            // 親子関係を構築したモデルを作成
            // IDは被らないようにする
            var currentId = 0;
            var rootElementList = new List<EffectTreeElement>();
            //foreach (var prefabData in prefabDataArray)
            {
                // ルートオブジェクト
                var rootElement = new EffectTreeElement
                {
                    Id = currentId++,
                    Name = _inputParticleSystem == null ? "" : _inputParticleSystem.name,
                    EffectData = new EffectData { ParticleSystem = _inputParticleSystem },
                };
                rootElementList.Add(rootElement);

                if (_inputParticleSystem != null)
                {
                    foreach (ParticleSystem child in _inputParticleSystem.gameObject.GetComponentsInChildren<ParticleSystem>())
                    {
                        rootElement.AddChild(new EffectTreeElement
                        {
                            Id = currentId++,
                            Name = child.name,
                            EffectData = new EffectData { ParticleSystem = child },
                        });
                    }
                }
            }

            // TreeViewの更新
            _treeView.Setup(rootElementList.ToArray());
        }

        /// <summary>
        /// 指定したディレクトリからアセット一覧を取得
        /// </summary>
        public static IEnumerable<string> GetAssetPaths(string[] directories, string filter = "")
        {
            // 最後にスラッシュがあったら消す
            for (int i = 0; i < directories.Length; i++)
            {
                var directory = directories[i];
                if (directory[directory.Length - 1] == '/')
                {
                    directory = directory.Substring(0, directory.Length - 1);
                }
            }

            // リストアップ
            var paths = AssetDatabase.FindAssets(filter, directories)
                .Select(x => AssetDatabase.GUIDToAssetPath(x))
                .Select(x => AssetImporter.GetAtPath(x)) // アセットを取得
                .Select(x => x.assetPath)
                .Where(x => !string.IsNullOrEmpty(x)) // アセット名がないやつを排除
                .OrderBy(x => x); // 並び替え

            return paths;
        }
    }

    [System.Serializable]
    public class EffectData
    {
        public ParticleSystem ParticleSystem;
    }
}