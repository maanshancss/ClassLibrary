using System;
using System.Collections.Generic;
using System.Text;
using ESBasic.Loggers;
using System.Windows.Forms;

namespace ESBasic.Helpers
{
    /// <summary>
    /// 将方法调用转发到UI线程。捕捉异常，并记录日志。
    /// </summary>
    public class UiSafeInvoker
    {
        private Control control;
        private IAgileLogger agileLogger = new EmptyAgileLogger();
        private bool showMessageBox = false;
        private bool useBeginInvoke = false;

        #region Ctor       
        public UiSafeInvoker(Control ui, bool showMessageBoxOnException, bool beginInvoke, IAgileLogger logger)
        {
            this.control = ui;
            this.agileLogger = logger ?? new EmptyAgileLogger();
            this.showMessageBox = showMessageBoxOnException;
            this.useBeginInvoke = beginInvoke;
        }
        #endregion

        #region ActionOnUI 
        public void ActionOnUI(CbGeneric method)
        {
            this.ActionOnUI(this.showMessageBox, this.useBeginInvoke, method);
        }
        public void ActionOnUI(bool showMessageBoxOnException, bool beginInvoke, CbGeneric method)
        {
            if (this.control.InvokeRequired)
            {
                if (beginInvoke)
                {
                    this.control.BeginInvoke(new CbGeneric<bool, CbGeneric>(this.Do_ActionOnUI), showMessageBoxOnException, method);
                    return;
                }
                this.control.Invoke(new CbGeneric<bool, CbGeneric>(this.Do_ActionOnUI), showMessageBoxOnException, method);
                return;
            }

            this.Do_ActionOnUI(showMessageBoxOnException, method);
        }

        private void Do_ActionOnUI(bool showMessageBoxOnException, CbGeneric method)
        {
            try
            {
                method();
            }
            catch (Exception ee)
            {
                this.agileLogger.Log(ee, method.Method.Name, ESBasic.Loggers.ErrorLevel.Standard);
                if (showMessageBoxOnException)
                {
                    MessageBox.Show(ee.Message);
                }
            }
        }
        #endregion

        #region ActionOnUI - 1
        public void ActionOnUI<T1>(CbGeneric<T1> method, T1 args)
        {
            this.ActionOnUI<T1>(this.showMessageBox, this.useBeginInvoke, method, args);
        }
        public void ActionOnUI<T1>(bool showMessageBoxOnException, bool beginInvoke, CbGeneric<T1> method, T1 args)
        {
            if (this.control.InvokeRequired)
            {
                if (beginInvoke)
                {
                    this.control.BeginInvoke(new CbGeneric<bool, CbGeneric<T1>, T1>(this.Do_ActionOnUI<T1>), showMessageBoxOnException, method, args);
                    return;
                }
                this.control.Invoke(new CbGeneric<bool, CbGeneric<T1>, T1>(this.Do_ActionOnUI<T1>), showMessageBoxOnException, method, args);
                return;
            }

            this.Do_ActionOnUI<T1>(showMessageBoxOnException, method, args);
        }

        private void Do_ActionOnUI<T1>(bool showMessageBoxOnException, CbGeneric<T1> method, T1 args)
        {
            try
            {
                method(args);
            }
            catch (Exception ee)
            {
                this.agileLogger.Log(ee, method.Method.Name, ESBasic.Loggers.ErrorLevel.Standard);
                if (showMessageBoxOnException)
                {
                    MessageBox.Show(ee.Message);
                }
            }
        } 
        #endregion

        #region ActionOnUI - 2
        public void ActionOnUI<T1, T2>(CbGeneric<T1, T2> method, T1 arg1, T2 arg2)
        {
            this.ActionOnUI<T1, T2>(this.showMessageBox, this.useBeginInvoke, method, arg1, arg2);
        }
        public void ActionOnUI<T1, T2>(bool showMessageBoxOnException, bool beginInvoke, CbGeneric<T1, T2> method, T1 arg1, T2 arg2)
        {
            if (this.control.InvokeRequired)
            {
                if (beginInvoke)
                {
                    this.control.BeginInvoke(new CbGeneric<bool, CbGeneric<T1, T2>, T1, T2>(this.Do_ActionOnUI<T1, T2>), showMessageBoxOnException, method, arg1, arg2);
                    return;
                }
                this.control.Invoke(new CbGeneric<bool, CbGeneric<T1, T2>, T1, T2>(this.Do_ActionOnUI<T1, T2>), showMessageBoxOnException, method, arg1, arg2);
                return;
            }

            this.Do_ActionOnUI<T1, T2>(showMessageBoxOnException, method, arg1, arg2);
        }

        private void Do_ActionOnUI<T1, T2>(bool showMessageBoxOnException, CbGeneric<T1, T2> method, T1 arg1, T2 arg2)
        {
            try
            {
                method(arg1, arg2);
            }
            catch (Exception ee)
            {
                this.agileLogger.Log(ee, method.Method.Name, ESBasic.Loggers.ErrorLevel.Standard);
                if (showMessageBoxOnException)
                {
                    MessageBox.Show(ee.Message);
                }
            }
        } 
        #endregion

