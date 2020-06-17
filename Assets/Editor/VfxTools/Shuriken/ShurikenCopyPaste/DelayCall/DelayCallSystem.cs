using System;
using System.Collections.Generic;
using System.Linq;

namespace VfxTools.ShurikenCopyPaste
{
    public class DelayCallSystem
    {
        private readonly List<DelayCallData> delayCallList = new List<DelayCallData>();
        
        /** ********************************************************************************
        * @summary 指定したフレーム経過後に処理を実行
        ***********************************************************************************/
        public void Register(int delayFrame, Action action)
        {
            var delayCallData = delayCallList.FirstOrDefault(item => item.IsEmpty);
            if (delayCallData == null)
            {
                delayCallData = new DelayCallData();
                delayCallList.Add(delayCallData);
            }

            delayCallData.Frame = delayFrame;
            delayCallData.Action = action;
        }
        
        /** ********************************************************************************
        * @summary 毎フレーム実行
        ***********************************************************************************/
        public void DoUpdate()
        {
            foreach (var delayCall in delayCallList)
            {
                if (delayCall.IsEmpty) continue;

                delayCall.Frame--;
                if (delayCall.Frame == 0)
                {
                    delayCall.Action?.Invoke();
                    delayCall.Action = null;
                }
            }
        }
    }
}