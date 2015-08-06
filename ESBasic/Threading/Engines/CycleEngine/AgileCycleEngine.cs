using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Threading.Engines
{
    /// <summary>
    /// AgileCycleEngine 通过组合使用的循环引擎
    /// </summary>
    public sealed class AgileCycleEngine :BaseCycleEngine
    {
        private IEngineActor engineActor;

        public AgileCycleEngine(IEngineActor _engineActor)
        {
            this.engineActor = _engineActor;
        }

        protected override bool DoDetect()
        {
            return this.engineActor.EngineAction();
        }
    }

    public interface IEngineActor
    {
        /// <summary>
        /// EngineAction 执行引擎动作，返回false表示停止引擎。
        /// 注意，该方法不能抛出异常，否则会导致引擎停止运行（循环线程遭遇异常退出）。
        /// </summary>       
        bool EngineAction() ;
    }

    
}
