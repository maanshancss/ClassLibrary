using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ESBasic.Widget
{    
    /// <summary>
    /// SystemIconShell 用于将系统图标绑定到ListView的Item上。
    /// </summary>
    public static class SystemIconShell
    {
        #region private
        private static uint SHGFI_ICON = 0x100;
        private static uint SHGFI_DISPLAYNAME = 0x200;
        private static uint SHGFI_TYPENAME = 0x400;
        private static uint SHGFI_ATTRIBUTES = 0x800;
        private static uint SHGFI_ICONLOCATION = 0x1000;
        private static uint SHGFI_EXETYPE = 0x2000;
        private static uint SHGFI_SYSICONINDEX = 0x4000;
        private static uint SHGFI_LINKOVERLAY = 0x8000;
        private static uint SHGFI_SELECTED = 0x10000;
        private static uint SHGFI_LARGEICON = 0x0;
        private static uint SHGFI_SMALLICON = 0x1;
        private static uint SHGFI_OPENICON = 0x2;
        private static uint SHGFI_SHELLICONSIZE = 0x4;
        private static uint SHGFI_PIDL = 0x8;
        private static uint SHGFI_USEFILEATTRIBUTES = 0x10;
        private static uint FILE_ATTRIBUTE_NORMAL = 0x80;
        private static uint LVM_FIRST = 0x1000;
        private static uint LVM_SETIMAGELIST = LVM_FIRST + 3;
        private static uint LVSIL_NORMAL = 0;
        private static uint LVSIL_SMALL = 1;

        [DllImport("Shell32.dll")]
        private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, int cbfileInfo, uint uFlags);
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public int dwAttributes;
            public string szDisplayName;
            public string szTypeName;
        }

        [DllImport("User32.DLL")]
        private static extern int SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam); 
        #endregion

        #region ListViewSysImages
        public static void ListViewSysImages(ListView AListView)
        {
            SHFILEINFO vFileInfo = new SHFILEINFO();
            IntPtr vImageList = SHGetFileInfo("", 0, ref vFileInfo, Marshal.SizeOf(vFileInfo), SHGFI_SHELLICONSIZE | SHGFI_SYSICONINDEX | SHGFI_LARGEICON);
            SendMessage(AListView.Handle, LVM_SETIMAGELIST, (IntPtr)LVSIL_NORMAL, vImageList);
            vImageList = SHGetFileInfo("", 0, ref vFileInfo, Marshal.SizeOf(vFileInfo), SHGFI_SHELLICONSIZE | SHGFI_SYSICONINDEX | SHGFI_SMALLICON);
            SendMessage(AListView.Handle, LVM_SETIMAGELIST, (IntPtr)LVSIL_SMALL, vImageList);
        } 
        #endregion

        #region FileIconIndex
        /// <summary>
        /// FileIconIndex 根据文件路径找到对应的系统图标的索引。
        /// </summary>
        /// <param name="filePath">文件必须存在！</param>       
        public static int FileIconIndex(string filePath)
        {
            SHFILEINFO vFileInfo = new SHFILEINFO();
            SHGetFileInfo(filePath, 0, ref vFileInfo, Marshal.SizeOf(vFileInfo), SHGFI_SYSICONINDEX);
            return vFileInfo.iIcon;
        } 
        #endregion

        /* sample ---
            SystemIconShell.ListViewSysImages(listView1);
            listView1.Items.Add("temp.txt", SystemIconShell.FileIconIndex(@"c:\temp\temp.txt"));

         */
    }
}
