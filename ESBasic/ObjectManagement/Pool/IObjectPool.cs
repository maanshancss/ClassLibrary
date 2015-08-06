using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.ObjectManagement.Pool
{
    /// <summary>
    /// IObjectPool ����ء�
    /// zhuweisky 2008.06.13
    /// </summary>
    public interface IObjectPool<TObject> where TObject : class
    {
        /// <summary>
        /// MinObjectCount �����������ͬʱ���ڵĶ�������
        /// </summary>
        int MinObjectCount { get;set; }

        /// <summary>
        /// MaxObjectCount ����������ͬʱ���ڵĶ�������
        /// </summary>
        int MaxObjectCount { get;set; }

        /// <summary>
        /// DetectSpanInMSecs ������û�п��еĶ����������ѴﵽMaxObjectCountʱ�������ʱ����Rent���ã�������ж����ʱ������
        /// Ĭ��ֵΪ10ms�� 
        /// </summary>
        int DetectSpanInMSecs { get;set; }

        /// <summary>
        /// PooledObjectCreator ���ڴ������ж���Ĵ�������Ĭ��ΪDefaultPooledObjectCreator
        /// </summary>
        IPooledObjectCreator<TObject> PooledObjectCreator { set; }

        void Initialize();

        TObject Rent();
        void GiveBack(TObject obj);
    }
}
