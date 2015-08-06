using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Text;
using System.IO;
using System.Threading;

namespace MyUtility.BS
{
    public  class WebScript
    {


    #region  Static Property Get BaseUrl(��̬���Ի�ȡURL��ַ)
        /**//// <summary>
        /// �����̬���Եĵ��ñ��������´��뷽������
        /// �������:
        /// Response.Write(UIHelper.BaseUrl);
        /// </summary>
        public static string BaseUrl
        {
            get
            {
                //strBaseUrl���ڴ洢URL��ַ
                string strBaseUrl = "";
                //��ȡ��ǰHttpContext�µĵ�ַ
                strBaseUrl += "http://" + HttpContext.Current.Request.Url.Host;
                //����˿ڲ���80�Ļ�����ô��������˿�
                if (HttpContext.Current.Request.Url.Port.ToString() != "80")
                {
                    strBaseUrl += ":" + HttpContext.Current.Request.Url.Port;
                }
                strBaseUrl += HttpContext.Current.Request.ApplicationPath;

                return strBaseUrl + "/";
            }
        }
        #endregion

        #region  Alert()
        /**//// <summary>
        /// �򵥵����Ի�����
        /// �������:
        /// UIHelper.Alert(this.Page,"OKOK");
        /// 
        /// 
        /// </summary>
        /// <param name="pageCurrent">
        /// ��ǰ��ҳ��
        /// </param>
        /// <param name="strMsg">
        /// ������Ϣ������
        /// </param>
        public static void Alert(System.Web.UI.Page pageCurrent, string strMsg)
        {
            //Replace \n
            strMsg = strMsg.Replace("\n", "file://n/");
            //Replace \r
            strMsg = strMsg.Replace("\r", "file://r/");
            //Replace "
            strMsg = strMsg.Replace("\"", "\\\"");
            //Replace '
            strMsg = strMsg.Replace("\'", "\\\'");

            pageCurrent.ClientScript.RegisterStartupScript(pageCurrent.GetType(),
                System.Guid.NewGuid().ToString(),
                "<script>window.alert('" + strMsg + "')</script>"
                );

            //���´����Ǽ���.net1.1�汾�ģ�������2.0ʱ����API�͹�ʱ��
            //pageCurrent.RegisterStartupScript(
            //    System.Guid.NewGuid().ToString(),
            //    "<script>window.alert('" + strMsg + "')</script>"
            //    );
        }
        public static void Alert(System.Web.UI.Page pageCurrent, string strMsg, string GoBackUrl)
        {
            //Replace \n
            strMsg = strMsg.Replace("\n", "file://n/");
            //Replace \r
            strMsg = strMsg.Replace("\r", "file://r/");
            //Replace "
            strMsg = strMsg.Replace("\"", "\\\"");
            //Replace '
            strMsg = strMsg.Replace("\'", "\\\'");

            pageCurrent.ClientScript.RegisterStartupScript(pageCurrent.GetType(),
                System.Guid.NewGuid().ToString(),
                "<script>window.alert('" + strMsg + "');location='" + GoBackUrl + "'</script>"
                );

            //���´����Ǽ���.net1.1�汾�ģ�������2.0ʱ����API�͹�ʱ��
            //pageCurrent.RegisterStartupScript(
            //    System.Guid.NewGuid().ToString(),
            //    "<script>window.alert('" + strMsg + "')</script>"
            //    );
        }


        #endregion

        #region ScrollMessage
        /**//// <summary>
        /// �򵥵Ĺ�����Ϣ��
        /// �������
        ///         UIHelper.ScrollMessage(this.Page, "����������");
        /// </summary>
        /// <param name="pageCurrent">
        /// ��ǰҳ��
        /// </param>
        /// <param name="strMsg">
        /// Ҫ��������Ϣ
        /// </param>
        public static string ScrollMessage(string strMsg)
        {
            //Replace \n
            strMsg = strMsg.Replace("\n", "file://n/");
            //Replace \r
            strMsg = strMsg.Replace("\r", "file://r/");
            //Replace "
            strMsg = strMsg.Replace("\"", "\\\"");
            //Replace '
            strMsg = strMsg.Replace("\'", "\\\'");


            StringBuilder sb = new StringBuilder();
            sb.Append("<MARQUEE>");
            sb.Append(strMsg);
            sb.Append("</MARQUEE>");
            return sb.ToString();
            //pageCurrent.ClientScript.RegisterStartupScript(pageCurrent.GetType(),
            //        System.Guid.NewGuid().ToString(),sb.ToString());
        }


