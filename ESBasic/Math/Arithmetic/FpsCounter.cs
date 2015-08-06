using System;
using System.Collections.Generic;
using System.Text;
using ESBasic.Threading.Engines;

namespace ESBasic.Arithmetic
{
    /// <summary>
    /// FpsCounter 用于计算帧率。
    /// </summary>
    public class FpsCounter : BaseCycleEngine, IDisposable
    {        
        private int count = 0;
        private object locker = new object();
        public event CbGeneric<double> FpsDetected;

        public FpsCounter(int detectSpanInSecs)
        {
            base.DetectSpanInSecs = detectSpanInSecs;
            base.Start();
        }

        #region AddFrame
        public void AddFrame()
        {
            lock (this.locker)
            {               
                ++this.count;
                ++this.totalCount;
            }
        } 
        #endregion

        #region Fps
        private double fps = 0;
        public double Fps
        {
            get { return fps; }            
        } 
        #endregion

        #region TotalCount
        private ulong totalCount = 0;
        public ulong TotalCount
        {
            get { return totalCount; }
        } 
        #endregion

        #region DoDetect
        protected override bool DoDetect()
        {
            lock (this.locker)
            {
                this.fps = this.count / (double)base.DetectSpanInSecs;
                this.count = 0;

                if (this.FpsDetected != null)
                {
                    this.FpsDetected(this.fps);
                }
            }

            return true;
        } 
        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            base.Stop();
        }

        #endregion
    }
}
