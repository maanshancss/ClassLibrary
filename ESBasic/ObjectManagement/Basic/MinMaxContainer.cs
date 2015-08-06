using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.ObjectManagement
{
    /// <summary>
    /// MinMaxContainer 用于保存最小最大值。
    /// 2009.09.25 zhuweisky
    /// </summary>
    public class MinMaxContainer
    {
        #region Insert        
        public void Insert(IComparable t)
        {
            IComparable com = (IComparable)t;
            if (com == null)
            {
                return;
            }

            if (this.Empty)
            {
                this.min = com;
                this.max = com;
                return;
            }

            if (this.min.CompareTo(com) > 0)
            {
                this.min = com;
            }

            if (this.max.CompareTo(com) < 0)
            {
                this.max = com;
            }
        } 
        #endregion

        #region Empty 
        public bool Empty
        {
            get { return this.min == null; }           
        }
        #endregion

        #region Min
        private IComparable min = null;
        public IComparable Min
        {
            get { return min; }            
        }
        #endregion

        #region Max
        private IComparable max = null;
        public IComparable Max
        {
            get
            {
                return this.max;
            }
        }
        #endregion

        #region ToString
        public override string ToString()
        {
            return string.Format("Min:{0} ,Max:{1}", this.min, this.max);
        } 
        #endregion
    }
}
