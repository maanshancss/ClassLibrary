using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Threading.Synchronize
{
    /// <summary>
    /// SmartRWLocker 简化了ReaderWriterLock的使用。通过using来使用Lock方法返回的对象，如：using(this.smartLocker.Lock(AccessMode.Read)){...}
    /// zhuweisky 2008.11.25
    /// </summary>   
    public class SmartRWLocker
    {
        private ReaderWriterLock readerWriterLock = new ReaderWriterLock();      

        #region LastRequireReadTime
        private DateTime lastRequireReadTime = DateTime.Now;
        public DateTime LastRequireReadTime
        {
            get { return lastRequireReadTime; }
        } 
        #endregion

        #region LastRequireWriteTime
        private DateTime lastRequireWriteTime = DateTime.Now;
        public DateTime LastRequireWriteTime
        {
            get { return lastRequireWriteTime; }
        } 
        #endregion

        #region Lock
        public LockingObject Lock(AccessMode accessMode, bool enableSynchronize)
        {
            if (!enableSynchronize)
            {
                return null;
            }

            return this.Lock(accessMode);
        }

        public LockingObject Lock(AccessMode accessMode)
        {
            if (accessMode == AccessMode.Read)
            {
                this.lastRequireReadTime = DateTime.Now;
            }
            else
            {
                this.lastRequireWriteTime = DateTime.Now;
            }

            return new LockingObject(this.readerWriterLock, accessMode);
        } 
        #endregion
    }   

    /// <summary>
    /// AccessMode 访问锁定资源的方式。
    /// </summary>
    public enum AccessMode
    {       
        Read = 0,
        Write,
        /// <summary>
        /// 前提条件：已经获取Read锁。
        /// 再采用此模式，可以先升级到Write，访问资源，再降级回Read。
        /// </summary>
        UpAndDowngrade4Write
    }
}
