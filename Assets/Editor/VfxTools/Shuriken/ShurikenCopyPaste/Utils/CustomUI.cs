using UnityEditor;
using UnityEngine;

namespace VfxTools.ShurikenCopyPaste
{
    public static class CustomUI
    {
        public static void DrawModule(ModuleCopyPasteBase module)
        {
            using (new EditorGUILayout.VerticalScope(Config.BoxClipboardModuleOption))
            {
                using (new EditorGUILayout.VerticalScope(GUI.skin.box))
                {
                    if (module.HasValue)
                    {
                        module.IsOpen = EditorGUILayout.Foldout(module.IsOpen, $"{module.ModuleType}");

                        if (module.IsOpen)
                        {
                            EditorGUI.indentLevel++;
                            foreach (var member in module.GetMemberValues())
                            {
                                DrawMember(member);
                            }

                            var internalValues = module.GetInternalValues();
                            if (internalValues != null)
                            {
                                foreach (var value in internalValues)
                                {
                                    using (new EditorGUILayout.HorizontalScope())
                                    {
                                        EditorGUILayout.TextArea(value.ToString());
                                    }
                                }
                            }

                            EditorGUI.indentLevel--;
                        }
                        // EditorGUI.EndDisabledGroup();
                    }
                }

            }
        }

        private static void DrawMember(MemberValue member)
        {
            // if (member.Value == null) return;

            // EditorGUILayout.LabelField($"{member.MemberName}({member.MemberType})");
            using (new EditorGUILayout.HorizontalScope())
            {
                // EditorGUILayout.LabelField(member.Value?.ToString());
                EditorGUILayout.LabelField(member.MemberName, Config.LabelClipboardOption);
                EditorGUILayout.TextArea(member.Value?.ToString());
            }
        }
    }
}