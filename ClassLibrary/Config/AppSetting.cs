using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ClassLibrary.Config
{
    public static class AppSetting
    {
        private static bool? debug = null;
        /// <summary>
        /// 读取配置文件中所有的Key值 判断当前状态是否为调试模式
        /// </summary>
        public static bool Debug
        {
            get
            {
                if (debug == null)
                {
                    List<string> keys = new List<string>();
                    keys.AddRange(ConfigurationManager.AppSettings.AllKeys);
                    if (keys.Contains("Debug"))
                    {
                        debug = ConfigurationManager.AppSettings["Debug"].ToUpper() == "TRUE";
                    }
                    else
                    {
                        debug = false;
                    }
                }

                return debug.Value;
            }
        }
    }
}
