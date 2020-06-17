using System;
using System.Reflection;

namespace VfxTools.ShurikenCopyPaste
{
    [Serializable]
    public class MemberValue
    {
        public MemberTypes MemberType;
        public object Value;
        public string MemberName;
    }
}