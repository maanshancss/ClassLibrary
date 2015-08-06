using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Engine
{
    /// <summary>
    /// IEngineTacheUtil 各个IEngineTache通过IEngineTacheUtil共享数据
    /// 上游引擎环可以向其中存放数据，下游引擎环从中取用数据
    /// </summary>
    public interface IEngineTacheUtil
    {
        void   RegisterObject(string name, object obj);
        object GetObject(string name);
        void   Remove(string name);
        void   Clear();
    }
}
