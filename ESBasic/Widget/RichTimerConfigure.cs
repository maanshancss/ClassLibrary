using System;
using System.ComponentModel;
using System.Collections ;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ESBasic.Threading.Timers.RichTimer;

namespace ESBasic.Widget
{
    /// <summary>
    /// RichTimerConfigure 对ESBasic.Threading.Timers.RichTimer.TimerConfiguration进行配置的控件。
    /// zhuweisky 2006.06
    /// </summary>
    public partial class RichTimerConfigure : UserControl, ITimerConfigure
    {    
        #region Ctor
        public RichTimerConfigure()
        {
            InitializeComponent();
            this.enumComboBox_timerType.DataSource = typeof(RichTimerType);
            this.enumComboBox_timerType.SelectedValue = RichTimerType.None;          
            this.comboBox_week.SelectedIndex = 0;           
        } 
        #endregion

        #region ActivityName
        private string activityName = "活动";
        [Description("定时任务的简称"), DefaultValue("活动")]
        public string ActivityName
        {
            get
            {
                return this.activityName;
            }
            set
            {
                this.activityName = value;
                this.label_span.Text = value + "周期：";
                this.label_time.Text = value + "时刻：";
                this.label_timerType.Text = value + "频率：";
            }
        } 
        #endregion

        #region TimerConfiguration
        [Browsable(false)]
        public TimerConfiguration TimerConfiguration
        {
            get
            {
                return this.GetTimerConfiguration();
            }
            set
            {
                this.LoadConfig(value);
            }
        } 
        #endregion      

        #region enumComboBox_timerType_SelectedIndexChanged
        private void enumComboBox_timerType_SelectedIndexChanged()
        {
            switch ((RichTimerType)this.enumComboBox_timerType.SelectedValue)
            {
                case RichTimerType.EverySpan:
                    {
                        this.numericUpDown_day.Enabled = false;
                        this.comboBox_week.Enabled = false;
                        this.numericUpDown_hours.Enabled = false;
                        this.numericUpDown_mins.Enabled = false;
                        this.numericUpDown_spanhour.Enabled = true;
                        this.numericUpDown_spanmins.Enabled = true;

                        this.numericUpDown_day.Value = 1;
                        this.comboBox_week.Text = "天";
                        this.numericUpDown_hours.Value = 0;
                        this.numericUpDown_mins.Value = 0;
                        this.numericUpDown_spanhour.Value = 0;
                        this.numericUpDown_spanmins.Value = 0;
                        break;
                    }
                case RichTimerType.PerDay:
                    {
                        this.numericUpDown_day.Enabled = false;
                        this.comboBox_week.Enabled = false;
                        this.numericUpDown_hours.Enabled = true;
                        this.numericUpDown_mins.Enabled = true;
                        this.numericUpDown_spanhour.Enabled = false;
                        this.numericUpDown_spanmins.Enabled = false;

                        this.numericUpDown_day.Value = 1;
                        this.comboBox_week.Text = "天";
                        this.numericUpDown_hours.Value = 0;
                        this.numericUpDown_mins.Value = 0;
                        this.numericUpDown_spanhour.Value = 0;
                        this.numericUpDown_spanmins.Value = 0;
                        break;
                    }
                case RichTimerType.PerHour:
                    {
                        this.numericUpDown_day.Enabled = false;
                        this.comboBox_week.Enabled = false;
                        this.numericUpDown_hours.Enabled = false;
                        this.numericUpDown_mins.Enabled = true;
                        this.numericUpDown_spanhour.Enabled = false;
                        this.numericUpDown_spanmins.Enabled = false;

                        this.numericUpDown_day.Value = 1;
                        this.comboBox_week.Text = "天";
                        this.numericUpDown_hours.Value = 0;
                        this.numericUpDown_mins.Value = 0;
                        this.numericUpDown_spanhour.Value = 0;
                        this.numericUpDown_spanmins.Value = 0;
                        break;
                    }
                case RichTimerType.PerMonth:
                    {
                        this.numericUpDown_day.Enabled = true;
                        this.comboBox_week.Enabled = false;
                        this.numericUpDown_hours.Enabled = true;
                        this.numericUpDown_mins.Enabled = true;
                        this.numericUpDown_spanhour.Enabled = false;
                        this.numericUpDown_spanmins.Enabled = false;

                        this.numericUpDown_day.Value = 1;
                        this.comboBox_week.Text = "天";
                        this.numericUpDown_hours.Value = 0;
                        this.numericUpDown_mins.Value = 0;
                        this.numericUpDown_spanhour.Value = 0;
                        this.numericUpDown_spanmins.Value = 0;
                        break;
                    }
                case RichTimerType.PerWeek:
                    {
                        this.numericUpDown_day.Enabled = false;
                        this.comboBox_week.Enabled = true;
                        this.numericUpDown_hours.Enabled = true;
                        this.numericUpDown_mins.Enabled = true;
                        this.numericUpDown_spanhour.Enabled = false;
                        this.numericUpDown_spanmins.Enabled = false;

                        this.numericUpDown_day.Value = 1;
                        this.comboBox_week.Text = "天";
                        this.numericUpDown_hours.Value = 0;
                        this.numericUpDown_mins.Value = 0;
                        this.numericUpDown_spanhour.Value = 0;
                        this.numericUpDown_spanmins.Value = 0;
                        break;
                    }
                case RichTimerType.None:
                    {
                        this.numericUpDown_day.Enabled = false;
                        this.comboBox_week.Enabled = false;
                        this.numericUpDown_hours.Enabled = false;
                        this.numericUpDown_mins.Enabled = false;
                        this.numericUpDown_spanhour.Enabled = false;
                        this.numericUpDown_spanmins.Enabled = false;

                        this.numericUpDown_day.Value = 1;
                        this.comboBox_week.Text = "天";
                        this.numericUpDown_hours.Value = 0;
                        this.numericUpDown_mins.Value = 0;
                        this.numericUpDown_spanhour.Value = 0;
                        this.numericUpDown_spanmins.Value = 0;
                        break;
                    }
            }
        }
        #endregion

