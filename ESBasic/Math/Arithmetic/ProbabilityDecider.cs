using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Arithmetic
{
    /// <summary>
    /// 概率决策器。
    /// </summary>
    public class ProbabilityDecider
    {
        private Random random = new Random();
        public ProbabilityDecider() { }
        public ProbabilityDecider(int _percentValue)
        {
            this.PercentValue = _percentValue;
        }

        #region PercentValue
        private int percentValue = 50;
        public int PercentValue
        {
            get { return percentValue; }
            set
            {
                if (value < 0 || value > 100)
                {
                    throw new Exception("PercentValue must be in the range (0,100).");
                }
                percentValue = value; 
            }
        } 
        #endregion

        /// <summary>
        /// 本次是否命中概率。
        /// </summary>        
        public bool Try()
        {
            return this.percentValue > this.random.Next(0, 100);
        }
    }
}
