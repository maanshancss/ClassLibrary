using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
namespace MyUtility.BS
{
  public  class WebObject
    {

        #region Cookie����
        	/// <summary>
		/// дcookieֵ
		/// </summary>
		/// <param name="strName">����</param>
		/// <param name="strValue">ֵ</param>
		public static void WriteCookie(string strName, string strValue)
		{
			HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
			if (cookie == null)
			{
				cookie = new HttpCookie(strName);
			}
			cookie.Value = strValue;
			HttpContext.Current.Response.AppendCookie(cookie);

		}
		/// <summary>
		/// дcookieֵ
		/// </summary>
		/// <param name="strName">����</param>
        /// <param name="strValue">ֵ</param>
        /// <param name="strValue">����ʱ��(����)</param>
        public static void WriteCookie(string strName, string strValue, int expires)
		{
			HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
			if (cookie == null)
			{
				cookie = new HttpCookie(strName);
			}
			cookie.Value = strValue;
			cookie.Expires = DateTime.Now.AddMinutes(expires);
			HttpContext.Current.Response.AppendCookie(cookie);

		}

		/// <summary>
		/// ��cookieֵ
		/// </summary>
		/// <param name="strName">����</param>
		/// <returns>cookieֵ</returns>
		public static string GetCookie(string strName)
		{
			if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null)
			{
				return HttpContext.Current.Request.Cookies[strName].Value.ToString();
			}

			return "";
		}
        #endregion

    }
}
