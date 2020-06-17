using UnityEditor;
using UnityEngine;

namespace VfxTools.ShurikenCopyPaste
{
    /** ********************************************************************************
    * @summary アイコンの管理
    ***********************************************************************************/
    public class ToolIcon
    {
        #region field
        private Texture2D prefabIconTexture = null;
        private Texture2D gameObjectIconTexture = null;
        #endregion

        #region prop
        public Texture2D PrefabIconTexture => prefabIconTexture;
        public Texture2D GameObjectIconTexture => gameObjectIconTexture;
        #endregion

        /** ********************************************************************************
        * @summary コンストラクタ
        ***********************************************************************************/
        public ToolIcon()
        {
            // Prefabアイコンをロード
            prefabIconTexture = EditorGUIUtility.Load("Prefab Icon") as Texture2D;
            gameObjectIconTexture = EditorGUIUtility.Load("GameObject Icon") as Texture2D;
        }
    }
}