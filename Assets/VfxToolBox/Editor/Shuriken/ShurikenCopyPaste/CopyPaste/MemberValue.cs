using System;
using System.Reflection;

namespace VfxToolBox.ShurikenCopyPaste
{
    [Serializable]
    public class MemberValue
    {
        public MemberTypes MemberType;
        public object Value;
        public string MemberName;
    }
}