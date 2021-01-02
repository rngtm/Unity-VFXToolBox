using System;

namespace VfxToolBox.ShurikenCopyPaste
{
    public class DelayCallData
    {
        public int Frame;
        public Action Action;
        public bool IsEmpty => Frame == 0;
    }
}