using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.ObjectManagement.Managers
{
    /// <summary>
    /// IRefreshableCache �ܹ�����ˢ�µĻ��棬��IRefreshableCacheManagerͳһ����
    /// zhuweisky 2007.07.07
    /// </summary>
    public interface IRefreshableCache
    {
        /// <summary>
        /// RefreshSpanInSecs ��ʱˢ�µ�ʱ�������룩���������Ϊ0�����ʾ��IRefreshableCacheManager��ˢ��ʱ��ͳһ��
        /// </summary>
        int RefreshSpanInSecs { get; }

        /// <summary>
        /// LastRefreshTime ���һ��ˢ��ʱ�䡣
        /// </summary>
        DateTime LastRefreshTime { get; set; }        
        
        void Refresh();       
    }   
}
