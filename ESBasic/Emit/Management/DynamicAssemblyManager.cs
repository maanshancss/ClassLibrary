using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using ESBasic.ObjectManagement.Managers;

namespace ESBasic.Emit.Management
{
    /// <summary>
    /// DynamicAssemblyManager 用于管理所有Emit的动态程序集。并且处理AppDomain.AssemblyResolve事件，确保动态程序集被正常解析。
    /// </summary>
    public class DynamicAssemblyManager
    {
        private IObjectManager<string, Assembly> assemblyManager = new ObjectManager<string, Assembly>(); //保持动态程序集

        #region Ctor
        public DynamicAssemblyManager()
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve); //处理动态程序集加载失败的情况
        }

        Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (asm.FullName == args.Name)
                {
                    return asm;
                }
            }

            return this.assemblyManager.Get(args.Name);
        }  
        #endregion

        #region RegisterAppDomain
        public void RegisterAppDomain(AppDomain domain)
        {
            domain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
        } 
        #endregion

        #region RegisterDynamicAssembly
        public void RegisterDynamicAssembly(Assembly assm)
        {
            this.assemblyManager.Add(assm.FullName, assm);
        } 
        #endregion

        #region GetDynamicAssembly
        public Assembly GetDynamicAssembly(string assmFullName)
        {
            return this.assemblyManager.Get(assmFullName);
        } 
        #endregion

        #region GetAll
        public IList<Assembly> GetAll()
        {
            return this.assemblyManager.GetAll();
        } 
        #endregion
       
    }
}
