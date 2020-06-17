using UnityEditor;

namespace VfxTools
{ 
    /// <summary>
    /// VFXウィンドウの基本となるウィンドウ
    /// </summary>
    public abstract class VfxWindowBase : EditorWindow
    {
        private readonly int repaintInterval = 8;
        private int frameCount = 0;
        private bool loadFailed = false;
        protected VfxToolConfig toolConfig { get; private set; }
        protected VfxToolStyle style { get; private set; }

        protected virtual void Update()
        {
            CheckRepaint();
            LoadConfigIfNull();
        }
        
        protected virtual void OnGUI()
        {
            CreateStyleIfNull();
        }

        private void CheckRepaint()
        {
            if (frameCount == repaintInterval)
            {
                frameCount = 0;
                Repaint();
            }
            else
            {
                frameCount++;
            }
        }

        private void LoadConfigIfNull()
        {
            if (loadFailed) return;

            if (toolConfig == null)
            {
                toolConfig = VfxToolConfig.Get();
                loadFailed = toolConfig == null;
            }
        }

        private void CreateStyleIfNull()
        {
            style = style ?? new VfxToolStyle();
        }
    }
}