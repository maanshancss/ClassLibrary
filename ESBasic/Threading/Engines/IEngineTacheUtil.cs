using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Engine
{
    /// <summary>
    /// IEngineTacheUtil ����IEngineTacheͨ��IEngineTacheUtil��������
    /// �������滷���������д�����ݣ��������滷����ȡ������
    /// </summary>
    public interface IEngineTacheUtil
    {
        void   RegisterObject(string name, object obj);
        object GetObject(string name);
        void   Remove(string name);
        void   Clear();
    }
}
