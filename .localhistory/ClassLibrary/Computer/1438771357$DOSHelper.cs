using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Computer
{
    public  class DOSHelper
    {
        
            Process.Start("rundll32.exe", " diskcopy.dll,DiskCopyRunDll");
        }

        public void 打开新建拨号连接()
        {
            Process.Start("rundll32.exe", " rnaui.dll,RnaWizard");
        }

        public void 打开显示属性背景()
        {
            Process.Start("rundll32.exe", " shell32.dll,Control_RunDLL desk.cpl,,0");
        }

        public void 打开显示属性屏幕保护()
        {
            Process.Start("rundll32.exe", " shell32.dll,Control_RunDLL desk.cpl,,1");
        }

        public void 打开显示属性外观()
        {
            Process.Start("rundll32.exe", " shell32.dll,Control_RunDLL desk.cpl,,2");
        }

        public void 打开显示属性属性()
        {
            Process.Start("rundll32.exe", " shell32.dll,Control_RunDLL desk.cpl,,3");
        }

        public void 打开格式化对话框()
        {
            Process.Start("rundll32.exe", " shell32.dll,SHFormatDrive");
        }

        public void 打开控制面板游戏控制器一般()
        {
            Process.Start("rundll32.exe", " shell32.dll,Control_RunDLL joy.cpl,,0");
        }

        public void 打开控制面板游戏控制器进阶()
        {
            Process.Start("rundll32.exe", " shell32.dll,Control_RunDLL joy.cpl,,1");
        }

        public void 打开控制面板键盘属性速度()
        {
            Process.Start("rundll32.exe", " shell32.dll,Control_RunDLL main.cpl @1");
        }

        public void 打开控制面板键盘属性语言()
        {
            Process.Start("rundll32.exe", " shell32.dll,Control_RunDLL main.cpl @1,,1");
        }

        public void 打开Windows打印机档案夹()
        {
            Process.Start("rundll32.exe", " shell32.dll,Control_RunDLL main.cpl @2");
        }

        public void 打开Windows字体档案夹()
        {
            Process.Start("rundll32.exe", " shell32.dll,Control_RunDLL main.cpl @3");
        }

        public void 打开控制面板输入法属性()
        {
            Process.Start("rundll32.exe", " shell32.dll,Control_RunDLL main.cpl @4");
        }

        public void 打开添加新调制解调器向导()
        {
            Process.Start("rundll32.exe", " shell32.dll,Control_RunDLL modem.cpl,,add");
        }

        public void 打开控制面板多媒体属性音频()
        {
            Process.Start("rundll32.exe", " shell32.dll,Control_RunDLL mmsys.cpl,,0");
        }

        public void 打开控制面板多媒体属性视频()
        {
            Process.Start("rundll32.exe", " shell32.dll,Control_RunDLL mmsys.cpl,,1");
        }

        public void 打开控制面板多媒体属性MIDI()
        {
            Process.Start("rundll32.exe", " shell32.dll,Control_RunDLL mmsys.cpl,,2");
        }

        public void 打开控制面板多媒体属性CD音乐()
        {
            Process.Start("rundll32.exe", " shell32.dll,Control_RunDLL mmsys.cpl,,3");
        }

        public void 打开控制面板多媒体属性设备()
        {
            Process.Start("rundll32.exe", " shell32.dll,Control_RunDLL mmsys.cpl,,4");
        }

        public void 打开控制面板声音()
        {
            Process.Start("rundll32.exe", " shell32.dll,Control_RunDLL mmsys.cpl @1");
        }

        public void 打开控制面板网络()
        {
            Process.Start("rundll32.exe", " shell32.dll,Control_RunDLL netcpl.cpl");
        }

        public void 打开控制面板密码()
        {
            Process.Start("rundll32.exe", " shell32.dll,Control_RunDLL password.cpl");
        }

        public void 打开控制面板电源管理()
        {
            Process.Start("rundll32.exe", " shell32.dll,Control_RunDLL powercfg.cpl");
        }

        public void 打开控制面板区域设置属性区域设置()
        {
            Process.Start("rundll32.exe", " shell32.dll,Control_RunDLL intl.cpl,,0");
        }

        public void 打开控制面板区域设置属性数字选项()
        {
            Process.Start("rundll32.exe", " shell32.dll,Control_RunDLL intl.cpl,,1");
        }

        public void 打开控制面板区域设置属性货币选项()
        {
            Process.Start("rundll32.exe", " shell32.dll,Control_RunDLL intl.cpl,,2");
        }

        public void 打开控制面板区域设置属性时间选项()
        {
            Process.Start("rundll32.exe", " shell32.dll,Control_RunDLL intl.cpl,,3");
        }

        public void 打开控制面板区域设置属性日期选项()
        {
            Process.Start("rundll32.exe", " shell32.dll,Control_RunDLL intl.cpl,,4");
        }

        public void 打开ODBC数据源管理器()
        {
            Process.Start("rundll32.exe", " shell32.dll,Control_RunDLL odbccp32.cpl");
        }

        public void 打开控制面板系统属性常规()
        {
            Process.Start("rundll32.exe", " shell32.dll,Control_RunDLL sysdm.cpl,,0");
        }

        public void 打开控制面板系统属性设备管理器()
        {
            Process.Start("rundll32.exe", " shell32.dll,Control_RunDLL sysdm.cpl,,1");
        }

        public void 打开控制面板系统属性硬件配置()
        {
            Process.Start("rundll32.exe", " shell32.dll,Control_RunDLL sysdm.cpl,,2");
        }

        public void 打开控制面板系统属性性能()
        {
            Process.Start("rundll32.exe", " shell32.dll,Control_RunDLL sysdm.cpl,,3");
        }

        /*shutdown -s -t 3600 -f 
        一小时后强行关机 用强行主要怕有些程序卡住 关不了机 
        -s 关机 
        -r重启 
        -f强行 
        -t 时间 
        -a 取消关机 
        -l 注销 
        -i 显示用户界面 具体是什么试试就知道了*/

        public void 关闭并重启计算机()
        {
            Process.Start("shutdown.exe", "-r");
        }

        public void 关闭计算机()
        {
            Process.Start("shutdown.exe", "-s -f");
        }
        //重载关闭计算机函数，可以设定倒计时
        public void 关闭计算机(string time)
        {
            string s = "-s -t " + time;
            Process.Start("shutdown.exe", s);
        }

        public void 注销计算机()
        {
            Process.Start("shutdown.exe", "-l");
        }

        public void 撤销关闭计算机()
        {
            Process.Start("shutdown.exe", "-a");
        }

        public void 打开桌面主旨面板()
        {
            Process.Start("rundll32.exe", " shell32.dll,Control_RunDLL themes.cpl");
        }

        public void 打开网址(string address)
        {
            Process.Start(address);
        }

        public void 运行程序(string name)
        {
            Process.Start(name);
        }

        public void 显示任务栏()
        {
            ShowWindow(FindWindow("Shell_TrayWnd", null), SW_SHOW);
        }

        public void 隐藏任务栏()
        {
            ShowWindow(FindWindow("Shell_TrayWnd", null), SW_HIDE);
        }

        public void 发送邮件(string address)
        {
            string s = "mailto:" + address;
            Process.Start(s);
        }

        public void 发送邮件()
        {
            Process.Start("mailto:feiyangqingyun@163.com");
        }

        public string 获取系统文件夹()
        {
            string s = Environment.GetFolderPath(Environment.SpecialFolder.System);
            return s;
        }

        public void 打开系统文件夹()
        {
            string s = Environment.GetFolderPath(Environment.SpecialFolder.System);
            Process.Start(s);
        }

        public string 获取ProgramFiles目录()
        {
            string s = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            return s;
        }

        public void 打开ProgramFiles目录()
        {
            string s = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            Process.Start(s);
        }

        public string 获取逻辑桌面()
        {
            string s = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            return s;
        }

        public void 打开逻辑桌面()
        {
            string s = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            Process.Start(s);
        }

        public string 获取启动程序组()
        {
            string s = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            return s;
        }

        public void 打开启动程序组()
        {
            string s = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            Process.Start(s);
        }

        public string 获取Cookies文件夹()
        {
            string s = Environment.GetFolderPath(Environment.SpecialFolder.Cookies);
            return s;
        }

        public void 打开Cookies文件夹()
        {
            string s = Environment.GetFolderPath(Environment.SpecialFolder.Cookies);
            Process.Start(s);
        }

        public string 获取Internet历史文件夹()
        {
            string s = Environment.GetFolderPath(Environment.SpecialFolder.History);
            return s;
        }

        public void 打开Internet历史文件夹()
        {
            string s = Environment.GetFolderPath(Environment.SpecialFolder.History);
            Process.Start(s);
        }

        public string 获取我的电脑文件夹()
        {
            string s = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            return s;
        }

        public void 打开我的电脑文件夹()
        {
            string s = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            Process.Start(s);
        }

        public string 获取MyMusic文件夹()
        {
            string s = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            return s;
        }

        public void 打开MyMusic文件夹()
        {
            string s = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            Process.Start(s);
        }

        public string 获取MyPictures文件夹()
        {
            string s = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            return s;
        }

        public void 打开MyPictures文件夹()
        {
            string s = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            Process.Start(s);
        }

        public string 获取StartMenu文件夹()
        {
            string s = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
            return s;
        }

        public void 打开StartMenu文件夹()
        {
            string s = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
            Process.Start(s);
        } 
    }
}
