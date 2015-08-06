using System;
using System.Collections.Generic ;
using System.Threading ;
using ESBasic;
using ESBasic.Threading.Engines;

namespace ESBasic.Threading.Application
{
	/// <summary>
    /// IOnLineChecker ���ڶ�ʱ���״̬�������ָ����ʱ������û�м���id������Ϊid��ʱ��
    /// ע�⣺���DetectSpanInSecs<=0��ʾ�����ж�ʱ���
	/// ���ߣ���ΰ sky.zhuwei@163.com 
	/// </summary>
    public interface IOnLineChecker : ICycleEngine
	{	
		void RegisterOrActivate(string id) ;
        void Unregister(string id); //��������UnregisterUserʱ��������CheckSomeOneDisConnected�¼�

		event CbSimpleStr  SomeTimeOuted ; //���ǵ���ʱ�����û�����ʱ�Ŵ���
    }  
}