        /**//// <summary>
        /// ָ���������ֵ���ϸ����
        /// 
        /// </summary>
        /// <param name="pageCurrent">
        /// ��ǰ��ҳ��
        /// </param>
        /// <param name="strMsg">
        /// Ҫ����������
        /// </param>
        /// <param name="aligh">
        /// align�����趨���Ļ��λ�ã����󡢾��С����ҡ����ϺͿ�������λ��
        /// left center  right top bottom 
        /// </param>
        /// <param name="bgcolor">
        /// �����趨���Ļ�ı�����ɫ��һ����ʮ������������#CCCCCC 
        /// </param>
        /// <param name="direction">
        /// �����趨���Ļ�Ĺ����������������ҡ����ϡ�����
        /// left|right|up|down 
        /// </param>
        /// <param name="behavior">
        /// �����趨�����ķ�ʽ����Ҫ�����ַ�ʽ��scroll slide��alternate
        /// 
        /// </param>
        /// <param name="height">
        /// �����趨������Ļ�ĸ߶�
        /// 
        /// </param>
        /// <param name="hspace">
        /// ���趨������Ļ�Ŀ��
        /// </param>
        /// <param name="scrollamount">
        /// �����趨���Ļ�Ĺ�������
        /// </param>
        /// <param name="scrolldelay">
        /// �����趨��������֮����ӳ�ʱ��
        /// </param>
        /// <param name="width"></param>
        /// <param name="vspace">
        /// �ֱ������趨������Ļ�����ұ߿�����±߿�Ŀ��
        /// 
        /// </param>
        /// <param name="loop">
        /// �����趨�����Ĵ�������loop=-1��ʾһֱ������ȥ��ֱ��ҳ�����
        /// </param>
        /// <param name="MarqueejavascriptPath">
        /// �ű��Ĵ��λ��
        /// </param>
        /// <returns></returns>
        public static string ScrollMessage(System.Web.UI.Page pageCurrent, string strMsg, string aligh, string bgcolor,
                                    string direction, string behavior, string height, string hspace,
                                    string scrollamount, string scrolldelay, string width, string vspace, string loop,
                                    string MarqueejavascriptPath)
        {
            StreamReader sr = new StreamReader(pageCurrent.MapPath(MarqueejavascriptPath));
            StringBuilder sb = new StringBuilder();
            string line;
            try
            {
                while ((line = sr.ReadLine()) != null)
                {
                    sb.AppendLine(line);

                }
                sr.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            sb.Replace("$strMessage", strMsg);
            sb.Replace("$aligh", aligh);
            sb.Replace("$bgcolor", bgcolor);
            sb.Replace("$direction", direction);
            sb.Replace("$behavior", behavior);
            sb.Replace("$height", height);
            sb.Replace("$hspace", hspace);
            sb.Replace("$scrollamount", scrollamount);
            sb.Replace("$scrolldelay", scrolldelay);
            sb.Replace("$width", width);
            sb.Replace("$vspace", vspace);
            sb.Replace("$loop", loop);
            //pageCurrent.ClientScript.RegisterStartupScript(pageCurrent.GetType(),
            //            System.Guid.NewGuid().ToString(), sb.ToString());
            return sb.ToString();
        }

        #endregion

        #region  Redirect()
        /**//// <summary>
        /// Add the javascript method to redirect page on client
        /// Created : Wang Hui, May 18,2006
        /// Modified: 
        /// </summary>
        /// <param name="pageCurrent"></param>
        /// <param name="strPage"></param>
        public static void Redirect(System.Web.UI.Page pageCurrent, string strPage)
        {
            pageCurrent.ClientScript.RegisterStartupScript(pageCurrent.GetType(),
                    System.Guid.NewGuid().ToString(),
                    "<script>window.location.href='" + strPage + "'</script>"
                    );

            //���·����Ǽ���1.1��,2.0��ʱ
            //pageCurrent.RegisterStartupScript(
            //    System.Guid.NewGuid().ToString(),
            //    "<script>window.location.href='" + strPage + "'</script>"
            //    );
        }
        /**//// <summary>
        /// ��Ҫ�����������п�ܵ�ҳ��
        /// </summary>
        /// <param name="pageCurrent">��ǰҳ����this.page</param>
        /// <param name="strPage">Ҫ������ҳ��</param>
        public static void RedirectFrame(System.Web.UI.Page pageCurrent, string strPage)
        {
            pageCurrent.ClientScript.RegisterStartupScript(pageCurrent.GetType(),
                    System.Guid.NewGuid().ToString(),
                    "<script>window.top.location.href='" + strPage + "'</script>"
                    );

            //���·����Ǽ���1.1��,2.0��ʱ
            //pageCurrent.RegisterStartupScript(
            //    System.Guid.NewGuid().ToString(),
            //    "<script>window.location.href='" + strPage + "'</script>"
            //    );
        }

        #endregion

        #region AddConfirm
        /**//// <summary>
        /// Add the confirm message to button
        /// Created : GuangMing Chu 1,1,2007
        /// Modified: GuangMing Chu 1,1,2007
        /// Modified: GuangMing Chu 1,1,2007
        /// ������ã�
        ///    UIHelper.AddConfirm(this.Button1, "���Ҫɾ�ˣ���");
        /// ��ȷ����ť�ͻ�ִ���¼��еĴ��룬��ȡ������
        /// </summary>
        /// <param name="button">The control, must be a button</param>
        /// <param name="strMsg">The popup message</param>
        public static void AddConfirm(System.Web.UI.WebControls.Button button, string strMsg)
        {
            strMsg = strMsg.Replace("\n", "file://n/");
            strMsg = strMsg.Replace("\r", "file://r/");
            strMsg = strMsg.Replace("\"", "\\\"");
            strMsg = strMsg.Replace("\'", "\\\'");
            button.Attributes.Add("onClick", "return confirm('" + strMsg + "')");
        }

        /**//// <summary>
        /// Add the confirm message to button
        /// Created : GuangMing Chu, 1 1,2007
        /// Modified: GuangMing Chu, 1 1,2007
        /// Modified:
        /// �������:
        ///       UIHelper.AddConfirm(this.Button1, "���Ҫɾ�ˣ���");
        /// ��ȷ����ť�ͻ�ִ���¼��еĴ��룬��ȡ������
        ///      
        /// </summary>
        /// <param name="button">The control, must be a button</param>
        /// <param name="strMsg">The popup message</param>
        public static void AddConfirm(System.Web.UI.WebControls.ImageButton button, string strMsg)
        {
            strMsg = strMsg.Replace("\n", "file://n/");
            strMsg = strMsg.Replace("\r", "file://r/");
            strMsg = strMsg.Replace("\"", "\\\"");
            strMsg = strMsg.Replace("\'", "\\\'");
            button.Attributes.Add("onClick", "return confirm('" + strMsg + "')");
        }

        /**//// <summary>
        /// Add the confirm message to one column of gridview
        /// ������ã�
        ///         UIHelper myHelp = new UIHelper();
        ///         myHelp.AddConfirm(this.GridView1,1, "ok");
        /// ��ʹ��ʱע�⣬�˷����ĵ��ñ���ʵ����������
        /// </summary>
        /// <param name="grid">The control, must be a GridView</param>
        /// <param name="intColIndex">The column index. It's usually the column which has the "delete" button.</param>
        /// <param name="strMsg">The popup message</param>
        public static void AddConfirm(System.Web.UI.WebControls.GridView grid, int intColIndex, string strMsg)
        {
            strMsg = strMsg.Replace("\n", "file://n/");
            strMsg = strMsg.Replace("\r", "file://r/");
            strMsg = strMsg.Replace("\"", "\\\"");
            strMsg = strMsg.Replace("\'", "\\\'");
            for (int i = 0; i < grid.Rows.Count; i++)
            {
                grid.Rows[i].Cells[intColIndex].Attributes.Add("onclick", "return confirm('" + strMsg + "')");
            }
        }
        #endregion

        #region AddShowDialog
        /**//// <summary>
        /// Add the javascript method showModalDialog to button
        /// ΪButton��ť����һ����������Ի���
        /// �������
        ///         UIHelper.AddShowDialog(this.Button1, "www.sina.com.cn", 300, 300);
        /// 
        /// 
        /// 
        /// </summary>
        /// <param name="button">The control, must be a button</param>
        /// <param name="strUrl">The page url, including query string</param>
        /// <param name="intWidth">Width of window</param>
        /// <param name="intHeight">Height of window</param>
        public static void AddShowDialog(System.Web.UI.WebControls.Button button, string strUrl, int intWidth, int intHeight)
        {
            string strScript = "";
            strScript += "var strFeatures = 'dialogWidth=" + intWidth.ToString() + "px;dialogHeight=" + intHeight.ToString() + "px;center=yes;help=no;status=no';";
            strScript += "var strName ='';";

            if (strUrl.Substring(0, 1) == "/")
            {
                strUrl = strUrl.Substring(1, strUrl.Length - 1);
            }

            strUrl = BaseUrl + "DialogFrame.aspx?URL=" + strUrl;

            strScript += "window.showModalDialog(\'" + strUrl + "\',window,strFeatures);return false;";

            button.Attributes.Add("onClick", strScript);
        }
        #endregion

        #region  AddShowDialog
        /**//// <summary>
        /// Add the javascript method showModalDialog to button
        /// </summary>
        /// <param name="button">The control, must be a link button</param>
        /// <param name="strUrl">The page url, including query string</param>
        /// <param name="intWidth">Width of window</param>
        /// <param name="intHeight">Height of window</param>
        public static void AddShowDialog(System.Web.UI.WebControls.LinkButton button, string strUrl, int intWidth, int intHeight)
        {
            string strScript = "";
            strScript += "var strFeatures = 'dialogWidth=" + intWidth.ToString() + "px;dialogHeight=" + intHeight.ToString() + "px;center=yes;help=no;status=no';";
            strScript += "var strName ='';";

            if (strUrl.Substring(0, 1) == "/")
            {
                strUrl = strUrl.Substring(1, strUrl.Length - 1);
            }

            strUrl = BaseUrl + "DialogFrame.aspx?URL=" + strUrl;

            strScript += "window.showModalDialog(\'" + strUrl + "\',strName,strFeatures);return false;";

            button.Attributes.Add("onClick", strScript);
        }
        #endregion

        #region  AddShowDialog
        /**//// <summary>
        /// Add the javascript method showModalDialog to button
        /// </summary>
        /// <param name="button">The control, must be a button</param>
        /// <param name="strUrl">The page url, including query string</param>
        /// <param name="intWidth">Width of window</param>
        /// <param name="intHeight">Height of window</param>
        public static void AddShowDialog(System.Web.UI.WebControls.ImageButton button, string strUrl, int intWidth, int intHeight)
        {
            string strScript = "";
            strScript += "var strFeatures = 'dialogWidth=" + intWidth.ToString() + "px;dialogHeight=" + intHeight.ToString() + "px;center=yes;help=no;status=no';";
            strScript += "var strName ='';";

            if (strUrl.Substring(0, 1) == "/")
            {
                strUrl = strUrl.Substring(1, strUrl.Length - 1);
            }

            strUrl = BaseUrl + "DialogFrame.aspx?URL=" + strUrl;

            strScript += "window.showModalDialog(\'" + strUrl + "\',window,strFeatures);return false;";

            button.Attributes.Add("onClick", strScript);
        }
        #endregion
        #region ClearTextBox
        /**//// <summary>
        /// ��ѡ����TextBoxֵ���
        /// </summary>
        /// <param name="myTextBox"></param>
        public static void ClearTextBox(System.Web.UI.WebControls.TextBox myTextBox)
        {
            myTextBox.Attributes.Add("onclick", "this.value=''");
        }
        #endregion

        #region OpenWindow
        /**//// <summary>
        /// Use "window.open" to popup the window
        /// Created : Wang Hui, Feb 24,2006
        /// Modified: Wang Hui, Feb 24,2006
        /// �򿪴���ĶԻ�����
        /// ������ã�
        /// 
        /// 
        ///         UIHelper.OpenWindow(this.Page, "www.sina.com.cn", 400, 300);
        ///         UIHelper.ShowDialog(this.Page, "lsdjf.com", 300, 200);
        /// 
        /// 
        /// 
        /// </summary>
        /// <param name="strUrl">The url of window, start with "/", not "http://"</param>
        /// <param name="intWidth">Width of popup window</param>
        /// <param name="intHeight">Height of popup window</param>
        public static void OpenWindow(System.Web.UI.Page pageCurrent, string strUrl, int intWidth, int intHeight, int intLeft, int intTop, string WinName)
        {
            #region �ϰ汾
            //string strScript = "";
            //strScript += "var strFeatures = 'width:" + intWidth.ToString() + "px;height:" + intHeight.ToString() + "px';";
            //strScript += "var strName ='__WIN';";
            /**/////strScript += "alert(strFeatures);";

            ////--- Add by Wang Hui on Feb 27
            //if (strUrl.Substring(0, 1) == "/")
            //{
            //    strUrl = strUrl.Substring(1, strUrl.Length - 1);
            //}
            /**/////--- End Add by Wang Hui on Feb 27

            //strUrl = BaseUrl + strUrl;

            //strScript += "window.open(\"" + strUrl + "\",strName,strFeatures);";

            //pageCurrent.RegisterStartupScript(
            //    System.Guid.NewGuid().ToString(),
            //    "<script language='javascript'>" + strScript + "</script>"
            //    );


            //pageCurrent.RegisterStartupScript(
            //    System.Guid.NewGuid().ToString(),
            //    "<script language='javascript'>" + strScript + "</script>"
            //    );
            #endregion

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"myleft={0};mytop={1};", intLeft.ToString(), intTop.ToString());
            sb.AppendLine();
            sb.AppendFormat(@"settings='top=' + mytop + ',left=' + myleft + ',width={0},height={1},location=no,directories=no,menubar=no,toolbar=no,status=no,scrollbars=no,resizable=no,fullscreen=no';",
                             intWidth.ToString(), intHeight.ToString());
            sb.AppendLine();
            sb.AppendFormat(@"window.open('{0}','{1}', settings);", strUrl, WinName);

            pageCurrent.ClientScript.RegisterStartupScript(pageCurrent.GetType(),
                    System.Guid.NewGuid().ToString(),
                    "<script language='javascript'>" + sb.ToString() + "</script>"
                    );

        }
        #endregion

