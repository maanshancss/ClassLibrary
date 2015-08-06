using System;
using System.Collections.Generic;

namespace ESBasic.ObjectManagement.Trees.Multiple
{
	/// <summary>
	/// IMTreeVal ���һ��������Ҫ�����ڶ�����Ľڵ��У���ö������ʵ��IMTreeVal�ӿڡ�
	/// zhuweisky 2005.07.28
	/// </summary>	
	public interface IMTreeVal
	{
        string CurrentID { get;}
        string FatherID { get;}

        /// <summary>
        /// DepthIndex ��ǰ�ڵ��ڶ�����е����������Root���������Ϊ0��
        /// ������ʹ������������뷵��-1������
        /// </summary>
        int DepthIndex { get; }
	}
}