        #region private
        #region LoadConfig
        private void LoadConfig(TimerConfiguration config)
        {
            if (config == null)
            {
                this.enumComboBox_timerType.SelectedValue = RichTimerType.None;
                this.numericUpDown_day.Value = 1;
                this.comboBox_week.Text = "天";
                this.numericUpDown_hours.Value = 0;
                this.numericUpDown_mins.Value = 0;
                this.numericUpDown_spanhour.Value = 0;
                this.numericUpDown_spanmins.Value = 0;

                this.numericUpDown_day.Enabled = false;
                this.comboBox_week.Enabled = false;
                this.numericUpDown_hours.Enabled = false;
                this.numericUpDown_mins.Enabled = false;
                this.numericUpDown_spanhour.Enabled = false;
                this.numericUpDown_spanmins.Enabled = false;
                return;
            }

            this.enumComboBox_timerType.SelectedValue = config.RichTimerType;

            switch (config.RichTimerType)
            {
                case RichTimerType.EverySpan:
                    {
                        this.numericUpDown_day.Value = 1;
                        this.comboBox_week.Text = "天";
                        this.numericUpDown_hours.Value = 0;
                        this.numericUpDown_mins.Value = 0;
                        this.numericUpDown_spanhour.Value = config.Hour;
                        this.numericUpDown_spanmins.Value = config.Minute;

                        this.numericUpDown_day.Enabled = false;
                        this.comboBox_week.Enabled = false;
                        this.numericUpDown_hours.Enabled = false;
                        this.numericUpDown_mins.Enabled = false;
                        this.numericUpDown_spanhour.Enabled = true;
                        this.numericUpDown_spanmins.Enabled = true;
                        break;
                    }
                case RichTimerType.PerDay:
                    {
                        this.numericUpDown_day.Enabled = false;
                        this.comboBox_week.Enabled = false;
                        this.numericUpDown_hours.Enabled = true;
                        this.numericUpDown_mins.Enabled = true;
                        this.numericUpDown_spanhour.Enabled = false;
                        this.numericUpDown_spanmins.Enabled = false;

                        this.numericUpDown_day.Value = 1;
                        this.comboBox_week.Text = "天";
                        this.numericUpDown_hours.Value = config.Hour;
                        this.numericUpDown_mins.Value = config.Minute;
                        this.numericUpDown_spanhour.Value = 0;
                        this.numericUpDown_spanmins.Value = 0;
                        break;
                    }
                case RichTimerType.PerHour:
                    {
                        this.numericUpDown_day.Enabled = false;
                        this.comboBox_week.Enabled = false;
                        this.numericUpDown_hours.Enabled = false;
                        this.numericUpDown_mins.Enabled = true;
                        this.numericUpDown_spanhour.Enabled = false;
                        this.numericUpDown_spanmins.Enabled = false;

                        this.numericUpDown_day.Value = 1;
                        this.comboBox_week.Text = "天";
                        this.numericUpDown_hours.Value = 0;
                        this.numericUpDown_mins.Value = config.Minute;
                        this.numericUpDown_spanhour.Value = 0;
                        this.numericUpDown_spanmins.Value = 0;
                        break;
                    }
                case RichTimerType.PerMonth:
                    {
                        this.numericUpDown_day.Enabled = true;
                        this.comboBox_week.Enabled = false;
                        this.numericUpDown_hours.Enabled = true;
                        this.numericUpDown_mins.Enabled = true;
                        this.numericUpDown_spanhour.Enabled = false;
                        this.numericUpDown_spanmins.Enabled = false;

                        this.numericUpDown_day.Value = config.Day;
                        this.comboBox_week.Text = "天";
                        this.numericUpDown_hours.Value = config.Hour;
                        this.numericUpDown_mins.Value = config.Minute;
                        this.numericUpDown_spanhour.Value = 0;
                        this.numericUpDown_spanmins.Value = 0;
                        break;
                    }
                case RichTimerType.PerWeek:
                    {
                        this.numericUpDown_day.Enabled = false;
                        this.comboBox_week.Enabled = true;
                        this.numericUpDown_hours.Enabled = true;
                        this.numericUpDown_mins.Enabled = true;
                        this.numericUpDown_spanhour.Enabled = false;
                        this.numericUpDown_spanmins.Enabled = false;

                        this.numericUpDown_day.Value = 1;
                        this.comboBox_week.SelectedItem = config.DayOfWeek;
                        this.numericUpDown_hours.Value = config.Hour;
                        this.numericUpDown_mins.Value = config.Minute;
                        this.numericUpDown_spanhour.Value = 0;
                        this.numericUpDown_spanmins.Value = 0;
                        break;
                    }
            }
        } 
        #endregion

