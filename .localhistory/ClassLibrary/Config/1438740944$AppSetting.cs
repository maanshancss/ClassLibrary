using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Config
{
  private static bool? debug = null;
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
