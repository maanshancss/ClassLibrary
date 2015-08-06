using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Threading.Engines
{
    /// <summary>
    /// ICycleEngine �ں�̨�߳��н��м��ѭ��������
    /// zhuweisky 2006.12.21
    /// </summary>
    public interface ICycleEngine
    {
        /// <summary>
        /// Start ������̨�����߳�
        /// </summary>
        void Start();

        /// <summary>
        /// Stop ֹͣ��̨�����̣߳�ֻ�е��̰߳�ȫ�˳��󣬸÷����ŷ���
        /// </summary>
        void Stop();

        /// <summary>
        /// IsRunning �����Ƿ�������
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// DetectSpanInSecs ���������ѯ�ļ����DetectSpanInSecs=0����ʾ�޼�϶�������棻DetectSpanInSecsС��0���ʾ��ʹ�����档Ĭ��ֵΪ0��
        /// </summary>
        int DetectSpanInSecs { get;set; }

        /// <summary>
        /// EngineStopped �����������б�Ϊֹͣ״̬ʱ�����������¼���������쳣ֹͣ�����¼�����Ϊ�쳣���󣬷����¼�����Ϊnull��
        /// </summary>
        event CbGeneric<Exception> EngineStopped;
    }    
}
