using System;
using System.Collections.Generic;
using System.Text;
using ESBasic.Threading.Engines;
using ESBasic.ObjectManagement;

namespace ESBasic.Threading.Application
{   
    /// <summary>
    /// CircleTaskSwitcher 循环任务切换器。将一天24小时分为多个时段，在不同的时段，会有不同的任务。当到达任务切换点时，切换器会触发切换事件。
    /// </summary>
    /// <typeparam name="TaskType">任务的类型</typeparam> 
    public class CircleTaskSwitcher<TaskType> : ICircleTaskSwitcher<TaskType> ,IEngineActor
    {
        private AgileCycleEngine agileCycleEngine ;
        private Circle<ShortTime> taskTimeCircle = new Circle<ShortTime>();
        public event CbGeneric<TaskType> TaskSwitched;      
        
        #region TaskDictionary
        private IDictionary<ShortTime, TaskType> taskDictionary = new Dictionary<ShortTime, TaskType>(); //ShortTime -- Task
        /// <summary>
        /// TaskDictionary key为任务的起始点hour，value为对应的任务。
        /// </summary>
        public IDictionary<ShortTime, TaskType> TaskDictionary
        {
            get { return taskDictionary; }
            set { taskDictionary = value; }
        } 
        #endregion  

        #region Ctor
        public CircleTaskSwitcher()
        {
            this.TaskSwitched += delegate { };
        } 
        #endregion

        #region Initialize
        public void Initialize()
        {           
            if (this.taskDictionary.Count < 2)
            {
                throw new Exception("Count of StartHour must >= 2 !");
            }

            List<ShortTime> list = new List<ShortTime>();

            foreach (ShortTime taskTime in this.taskDictionary.Keys)
            {
                list.Add(taskTime);
            }

            list.Sort();            

            this.taskTimeCircle = new Circle<ShortTime>(list);

            #region 初始化当前值
            ShortTime now = new ShortTime(DateTime.Now);
            if (now.CompareTo(this.taskTimeCircle.Tail) >= 0 || now.CompareTo(this.taskTimeCircle.Header) <0)
            {
                this.taskTimeCircle.SetCurrent(this.taskTimeCircle.Tail);
            }
            else
            {
                this.taskTimeCircle.SetCurrent(this.taskTimeCircle.Header);
                while (now.CompareTo(this.taskTimeCircle.PeekNext()) >= 0)
                {
                    this.taskTimeCircle.MoveNext();
                }
            } 
            #endregion

            this.agileCycleEngine = new AgileCycleEngine(this);
            this.agileCycleEngine.DetectSpanInSecs = 1;            
            this.agileCycleEngine.Start();
        }       
        #endregion

        #region CurrentTask
        public TaskType CurrentTask
        {
            get
            {
                return this.taskDictionary[this.taskTimeCircle.Current];
            }
        } 
        #endregion       

        #region IEngineActor 成员

        public bool EngineAction()
        {
            if (this.taskTimeCircle.PeekNext().IsOnTime(DateTime.Now, this.agileCycleEngine.DetectSpanInSecs))
            {
                this.taskTimeCircle.MoveNext();
                this.TaskSwitched(this.CurrentTask);
            }

            return true;
        }

        #endregion
    }
}