        #region ActionOnUI - 3
        public void ActionOnUI<T1, T2, T3>(CbGeneric<T1, T2, T3> method, T1 arg1, T2 arg2, T3 arg3)
        {
            this.ActionOnUI<T1, T2, T3>(this.showMessageBox, this.useBeginInvoke, method, arg1, arg2, arg3);
        }
        public void ActionOnUI<T1, T2, T3>(bool showMessageBoxOnException, bool beginInvoke, CbGeneric<T1, T2, T3> method, T1 arg1, T2 arg2, T3 arg3)
        {
            if (this.control.InvokeRequired)
            {
                if (beginInvoke)
                {
                    this.control.BeginInvoke(new CbGeneric<bool, CbGeneric<T1, T2, T3>, T1, T2, T3>(this.Do_ActionOnUI<T1, T2, T3>), showMessageBoxOnException, method, arg1, arg2, arg3);
                    return;
                }
                this.control.Invoke(new CbGeneric<bool, CbGeneric<T1, T2, T3>, T1, T2, T3>(this.Do_ActionOnUI<T1, T2, T3>), showMessageBoxOnException, method, arg1, arg2, arg3);
                return;
            }

            this.Do_ActionOnUI<T1, T2, T3>(showMessageBoxOnException, method, arg1, arg2, arg3);
        }


        private void Do_ActionOnUI<T1, T2, T3>(bool showMessageBoxOnException, CbGeneric<T1, T2, T3> method, T1 arg1, T2 arg2, T3 arg3)
        {
            try
            {
                method(arg1, arg2, arg3);
            }
            catch (Exception ee)
            {
                this.agileLogger.Log(ee, method.Method.Name, ESBasic.Loggers.ErrorLevel.Standard);
                if (showMessageBoxOnException)
                {
                    MessageBox.Show(ee.Message);
                }
            }
        } 
        #endregion

        #region Do_ActionOnUI - 4
        public void ActionOnUI<T1, T2, T3, T4>(CbGeneric<T1, T2, T3, T4> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            this.ActionOnUI<T1, T2, T3, T4>(this.showMessageBox, this.useBeginInvoke, method, arg1, arg2, arg3, arg4);
        }
        public void ActionOnUI<T1, T2, T3, T4>(bool showMessageBoxOnException, bool beginInvoke, CbGeneric<T1, T2, T3, T4> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (this.control.InvokeRequired)
            {
                if (beginInvoke)
                {
                    this.control.BeginInvoke(new CbGeneric<bool, CbGeneric<T1, T2, T3, T4>, T1, T2, T3, T4>(this.Do_ActionOnUI<T1, T2, T3, T4>), showMessageBoxOnException, method, arg1, arg2, arg3, arg4);
                    return;
                }
                this.control.Invoke(new CbGeneric<bool, CbGeneric<T1, T2, T3, T4>, T1, T2, T3, T4>(this.Do_ActionOnUI<T1, T2, T3, T4>), showMessageBoxOnException, method, arg1, arg2, arg3, arg4);
                return;
            }

            this.Do_ActionOnUI<T1, T2, T3, T4>(showMessageBoxOnException, method, arg1, arg2, arg3, arg4);
        }

        private void Do_ActionOnUI<T1, T2, T3, T4>(bool showMessageBoxOnException, CbGeneric<T1, T2, T3, T4> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            try
            {
                method(arg1, arg2, arg3, arg4);
            }
            catch (Exception ee)
            {
                this.agileLogger.Log(ee, method.Method.Name, ESBasic.Loggers.ErrorLevel.Standard);
                if (showMessageBoxOnException)
                {
                    MessageBox.Show(ee.Message);
                }
            }
        } 
        #endregion

        #region ActionOnUI - 5
        public void ActionOnUI<T1, T2, T3, T4, T5>(CbGeneric<T1, T2, T3, T4, T5> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            this.ActionOnUI<T1, T2, T3, T4, T5>(this.showMessageBox, this.useBeginInvoke, method, arg1, arg2, arg3, arg4, arg5);
        }
        public void ActionOnUI<T1, T2, T3, T4, T5>(bool showMessageBoxOnException, bool beginInvoke, CbGeneric<T1, T2, T3, T4, T5> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            if (this.control.InvokeRequired)
            {
                if (beginInvoke)
                {
                    this.control.BeginInvoke(new CbGeneric<bool, CbGeneric<T1, T2, T3, T4, T5>, T1, T2, T3, T4, T5>(this.Do_ActionOnUI<T1, T2, T3, T4, T5>), showMessageBoxOnException, method, arg1, arg2, arg3, arg4, arg5);
                    return;
                }
                this.control.Invoke(new CbGeneric<bool, CbGeneric<T1, T2, T3, T4, T5>, T1, T2, T3, T4, T5>(this.Do_ActionOnUI<T1, T2, T3, T4, T5>), showMessageBoxOnException, method, arg1, arg2, arg3, arg4, arg5);
                return;
            }

            this.Do_ActionOnUI<T1, T2, T3, T4, T5>(showMessageBoxOnException, method, arg1, arg2, arg3, arg4, arg5);
        }


