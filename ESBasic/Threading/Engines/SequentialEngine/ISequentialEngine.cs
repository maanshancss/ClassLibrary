using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Threading.Engines
{
    /// <summary>
    /// ISequentialEngine ���ڽ�����IEngineTache��װ�����γ�һ�����棬��˳��ִ�С�ͬʱ���ݸ��������¼���Ϣ
    /// </summary>
    public interface ISequentialEngine 
    {
        /// <summary>
        /// Initialize ��ʼ�����档������ÿ�����滷��Initialize������
        /// </summary>
        void Initialize(IList<IEngineTache> tacheList, bool has_NecceryEnder);        

        /// <summary>
        /// Excute ��������
        /// </summary>
        void Excute();

        /// <summary>
        /// Pause ��ͣ����
        /// </summary>
        void Pause();

        /// <summary>
        /// Continue ������������
        /// </summary>
        void Continue();

        /// <summary>
        /// Stop ֹͣ����
        /// </summary>
        void Stop();

        IEngineTache GetEngineTache(int tacheID);

        /// <summary>
        /// Running �����Ƿ���������
        /// </summary>
        bool Running { get;}
       
        /// <summary>
        /// PartProgressPublished ����IEngineTache��ִ�н��ȱ仯
        /// </summary>
        event CbProgress  PartProgressPublished;         

        /// <summary>
        /// TitleChanged �����ӵ��������滷������ʱ�������������滷��Title
        /// </summary>
        event CbSimpleStr TitleChanged;

        /// <summary>
        /// EngineDisruptted ���������жϣ��¼�����˵���ж�ԭ��
        /// </summary>
        event CbSimpleStr EngineDisruptted; 

        event CbSimple    EngineCompleted;
        event CbSimpleObj EngineStatusChanged;
        event CbSimpleStr MessagePublished;
        event CbSimpleStr IgnoredMessagePublished;
    }
}
