using System;

namespace VfxTools.ShurikenCopyPaste
{
    public class DelayCallData
    {
        public int Frame;
        public Action Action;
        public bool IsEmpty => Frame == 0;
    }
}