using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Threading.Engines
{
    /// <summary>
    /// IEngineTache ���ڱ�ʾ�����пɲ�ж��һ����
    /// </summary>
    public interface IEngineTache
    {
        /// <summary>
        /// IsActive ��ʾ�����滷�Ƿ�����������    
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
