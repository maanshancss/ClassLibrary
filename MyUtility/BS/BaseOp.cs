using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
namespace MyUtility.BS
{
 public static   class BaseOp
    {

     /// <summary>
     /// ·��ת����ת���ɾ���·����
     /// </summary>
     /// <param name="path"></param>
     /// <returns></returns>
     public static string WebPathTran(string path)
     {
         try
         {
             return HttpContext.Current.Server.MapPath(path);
         }
         catch
         {
             return path;
         }
     
     
     
     
     }











    }
}
