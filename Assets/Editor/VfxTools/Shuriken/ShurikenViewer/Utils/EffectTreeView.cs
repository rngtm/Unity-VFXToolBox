namespace VfxTools.ShurikenViewer
{
    using UnityEditor.IMGUI.Controls;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEditor;

    // 抽象クラスTreeViewを継承したクラスを作る
    public class EffectTreeView : TreeView
    {
        private EffectTreeElement[] _baseElements;
        private EffectTreeElement[] _displayElements;
        private Texture2D prefabIconTexture;
        private Texture2D gameObjectIconTexture;

        static MultiColumnHeaderState.Column[] headerColumns => fieldAccessorArray
            .Select(field => new EffectHeaderStateColumn(field.Title, field.Width))
            .Prepend(new EffectHeaderStateColumn("Name", 140f))
            .ToArray();

        static readonly RowFieldAccessor[] fieldAccessorArray = new RowFieldAccessor[]
        {
            new RowFieldAccessor("Sorting Fudge", 100f,
                (rect, ps) => EditorGUI.LabelField(rect, ps.GetComponent<ParticleSystemRenderer>().sortingFudge.ToString())
            ),
            new RowFieldAccessor("Material", 160f,
                (rect, ps) => EditorGUI.ObjectField(rect, ps.GetComponent<ParticleSystemRenderer>().sharedMaterial, typeof(Material), false )
            ),
            new RowFieldAccessor("Main Texture", 170f,
                (rect, ps) => EditorGUI.ObjectField(rect, ps.GetComponent<ParticleSystemRenderer>().sharedMaterial?.mainTexture, typeof(Texture2D), false )
            ),
            new RowFieldAccessor("Shader", 320f,
                (rect, ps) => EditorGUI.ObjectField(rect, ps.GetComponent<ParticleSystemRenderer>().sharedMaterial?.shader, typeof(Shader), false )
            ),
            new RowFieldAccessor("Mesh", 130f,
                (rect, ps) => EditorGUI.ObjectField(rect, ps.GetComponent<ParticleSystemRenderer>().mesh, typeof(Mesh), false )
            ),
            //new RowFieldAccessor("Renderer\n sortingOrder", 100f,
            //    (rect, ps) => EditorGUI.LabelField(rect, ps.GetComponent<ParticleSystemRenderer>().sortingOrder.ToString())
            //),
            //new RowFieldAccessor("Renderer\n sortingLayerID", 100f,
            //    (rect, ps) => EditorGUI.LabelField(rect, ps.GetComponent<ParticleSystemRenderer>().sortingLayerID.ToString())
            //),
            //new RowFieldAccessor("Renderer\n sortingLayerName", 120f,
            //    (rect, ps) => EditorGUI.LabelField(rect, ps.GetComponent<ParticleSystemRenderer>().sortingLayerName.ToString())
            //),
        };

        public EffectTreeView(TreeViewState state)
            : base(state, new EffectColumnHeader(new EffectHeaderState(headerColumns)))
        {
            showAlternatingRowBackgrounds = true; // 背景のシマシマを表示
            showBorder = true; // 境界線を表示
        }

        /// <summary>
        /// TreeViewセットアップ
        /// </summary>
        public void Setup(EffectTreeElement[] baseElements)
        {
            // モデルを入れて
            _baseElements = baseElements;

            List<EffectTreeElement> list = new List<EffectTreeElement>();
            foreach (var baseElement in baseElements)
            {
                list.Add(baseElement);
                foreach (var child in baseElement.Children)
                {
                    list.Add(child);
                }
            }
            _displayElements = list.ToArray();

            if (_baseElements != null)
            {
                // Reload()で更新（BuildRootが呼ばれる）
                Reload();

                foreach (var element in baseElements)
                {
                    if (element.Parent == null)
                    {
                        SetExpanded(element.Id, true); // 折り畳みを開く
                    }
                }
            }
        }

        // ルートとなるTreeViewItemを作って返す
        // Reloadが呼ばれるたびに呼ばれる
        protected override TreeViewItem BuildRoot()
        {
            // RootのItemはdepth = -1として定義する
            var root = new TreeViewItem { id = -1, depth = -1, displayName = "Root" };

            //　モデルからTreeViewItemの親子関係を構築
            var elements = new List<TreeViewItem>();
            foreach (EffectTreeElement baseElement in _baseElements)
            {
                var baseItem = CreateTreeViewItem(baseElement);
                root.AddChild(baseItem);
                AddChildrenRecursive(baseElement, baseItem);
            }

            // 親子関係に基づいてDepthを自動設定するメソッド
            SetupDepthsFromParentsAndChildren(root);

            return root;
        }

        /// <summary>
        /// モデルとItemから再帰的に子Itemを作成・追加する
        /// </summary>
        private void AddChildrenRecursive(EffectTreeElement model, TreeViewItem item)
        {
            foreach (var childModel in model.Children)
            {
                var childItem = CreateTreeViewItem(childModel);
                item.AddChild(childItem);
                AddChildrenRecursive(childModel, childItem);
            }
        }

        /// <summary>
        /// ExampleTreeElementからTreeViewItemを作成する
        /// </summary>
        private TreeViewItem CreateTreeViewItem(EffectTreeElement model)
        {
            return new TreeViewItem { id = model.Id, displayName = model.Name };
        }

        /// <summary>
        /// 列の行を描画
        /// </summary>
        private void DrawRowColumn(RowGUIArgs args, Rect rect, int columnIndex)
        {
            var displayElement = _displayElements[args.item.id]; // 表示する列を取得
            if (displayElement.EffectData.ParticleSystem == null) { return; }

            GUIStyle labelStyle = EditorStyles.label;
            switch (columnIndex)
            {
                case 0:
                    float indentSize = 16f + 16f * args.item.depth;
                    rect.x += indentSize;

                    var iconTexture = gameObjectIconTexture;

                    // アイコンを描画する
                    Rect toggleRect = rect;
                    toggleRect.y += 2f;
                    toggleRect.size = new Vector2(16f, 16f);
                    GUI.DrawTexture(toggleRect, iconTexture);

                    // テキストを描画する
                    Rect labelRect = new Rect(rect);
                    labelRect.x += toggleRect.width;
                    EditorGUI.LabelField(labelRect, args.label);
                    break;

                default:
                    {
                        if (args.item.depth == 0) { break; } // 入れ子の外側だったら表示しない

                        var effectData = displayElement.EffectData;
                        if (effectData != null)
                        {
                            //using (new EditorGUI.DisabledGroupScope(true))
                            {
                                if (effectData.ParticleSystem != null)
                                {
                                    fieldAccessorArray[columnIndex - 1].ObjectField(rect, effectData.ParticleSystem);
                                }
                            }
                        }
                    }
                    break;
            }
        }

        /** ********************************************************************************
         * @summary TreeViewの列の描画
         ***********************************************************************************/
        protected override void RowGUI(RowGUIArgs args)
        {
            if (prefabIconTexture == null)
            {
                // Prefabアイコンをロード
                prefabIconTexture = EditorGUIUtility.Load("Prefab Icon") as Texture2D;
            }

            gameObjectIconTexture = gameObjectIconTexture ?? EditorGUIUtility.Load("GameObject Icon") as Texture2D;

            // TreeView 各列の描画
            for (var visibleColumnIndex = 0; visibleColumnIndex < args.GetNumVisibleColumns(); visibleColumnIndex++)
            {
                var rect = args.GetCellRect(visibleColumnIndex);
                var columnIndex = args.GetColumn(visibleColumnIndex);
                var labelStyle = args.selected ? EditorStyles.whiteLabel : EditorStyles.label;
                labelStyle.alignment = TextAnchor.MiddleLeft;

                // 列の描画
                DrawRowColumn(args, rect, columnIndex);
            }
        }

        ///// <summary>
        ///// ダブルクリック時に呼ばれる
        ///// </summary>
        //protected override void DoubleClickedItem(int id)
        //{
        //}

        /// <summary>
        /// 選択が変化した時に呼ばれる
        /// </summary>
        protected override void SelectionChanged(IList<int> selectedIds)
        {
            base.SelectionChanged(selectedIds);
            if (selectedIds.Count == 0) { return; }

            int id = selectedIds.ElementAt(0);
            if (id >= 0)
            {
                var element = _displayElements[id];
                Selection.activeObject = element.EffectData.ParticleSystem; // ParticleSystemを選択
            }
        }

        class RowFieldAccessor
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
}