        #region ShowDialog
        /**//// <summary>
        /// Use "window.showModalDialog" to show the dialog
        /// Created : Wang Hui, Feb 24,2006
        /// Modified: Wang Hui, Feb 27,2006
        /// �˴�����ģʽ����,��C/S�ṹ�е�ģʽ������һ�µ�
        /// </summary>
        /// <param name="strUrl">The url of dialog, start with "/", not "http://"</param>
        /// <param name="intWidth">Width of dialog</param>
        /// <param name="intHeight">Height of dialog</param>
        public static void ShowDialog(System.Web.UI.Page pageCurrent, string strUrl, int intWidth, int intHeight)
        {
            string strScript = "";
            strScript += "var strFeatures = 'dialogWidth=" + intWidth.ToString() + "px;dialogHeight=" + intHeight.ToString() + "px;center=yes;help=no;status=no';";
            strScript += "var strName ='';";

            //--- Add by Wang Hui on Feb 27 
            if (strUrl.Substring(0, 1) == "/")
            {
                strUrl = strUrl.Substring(1, strUrl.Length - 1);
            }
            //--- End Add by Wang Hui on Feb 27

            strUrl = BaseUrl + "DialogFrame.aspx?URL=" + strUrl;

            //strScript += "window.showModalDialog(\"" + strUrl + "\",strName,strFeatures);";
            strScript += "window.showModalDialog(\"" + strUrl + "\",window,strFeatures); ";

            pageCurrent.ClientScript.RegisterStartupScript(
                pageCurrent.GetType(), System.Guid.NewGuid().ToString(),
                "<script language='javascript'>" + strScript + "</script>"
                );
        }
        #endregion