        #region GetTimerConfiguration
        private TimerConfiguration GetTimerConfiguration()
        {
            TimerConfiguration ConfigTrans = null;

            switch ((RichTimerType)this.enumComboBox_timerType.SelectedValue)
            {
                case RichTimerType.None:
                    {
                        return null;
                    }
                case RichTimerType.EverySpan:
                    {
                        ConfigTrans = new TimerConfiguration();
                        ConfigTrans.RichTimerType = RichTimerType.EverySpan;
                        ConfigTrans.Hour = (int)this.numericUpDown_spanhour.Value;
                        ConfigTrans.Minute = (int)this.numericUpDown_spanmins.Value;
                        return ConfigTrans;
                    }
                case RichTimerType.PerMonth:
                    {
                        ConfigTrans = new TimerConfiguration();
                        ConfigTrans.RichTimerType = RichTimerType.PerMonth;
                        ConfigTrans.Day = (int)this.numericUpDown_day.Value;
                        ConfigTrans.Hour = (int)this.numericUpDown_hours.Value;
                        ConfigTrans.Minute = (int)this.numericUpDown_mins.Value;
                        //ConfigTrans.Second =(int)this.
                        return ConfigTrans;
                    }
                case RichTimerType.PerWeek:
                    {
                        ConfigTrans = new TimerConfiguration();
                        ConfigTrans.RichTimerType = RichTimerType.PerWeek;
                        ConfigTrans.DayOfWeek = (int)this.comboBox_week.SelectedIndex;
                        ConfigTrans.Hour = (int)this.numericUpDown_hours.Value;
                        ConfigTrans.Minute = (int)this.numericUpDown_mins.Value;
                        return ConfigTrans;
                    }
                case RichTimerType.PerDay:
                    {
                        ConfigTrans = new TimerConfiguration();
                        ConfigTrans.RichTimerType = RichTimerType.PerDay;
                        ConfigTrans.Hour = (int)this.numericUpDown_hours.Value;
                        ConfigTrans.Minute = (int)this.numericUpDown_mins.Value;
                        return ConfigTrans;
                    }
                case RichTimerType.PerHour:
                    {
                        ConfigTrans = new TimerConfiguration();
                        ConfigTrans.RichTimerType = RichTimerType.PerHour;
                        ConfigTrans.Hour = (int)this.numericUpDown_hours.Value;
                        ConfigTrans.Minute = (int)this.numericUpDown_mins.Value;
                        return ConfigTrans;
                    }
                default:
                    {
                        return null;
                    }
            }
        } 
        #endregion
        #endregion     

    }
}