        private void Do_ActionOnUI<T1, T2, T3, T4, T5>(bool showMessageBoxOnException, CbGeneric<T1, T2, T3, T4, T5> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            try
            {
                method(arg1, arg2, arg3, arg4, arg5);
            }
            catch (Exception ee)
            {
                this.agileLogger.Log(ee, method.Method.Name, ESBasic.Loggers.ErrorLevel.Standard);
                if (showMessageBoxOnException)
                {
                    MessageBox.Show(ee.Message);
                }
            }
        } 
        #endregion

        #region ActionOnUI - 6
        public void ActionOnUI<T1, T2, T3, T4, T5, T6>(CbGeneric<T1, T2, T3, T4, T5, T6> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            this.ActionOnUI<T1, T2, T3, T4, T5, T6>(this.showMessageBox, this.useBeginInvoke, method, arg1, arg2, arg3, arg4, arg5, arg6);
        }
        public void ActionOnUI<T1, T2, T3, T4, T5, T6>(bool showMessageBoxOnException, bool beginInvoke, CbGeneric<T1, T2, T3, T4, T5, T6> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            if (this.control.InvokeRequired)
            {
                if (beginInvoke)
                {
                    this.control.BeginInvoke(new CbGeneric<bool, CbGeneric<T1, T2, T3, T4, T5, T6>, T1, T2, T3, T4, T5, T6>(this.Do_ActionOnUI<T1, T2, T3, T4, T5, T6>), showMessageBoxOnException, method, arg1, arg2, arg3, arg4, arg5, arg6);
                    return;
                }
                this.control.Invoke(new CbGeneric<bool, CbGeneric<T1, T2, T3, T4, T5, T6>, T1, T2, T3, T4, T5, T6>(this.Do_ActionOnUI<T1, T2, T3, T4, T5, T6>), showMessageBoxOnException, method, arg1, arg2, arg3, arg4, arg5, arg6);
                return;
            }

            this.Do_ActionOnUI<T1, T2, T3, T4, T5, T6>(showMessageBoxOnException, method, arg1, arg2, arg3, arg4, arg5, arg6);
        }
        private void Do_ActionOnUI<T1, T2, T3, T4, T5, T6>(bool showMessageBoxOnException, CbGeneric<T1, T2, T3, T4, T5, T6> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            try
            {
                method(arg1, arg2, arg3, arg4, arg5, arg6);
            }
            catch (Exception ee)
            {
                this.agileLogger.Log(ee, method.Method.Name, ESBasic.Loggers.ErrorLevel.Standard);
                if (showMessageBoxOnException)
                {
                    MessageBox.Show(ee.Message);
                }
            }
        }
        
        #endregion
        #region ActionOnUI - 7
        public void ActionOnUI<T1, T2, T3, T4, T5, T6, T7>(CbGeneric<T1, T2, T3, T4, T5, T6, T7> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            this.ActionOnUI<T1, T2, T3, T4, T5, T6, T7>(this.showMessageBox, this.useBeginInvoke, method, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }
        public void ActionOnUI<T1, T2, T3, T4, T5, T6, T7>(bool showMessageBoxOnException, bool beginInvoke, CbGeneric<T1, T2, T3, T4, T5, T6, T7> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            if (this.control.InvokeRequired)
            {
                if (beginInvoke)
                {
                    this.control.BeginInvoke(new CbGeneric<bool, CbGeneric<T1, T2, T3, T4, T5, T6, T7>, T1, T2, T3, T4, T5, T6, T7>(this.Do_ActionOnUI<T1, T2, T3, T4, T5, T6, T7>), showMessageBoxOnException, method, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
                    return;
                }
                this.control.Invoke(new CbGeneric<bool, CbGeneric<T1, T2, T3, T4, T5, T6, T7>, T1, T2, T3, T4, T5, T6, T7>(this.Do_ActionOnUI<T1, T2, T3, T4, T5, T6, T7>), showMessageBoxOnException, method, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
                return;
            }

            this.Do_ActionOnUI<T1, T2, T3, T4, T5, T6, T7>(showMessageBoxOnException, method, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }
        private void Do_ActionOnUI<T1, T2, T3, T4, T5, T6, T7>(bool showMessageBoxOnException, CbGeneric<T1, T2, T3, T4, T5, T6, T7> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            try
            {
                method(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            }
            catch (Exception ee)
            {
                this.agileLogger.Log(ee, method.Method.Name, ESBasic.Loggers.ErrorLevel.Standard);
                if (showMessageBoxOnException)
                {
                    MessageBox.Show(ee.Message);
                }
            }
        } 
        #endregion
    }
}
