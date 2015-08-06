using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace ESBasic.Widget
{
    public class TimerLabel : Label
    {
        private Timer timer = new Timer();
        /// <summary>
        /// 当定时器启动后，每隔一秒触发一次。在UI线程中触发。
        /// </summary>
        public event CbGeneric SecondTick;

        #region TotalSeconds
        private int totalSeconds = 0;
        public int TotalSeconds
        {
            get { return totalSeconds; }
        } 
        #endregion

        public TimerLabel()
            : base()
        {
            this.SetStyle(ControlStyles.ResizeRedraw, true);//调整大小时重绘
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);// 双缓冲
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);// 禁止擦除背景.
            this.SetStyle(ControlStyles.UserPaint, true);//自行绘制            
            this.UpdateStyles();

            this.BackColor = Color.Transparent;            
            this.Text = "00:00";
            this.timer.Interval = 1000;
            this.timer.Tick += new EventHandler(timer_Tick);
        }             

        void timer_Tick(object sender, EventArgs e)
        {
            ++this.totalSeconds;
            this.Text = string.Format("{0}:{1}", (this.totalSeconds / 60).ToString("00"), (this.totalSeconds % 60).ToString("00"));
            if (this.SecondTick != null)
            {
                this.SecondTick();
            }
        }

        public void Start()
        {
            this.Text = "00:00" ;
            this.timer.Start();
        }

        public void Stop()
        {
            this.timer.Stop();
        }

        public bool IsWorking
        {
            get
            {
                return this.timer.Enabled ;
            }
        }

        public void Reset()
        {
            this.timer.Stop();
            this.totalSeconds = 0;
            this.Text = "00:00";
        }
    }
}
