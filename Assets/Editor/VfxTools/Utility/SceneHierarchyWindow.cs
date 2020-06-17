using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace VfxTools.Utility
{
    public static class SceneHierarchyWindow
    {
        private static EditorWindow window = null;
        private static BindingFlags flags = BindingFlags.Public
                                            | BindingFlags.NonPublic
                                            | BindingFlags.Instance
                                            | BindingFlags.Static;

        private static EditorWindow GetWindow()
        {
            if (window != null) return window;
            
            var windowType = sceneHierarchyWindowType.Value;
            window = EditorWindow.GetWindow(windowType);
            return window;
        }
        

        private static Lazy<Type> sceneHierarchyWindowType = new Lazy<Type>(() =>
        {
            var type = Assembly.Load("UnityEditor.dll").GetType("UnityEditor.SceneHierarchyWindow");
            return type;
        });

        private static object sceneHierarchy
        {
            get
            {
                var windowType = sceneHierarchyWindowType.Value;
                return windowType.GetField("m_SceneHierarchy", flags)?
                    .GetValue(GetWindow());
            }
        }
        //
        public static void SetSelection(int[] instanceIDs)
        {
            var treeView = sceneHierarchy?.GetType().GetProperty("treeView", flags).GetValue(sceneHierarchy);
            var method_TreeViewSelectionChanged = treeView.GetType()
                .GetMethod("SetSelection", flags, null, new Type[] {typeof(int[]), typeof(bool)}, null);
            method_TreeViewSelectionChanged.Invoke(treeView, new object[] { instanceIDs, false});

            // int[] instanceIDs = treeView.GetRowIDs();
            // treeView.SetSelection(instanceIDs, false);
            // TreeViewSelectionChanged(instanceIDs);
        }
        // public static void SetSelection(int[] instanceIDs)
        // {
        //     var window = EditorWindow.GetWindow(sceneHierarchyWindowType.Value);
        //
        //     var treeView = sceneHierarchy.GetType()
        //         .GetProperty("treeView", flags)
        //         .GetValue(sceneHierarchy);
        //
        //     var method_SetSelection = treeView?
        //         .GetType()
        //         .GetMethod("SetSelection", flags, null, new Type[] {typeof(int[]), typeof(bool)}, null);
        //
        //     var method_TreeViewSelectionChanged = treeView?
        //         .GetType()
        //         .GetMethod("TreeViewSelectionChanged", flags, null, new Type[] {typeof(int[])}, null);
        //
        //     // SceneHierarchy.cs
        //     // treeView.SetSelection(instanceIDs, false);
        //     method_SetSelection?.Invoke(treeView, new object[] {instanceIDs, false});
        //     window.Repaint();
        // }

        public static void SelectChildren()
        {
            var method = sceneHierarchy?.GetType().GetMethod("SelectChildren", flags);
            method?.Invoke(sceneHierarchy, null);
        }

        public static void SelectAll()
        {
            var method = sceneHierarchy?.GetType().GetMethod("SelectAll", flags);
            method?.Invoke(sceneHierarchy, null);
        }
        
        public static void FrameObject(int instanceID, bool ping)
        {
            var method = sceneHierarchy?.GetType().GetMethod("FrameObject", flags);
            method?.Invoke(sceneHierarchy, null);
        }
        
        public static void Collapse(GameObject go, int depth)
        {
            // bail out immediately if the go doesn't have children
            if (go.transform.childCount == 0) return;
            SelectObject(go);
            
            var key = new Event { keyCode = KeyCode.RightArrow, type = EventType.KeyDown };
            for (int i = 0; i < depth; i++)
            {
                GetWindow().SendEvent(key);
            }
        }
        
        private static void SelectObject(GameObject obj)
        {
            Selection.activeObject = obj;
        }
    }
}