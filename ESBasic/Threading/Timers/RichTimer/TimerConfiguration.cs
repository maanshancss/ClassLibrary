using System;
using System.Collections.Generic;
using System.Text;
using ESBasic.Helpers;

namespace ESBasic.Threading.Timers.RichTimer
{
    /// <summary>
    /// TimerConfiguration 定时详细设置
    /// TimerConfiguration可以不依赖ESBasic.RichTimer空间中的其他组件而被单独轻量地使用。使用TimerConfigure来进行UI配置。
    /// zhuweisky 2006.06
    /// </summary>
    [Serializable]
    public class TimerConfiguration
    {
        [NonSerialized]
        private DateTime lastRightTime = DateTime.Parse("2000-01-01 00:00:00");

        #region RichTimerType
        private RichTimerType richTimerType = RichTimerType.PerDay;
        public RichTimerType RichTimerType
        {
            get
            {
                return this.richTimerType;
            }
            set
            {
                this.richTimerType = value;
            }
        }
        #endregion

        #region ValidityDateScope
        private DateScope validityDateScope = new DateScope();
        public DateScope ValidityDateScope
        {
            get { return validityDateScope; }
            set
            {
                validityDateScope = value ?? new DateScope();
            }
        } 
        #endregion        

        #region ShortTimeScope
        private ShortTimeScope shortTimeScope = new ShortTimeScope();
        public ShortTimeScope ShortTimeScope
        {
            get { return shortTimeScope; }
            set
            {
                shortTimeScope = value ?? new ShortTimeScope();
            }
        } 
        #endregion        

        #region Day
        private int day = 1;//该月的第几天
        public int Day
        {
            get
            {
                return this.day;
            }
            set
            {
                this.day = value;
            }
        }
        #endregion

        #region DayOfWeek
        private int dayOfWeek = 1;//周
        public int DayOfWeek
        {
            get
            {
                return this.dayOfWeek;
            }
            set
            {
                this.dayOfWeek = value;
            }
        }
        #endregion

        #region Hour
        private int hour = 0;
        public int Hour
        {
            get
            {
                return this.hour;
            }
            set
            {
                this.hour = value;
            }
        }
        #endregion

        #region Minute
        private int minute = 0;
        public int Minute
        {
            get
            {
                return this.minute;
            }
            set
            {
                this.minute = value;
            }
        }
        #endregion

        #region Second
        private int second = 0;
        public int Second
        {
            get
            {
                return this.second;
            }
            set
            {
                this.second = value;
            }
        }
        #endregion

        #region IsExpired
        public bool IsExpired(DateTime now)
        {
            if (this.richTimerType == RichTimerType.JustOnce)
            {
                TimeSpan span = now - this.targetTimeForJustOnce;

                return span.TotalMilliseconds >= 10000;//10s
            }

            return !this.validityDateScope.Contains(now);
        } 
        #endregion

        #region IsOnTime
        public bool IsOnTime(int checkSpanSeconds, DateTime now)
        {
            TimeSpan span = now - this.lastRightTime;
            if (span.TotalMilliseconds < checkSpanSeconds * 1000 * 2)
            {
                return false;
            }

            bool onTime = this.CheckOnTime(checkSpanSeconds, now);
            if (onTime)
            {
                this.lastRightTime = now;
            }

            return onTime;
        }

        private bool CheckOnTime(int checkSpanSeconds ,DateTime now)
        {  
            if (this.IsExpired(now))
            {
                return false;
            }

            ShortTime target = new ShortTime(now);
            if (! this.ShortTimeScope.Contains(target))
            {
                return false;
            }

            DateTime onTime = new DateTime();

            switch (this.richTimerType)
            {
                case RichTimerType.PerDay:
                    {
                        onTime = new DateTime(now.Year, now.Month, now.Day, this.hour, this.minute, this.second);
                        break;
                    }
                case RichTimerType.PerHour:
                    {
                        onTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, this.minute, this.second);
                        break;
                    }
                case RichTimerType.PerMonth:
                    {
                        onTime = new DateTime(now.Year, now.Month, this.Day, this.hour, this.minute, this.second);
                        break;
                    }
                case RichTimerType.PerWeek:
                    {
                        onTime = new DateTime(now.Year, now.Month, now.Day, this.hour, this.minute, this.second);
                        break;
                    }
                case RichTimerType.JustOnce:
                    {
                        onTime = this.targetTimeForJustOnce;
                        break;
                    }
                case RichTimerType.EverySpan:
                    {
                        int cycleSpanInSecs = (this.hour * 3600 + this.minute * 60 + this.second);
                        if (cycleSpanInSecs <= 0)
                        {
                            return false;
                        }

                        return TimeHelper.IsOnTime(this.shortTimeScope.ShortTimeStart.GetDateTime(), now, cycleSpanInSecs, checkSpanSeconds);   
                    }
                
                default:
                    {
                        break;
                    }
            }

            return TimeHelper.IsOnTime(onTime, now, checkSpanSeconds);
        }
        #endregion

        #region operator
        public static bool operator ==(TimerConfiguration left, TimerConfiguration right)
        {
            if (object.ReferenceEquals(left, null) && object.ReferenceEquals(right, null))
            {
                return true;
            }

            if (object.ReferenceEquals(left ,null) || object.ReferenceEquals(right ,null))
            {
                return false;
            }

            if (object.ReferenceEquals(left, right))
            {
                return true;
            }

            bool b1 = (left.day == right.day);
            bool b2 = (left.dayOfWeek == right.dayOfWeek);
            bool b3 = (left.hour == right.hour);
            bool b4 = (left.minute == right.minute);
            bool b5 = (left.richTimerType == right.richTimerType);
            bool b6 = (left.second == right.second);
            bool b7 = (left.shortTimeScope == right.shortTimeScope);
            bool b8 = (left.validityDateScope == right.validityDateScope);

            return b1 && b2 && b3 && b4 && b5 && b6 && b7 && b8;
        }

        public static bool operator !=(TimerConfiguration left, TimerConfiguration right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            TimerConfiguration target = obj as TimerConfiguration;
            if (target == null)
            {
                return false;
            }

            return this == target;
        } 
        #endregion

        #region TargetTimeForJustOnce
        private DateTime targetTimeForJustOnce = DateTime.Now;
        /// <summary>
        /// TargetTimeForJustOnce 仅仅为RichTimerType.JustOnce类型时，该设置才有效
        /// </summary>
        public DateTime TargetTimeForJustOnce
        {
            get { return targetTimeForJustOnce; }
            set { targetTimeForJustOnce = value; }
        } 
        #endregion
    }
}
