using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Threading.Timers
{
    /// <summary>
    /// CallbackTask ���ڷ�װһ����ʱ�ص�����
    /// </summary>    
    public class CallbackTask<T>
    {
        public CallbackTask()
        {
        }

        public CallbackTask(int _id, int iniSecs, CbGeneric<T> _callback, T _callbackPara)
        {
            this.iD = _id;
            this.leftSeconds = iniSecs;
            this.callbackPara = _callbackPara;
            this.callback = _callback;

            if (this.leftSeconds <= 0)
            {
                throw new ArgumentException("The Left Seconds must greater than 0!");
            }
        }

        #region ID
        private int iD = 0;
        public int ID
        {
            get { return iD; }
            set { iD = value; }
        } 
        #endregion

        #region CallbackPara
        private T callbackPara;
        public T CallbackPara
        {
            get { return callbackPara; }
            set { callbackPara = value; }
        }
      
        #endregion

        #region LeftSeconds
        private int leftSeconds = 10;
        public int LeftSeconds
        {
            set { leftSeconds = value; }
            get { return leftSeconds; }
        }
        #endregion

        #region Callback
        private CbGeneric<T> callback;
        /// <summary>
        /// Callback �ص�ִ��ʱ���������׳��κ��쳣
        /// </summary>
        public CbGeneric<T> Callback
        {
            get { return callback; }
            set { callback = value; }
        }
        #endregion

        #region SecondsPassed
        public bool SecondsPassed(int seconds)
        {
            this.leftSeconds -= seconds;

            return this.leftSeconds <= 0;
        }
        #endregion
    }
}