        #region CloseWindows
        /**//// <summary>
        /// �رմ���,û���κ���ʾ�Ĺرմ���
        /// </summary>
        /// <param name="pageCurrent"></param>
        public static void CloseWindows(System.Web.UI.Page pageCurrent)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script>window.opener=null;window.close();</script>");
            pageCurrent.ClientScript.RegisterStartupScript(pageCurrent.GetType(),
                System.Guid.NewGuid().ToString(), sb.ToString());
        }



        /**//// <summary>
        /// ����ʾ��Ϣ�Ĺرմ���
        /// </summary>
        /// <param name="pageCurrent"></param>
        /// <returns></returns>
        public static void CloseWindows(System.Web.UI.Page pageCurrent, string strMessage)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script>if(confirm(\"" + strMessage + "\")==true){window.close();}</script>");
            pageCurrent.ClientScript.RegisterStartupScript(pageCurrent.GetType(),
                                System.Guid.NewGuid().ToString(), sb.ToString());
        }
        /**//// <summary>
        /// �еȴ�ʱ��Ĺرմ���
        /// </summary>
        /// <param name="pageCurrent"></param>
        /// <param name="WaitTime">�ȴ�ʱ�䣬�Ժ���Ϊ������λ</param>
        public static void CloseWindows(System.Web.UI.Page pageCurrent, int WaitTime)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script language=\"javascript\">");
            //������й��ܺ�û������ʾ����
            sb.Append("window.opener=null;");
            sb.Append("setTimeout");
            sb.Append("(");
            sb.Append("'");
            sb.Append("window.close()");
            sb.Append("'");

            sb.Append(",");
            sb.Append(WaitTime.ToString());
            sb.Append(")");
            sb.Append("</script>");
            pageCurrent.ClientScript.RegisterStartupScript(pageCurrent.GetType(),
                        System.Guid.NewGuid().ToString(), sb.ToString());

        }
        #endregion

        #region  ShowStatusBar
        public static void ShowStatus(System.Web.UI.Page pageCurrent, string StatusString)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script language=\"javascript\">");
            sb.Append("window.status=");
            sb.Append("\"");
            sb.Append(StatusString);
            sb.Append("\"");
            sb.Append("</script>");
            pageCurrent.ClientScript.RegisterStartupScript(pageCurrent.GetType(),
                System.Guid.NewGuid().ToString(), sb.ToString());
        }
        #endregion

        #region PlayMediaFile
        /**//// <summary>
        /// ����Media����mp3���Ӱ�ļ�
        /// </summary>
        /// <param name="pageCurrent">
        /// ��ǰ��ҳ�����
        /// </param>
        /// <param name="PlayFilePath">
        /// �����ļ���λ��
        /// </param>
        /// <param name="MediajavascriptPath">
        /// Mediajavascript�Ľű�λ��
        /// </param>
        /// <param name="enableContextMenu">
        /// �Ƿ����ʹ���Ҽ�
        /// ָ���Ƿ�ʹ�Ҽ��˵���Ч
        /// ָ���Ҽ��Ƿ����,Ĭ��Ϊ0������
        /// ָ��Ϊ1ʱ���Ǻ���
        /// </param>
        /// <param name="uiMode">
        /// �������Ĵ�С��ʾ
        /// None��mini����full��ָ��Windowsý�岥�������������ʾ
        /// </param>
        public static string PlayMediaFile(System.Web.UI.Page pageCurrent,
                        string PlayFilePath, string MediajavascriptPath,
                        string enableContextMenu, string uiMode)
        {
            StreamReader sr = new StreamReader(pageCurrent.MapPath(MediajavascriptPath));
            StringBuilder sb = new StringBuilder();
            string line;
            try
            {
                while ((line = sr.ReadLine()) != null)
                {
                    sb.AppendLine(line);

                }
                sr.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            sb.Replace("$URL", pageCurrent.MapPath(PlayFilePath));
            sb.Replace("$enableContextMenu", enableContextMenu);
            sb.Replace("$uiMode", uiMode);
            //pageCurrent.ClientScript.RegisterStartupScript(pageCurrent.GetType(),
            //            System.Guid.NewGuid().ToString(), sb.ToString());
            return sb.ToString();
        }
        #endregion

        #region ShowProgBar
        /**//// <summary>
        /// ��Ҫʵ�ֽ������Ĺ��ܣ���δ���ĵ��þ�Ҫʵ�ֽ��ȵĵ���
        /// ʵ����Ҫ����
        /// default.aspx.cs�ǵ���ҳ��
        /// ����page_load�¼���
        ///            UIHelper myUI = new UIHelper();
        ///            Response.Write(myUI.ShowProgBar(this.Page,"../JS/progressbar.htm"));
        ///            Thread thread = new Thread(new ThreadStart(ThreadProc));
        ///            thread.Start();
        ///            LoadData();//load���� 
        ///            thread.Join();
        ///            Response.Write("OK");
        /// 
        /// ����ThreadProc����Ϊ
        ///     public void ThreadProc()
        ///    {
        ///    string strScript = "<script>setPgb('pgbMain','{0}');</script>";
        ///    for (int i = 0; i <= 100; i++)
        ///     {
        ///        System.Threading.Thread.Sleep(10);
        ///        Response.Write(string.Format(strScript, i));
        ///        Response.Flush();
        ///     }
        ///    }
        /// ����LoadData()
        ///     public void LoadData()
        ///        {
        ///            for (int m = 0; m < 900; m++)
        ///            {
        ///                for (int i = 0; i < 900; i++)
        ///                {
        ///
        ///                }
        ///            }
        ///        }
        /// 
        /// </summary>
        /// <param name="pageCurrent"></param>
        /// <param name="ShowProgbarScript"></param>
        /// <returns></returns>
        public static string ShowProgBar(System.Web.UI.Page pageCurrent, string ShowProgbarScript)
        {
            StreamReader sr = new StreamReader(pageCurrent.MapPath(ShowProgbarScript), System.Text.Encoding.Default);
            StringBuilder sb = new StringBuilder();
            string line;
            try
            {
                while ((line = sr.ReadLine()) != null)
                {
                    sb.AppendLine(line);

                }
                sr.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            //pageCurrent.ClientScript.RegisterStartupScript(pageCurrent.GetType(),
            //            System.Guid.NewGuid().ToString(), sb.ToString());
            return sb.ToString();
        }
        #endregion

        #region fixedHeader
        public static string fixedHeader()
        {
            StringBuilder s = new StringBuilder();
            s.Append(@"<table width='100%' border='1' cellspacing='0' style='MARGIN-TOP:-2px'>");
            s.Append(@"<TR class='fixedHeaderTr' style='BACKGROUND:navy;COLOR:white'>");
            s.Append(@"<TD nowrap>Header A</TD>");
            s.Append(@"<TD nowrap>Header B</TD>");
            s.Append(@"<TD nowrap>Header C</TD>");
            s.Append(@"</TR>");
            for (int m = 0; m < 100; m++)
            {
                s.Append(@"<TR>");
                s.Append(@"<TD>A").Append(m).Append("</TD>");
                s.Append(@"<TD>B").Append(m).Append("</TD>");
                s.Append(@"<TD>C").Append(m).Append("</TD>");
                s.Append(@"</TR>");
            }
            s.Append(@"</table>");
            return s.ToString();
        }
        #endregion

       #region refreshPage
        public static void refreshPage(System.Web.UI.Page pageCurrent)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script language=\"javascript\">");
            sb.Append("window.location.reload(true);");
            sb.Append("</script>");
            pageCurrent.ClientScript.RegisterStartupScript(pageCurrent.GetType(),
                        System.Guid.NewGuid().ToString(), sb.ToString());

        }
        #endregion

        #region Page_revealTrans
        //����ҳ��<meta http-equiv="Page-Enter" content="revealTrans(duration=x, transition=y)">
        //�Ƴ�ҳ��<meta http-equiv="Page-Exit" content="revealTrans(duration=x, transition=y)"> 
        //�����ҳ�汻����͵���ʱ��һЩ��Ч��duration��ʾ��Ч�ĳ���ʱ�䣬����Ϊ��λ��transition��ʾʹ��������Ч��ȡֵΪ1-23:
        //  0 ������С 
        //  1 �������� 
        //  2 Բ����С
        //  3 Բ������ 
        //  4 �µ���ˢ�� 
        //  5 �ϵ���ˢ��
        //  6 ����ˢ�� 
        //  7 �ҵ���ˢ�� 
        //  8 ����Ҷ��
        //  9 ���Ҷ�� 
        //  10 ��λ���Ҷ�� 
        //  11 ��λ����Ҷ��
        //  12 ����ɢ 
        //  13 ���ҵ��м�ˢ�� 
        //  14 �м䵽����ˢ��
        //  15 �м䵽����
        //  16 ���µ��м� 
        //  17 ���µ�����
        //  18 ���ϵ����� 
        //  19 ���ϵ����� 
        //  20 ���µ�����
        //  21 ���� 
        //  22 ���� 
        //  23 ����22�����ѡ��һ��

        public static string Page_revealTrans(System.Web.UI.Page currentPage, string duration,
                                       string transition)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<meta http-equiv=\"Page-Enter\"");
            sb.Append("content=\"");
            sb.Append("revealTrans(duration=" + duration);
            sb.Append(",transition=" + transition);
            sb.Append(")\">");
            //currentPage.ClientScript.RegisterStartupScript(currentPage.GetType(),
            //        System.Guid.NewGuid().ToString(), sb.ToString());
            return sb.ToString();
        }
        #endregion

       
        /// <summary>
        /// ��ʾһ���Զ�����������
        /// </summary>
        /// <param name="page">ҳ��ָ��,һ��ΪThis</param>
        public static void ResponseScript(System.Web.UI.Page page, string script)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script language=\"javascript\"> \n");
            sb.Append(script.Trim());
            sb.Append("</script>");
            page.Response.Write(sb.ToString());
        }

        /**//// <summary>
        /// ���ÿͻ���JavaScript����
        /// </summary>
        /// <param name="page">ҳ��ָ��,һ��ΪThis</param>
        /// <param name="scriptName">������,������,��:FunA(1);</param>
        public static void CallClientScript(System.Web.UI.Page page, string scriptName)
        {
            String csname = "PopupScript";
            Type cstype = page.GetType();
            System.Web.UI.ClientScriptManager cs = page.ClientScript;
            if (!cs.IsStartupScriptRegistered(cstype, csname))
            {
                String cstext = scriptName;
                cs.RegisterStartupScript(cstype, csname, cstext, true);
            }
        }

        /**//// <summary>
        /// �����Ի���(�����Ի����css��ʧЧ)
        /// </summary>
        /// <param name="message">��ʾ��Ϣ</param>
        public static void ShowMessage(string message)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script language=\"javascript\"> \n");
            sb.Append("alert(\"" + message.Trim() + "\"); \n");
            sb.Append("</script>");

            System.Web.HttpContext.Current.Response.Write(sb.ToString());
        }

        /**//// <summary>
        /// �����Ի���(��Ӱ��css��ʽ)
        /// </summary>
        /// <param name="page">ҳ��ָ��,һ��Ϊthis</param>
        /// <param name="scriptKey">�ű���,Ψһ</param>
        /// <param name="message">��ʾ��Ϣ</param>
        public static void ShowMessage(System.Web.UI.Page page, string scriptKey, string message)
        {
            System.Web.UI.ClientScriptManager csm = page.ClientScript;
            if (!csm.IsClientScriptBlockRegistered(scriptKey))
            {
                string strScript = "alert('" + message + "');";
                csm.RegisterClientScriptBlock(page.GetType(), scriptKey, strScript, true);
            }
        }

        /**//// <summary>
        /// Ϊ�ؼ����ȷ����ʾ�Ի���
        /// </summary>
        /// <param name="Control">��Ҫ�����ʾ�ĶԻ���</param>
        /// <param name="message">��ʾ��Ϣ</param>
        public static void ShowConfirm(System.Web.UI.WebControls.WebControl Control, string message)
        {
            Control.Attributes.Add("onclick", "return confirm('" + message + "');");
        }

        /**//**/
        /**//// <summary>
        /// ��ʾһ���������ڣ���ת��Ŀ��ҳ(����)
        /// </summary>
        public static void ShowAndRedirect(string message, string url)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script language=\"javascript\"> \n");
            sb.Append("alert(\"" + message.Trim() + "\"); \n");
            sb.Append("window.location.href=\"" + url.Trim().Replace("'", "") + "\";\n");
            sb.Append("</script>");

            System.Web.HttpContext.Current.Response.Write(sb.ToString());
        }

        /**//// <summary>
        /// ��ʾһ���������ڣ����¼��ص�ǰҳ
        /// </summary>
        public static void ShowAndReLoad(string message)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script language=\"javascript\"> \n");
            sb.Append("alert(\"" + message.Trim() + "\"); \n");
            sb.Append("window.location.href=window.location.href;\n");
            sb.Append("</script>");

            System.Web.HttpContext.Current.Response.Write(sb.ToString());
        }

        /**//// <summary>
        /// ��ʾһ����������,ˢ�µ�ǰҳ(Σ�յ�,�п���������ѭ��)
        /// </summary>
        public static void ShowAndRefresh(string message)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script language=\"javascript\"> \n");
            sb.Append("alert(\"" + message.Trim() + "\"); \n");
            sb.Append("document.execCommand('Refresh')");
            sb.Append("</script>");

            System.Web.HttpContext.Current.Response.Write(sb.ToString());
        }

        /**//// <summary>
        /// ��ʾһ����������,���رյ�ǰҳ
        /// </summary>
        /// <param name="message"></param>
        public static void ShowAndClose(string message)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<script language=\"javascript\">\n");
            sb.Append("alert(\"" + message.Trim() + "\"); \n");
            sb.Append("window.close();\n");
            sb.Append("</script>\n");
            System.Web.HttpContext.Current.Response.Write(sb.ToString());
        }

        /**//// <summary>
        /// ��ʾһ����������,��ת����һҳ
        /// </summary>
        /// <param name="message"></param>
        public static void ShowPre(string message)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script language=\"javascript\"> \n");
            sb.Append("alert(\"" + message.Trim() + "\"); \n");
            sb.Append("var p=document.referrer; \n");
            sb.Append("window.location.href=p;\n");
            sb.Append("</script>");

            System.Web.HttpContext.Current.Response.Write(sb.ToString());
        }

        /**//// <summary>
        /// ҳ������
        /// </summary>
        public static void ReLoad()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script language=\"javascript\"> \n");
            sb.Append("window.location.href=window.location.href;");
            sb.Append("</script>");
            System.Web.HttpContext.Current.Response.Write(sb.ToString());

        }

        /**//// <summary>
        /// �ض���
        /// </summary>
        public static void Redirect(string url)
        {
            //string path = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":" + System.Web.HttpContext.Current.Request.Url.Port + url;
            string path = "http://" + System.Web.HttpContext.Current.Request.Url.Host + url;
            StringBuilder sb = new StringBuilder();
            sb.Append("<script language=\"javascript\"> \n");
            sb.Append(string.Format("window.location.href='{0}';", @path.Replace("'", "")));
            sb.Append("</script>");

            System.Web.HttpContext.Current.Response.Write(sb.ToString());
        }
    }
}