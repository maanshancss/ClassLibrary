using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Threading.Engines
{
    /// <summary>
    /// IEngineTache 用于表示引擎中可拆卸的一部分
    /// </summary>
    public interface IEngineTache
    {
        /// <summary>
        /// IsActive 表示该引擎环是否正在运行中    
        /// </summary>
        bool IsActive { get; }    

        void Initialize(IEngineTacheUtil util);
        bool Excute(out string failureCause); 
        void Pause();
        void Continue();
        void Stop();

        string Title { get; }
        int EngineTacheID { get;}

        event CbSimpleStr MessagePublished;
        event CbProgress  ProgressPublished;
        event CbSimpleStr IgnoredMessagePublished;
        event CbSimpleObj EngineStatusChanged;
    }
}
