using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Threading.Application
{
    /// <summary>
    /// 异步延迟执行器。
    /// </summary>
    public class AsynDelayActor
    {
        private CbGeneric<object> handler ;
        private object argument;
        private int delayInMSecs = 0;

        public AsynDelayActor(int delayMSecs, CbGeneric<object> action, object arg)
        {
            if (delayInMSecs < 0)
            {
                throw new ArgumentException("The value of delayMSecs is invalid. ");
            }

            this.handler = action;
            this.delayInMSecs = delayMSecs;
            this.argument = arg;
            CbGeneric cb = new CbGeneric(this.Action);
            cb.BeginInvoke(null, null);
        }

        private void Action()
        {
            System.Threading.Thread.Sleep(this.delayInMSecs);
            this.handler(this.argument);
        }
    }
}

