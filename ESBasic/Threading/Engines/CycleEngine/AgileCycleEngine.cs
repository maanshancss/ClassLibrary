using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Threading.Engines
{
    /// <summary>
    /// AgileCycleEngine ͨ�����ʹ�õ�ѭ������
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
        /// EngineAction ִ�����涯��������false��ʾֹͣ���档
        /// ע�⣬�÷��������׳��쳣������ᵼ������ֹͣ���У�ѭ���߳������쳣�˳�����
        /// </summary>       
        bool EngineAction() ;
    }

    
}
