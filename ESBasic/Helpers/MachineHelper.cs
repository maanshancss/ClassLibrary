using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ESBasic.Helpers
{
    public static class MachineHelper
    {
        private static PerformanceCounter CpuPerformanceCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        private static PerformanceCounter MemPerformanceCounter = new PerformanceCounter("Memory", "% Committed Bytes In Use", "");//Available MBytes
        
        #region GetMacAddress
        /// <summary>
        /// GetMacAddress ��ȡ����mac��ַ
        /// </summary>        
        public static IList<string> GetMacAddress()
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            IList<string> strArr = new List<string>();

            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"])
                {
                    strArr.Add(mo["MacAddress"].ToString().Replace(":", ""));
                }
                mo.Dispose();
            }

            return strArr;
        }
        #endregion

        #region IsCurrentMachine
        public static bool IsCurrentMachine(string macAddress)
        {
            IList<string> addList = MachineHelper.GetMacAddress();
            return addList.Contains(macAddress);
        }
        #endregion

        #region GetPhysicalMemorySize
        /// <summary>
        /// ��ȡ�����ڴ��С
        /// </summary>
        /// <returns></returns>
        public static ulong GetPhysicalMemorySize()
        {
            ulong PhysicalMemorySize = 0, FreePhysicalMemory = 0;
            ManagementClass osClass = new ManagementClass("Win32_OperatingSystem");
            foreach (ManagementObject obj in osClass.GetInstances())
            {
                if (obj["TotalVisibleMemorySize"] != null)
                    PhysicalMemorySize = (ulong)obj["TotalVisibleMemorySize"];            

                if (obj["FreePhysicalMemory"] != null)
                    FreePhysicalMemory = (ulong)obj["FreePhysicalMemory"];
                break;
            }
            osClass.Dispose();
            return PhysicalMemorySize;

        } 
        #endregion

        #region GetCpuInfo
        /// <summary>
        /// ��ȡCPU��Ϣ
        /// </summary>        
        public static List<CpuInfo> GetCpuInfo()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(@"root\CIMV2", "SELECT * FROM Win32_Processor");
            ManagementObjectCollection list = searcher.Get();
            uint count = 0;
            foreach (ManagementObject obj2 in list)
            {
                ++count;
            }
            List<CpuInfo> cpuList = new List<CpuInfo>();
            foreach (ManagementObject obj2 in list)
            {
                cpuList.Add(new CpuInfo(obj2.GetPropertyValue("Name").ToString(), (uint)obj2.GetPropertyValue("CurrentClockSpeed"), (uint)(Environment.ProcessorCount / count)));
            }
            return cpuList;
        }
        #endregion

        #region GetPerformanceUsage
        /// <summary>
        /// GetPerformanceUsage ��ȡ���ܲ�����
        /// </summary>
        /// <param name="cpuUsage">CPU�����ʡ�</param>
        /// <param name="memoryUsage">�����ڴ�������</param>
        public static void GetPerformanceUsage(out float cpuUsage, out float memoryUsage)
        {
            cpuUsage = MachineHelper.CpuPerformanceCounter.NextValue();
            ulong PhysicalMemorySize = 0, FreePhysicalMemory = 0;
            ManagementClass osClass = new ManagementClass("Win32_OperatingSystem");
            foreach (ManagementObject obj in osClass.GetInstances())
            {
                if (obj["TotalVisibleMemorySize"] != null)
                    PhysicalMemorySize = (ulong)obj["TotalVisibleMemorySize"];

                if (obj["FreePhysicalMemory"] != null)
                    FreePhysicalMemory = (ulong)obj["FreePhysicalMemory"];
                break;
            }
            osClass.Dispose();

            if (PhysicalMemorySize == 0)
            {
                memoryUsage = 0;
            }
            else
            {
                memoryUsage = (PhysicalMemorySize - FreePhysicalMemory) * 100 / PhysicalMemorySize;
            }
        }      
        #endregion

        #region GetDiskFreeSpace
        [DllImport("kernel32.dll")]
        private static extern bool GetDiskFreeSpaceEx(string lpDirectoryName, out UInt64 lpFreeBytesAvailable, out UInt64 lpTotalNumberOfBytes, out UInt64 lpTotalNumberOfFreeBytes);
        /// <summary>
        /// ��ȡ���̵Ŀ��ÿռ��С��
        /// </summary>
        /// <param name="diskName">���̵����ơ���"C:\"</param>
        /// <returns>���̵�ʣ��ؼ�</returns>      
        public static ulong GetDiskFreeSpace(string diskName)
        {
            ulong freeBytesAvailable = 0;
            ulong totalNumberOfBytes = 0;
            ulong totalNumberOfFreeBytes = 0;

            GetDiskFreeSpaceEx(diskName, out freeBytesAvailable, out totalNumberOfBytes, out totalNumberOfFreeBytes);
            return freeBytesAvailable;
        }
        #endregion

        #region IsNetworkConnected2
        /// <summary>
        /// ���Լ�ʱ��Ӧ������ͨ�����������Ҫ����System Event Notification֧�֣�ϵͳĬ���Զ������÷���
        /// </summary>       
        public static bool IsNetworkConnected()
        {
            int flags;//������ʽ 
            bool online = IsNetworkAlive(out flags);
            return online;

            #region details
            int NETWORK_ALIVE_LAN = 0;
            int NETWORK_ALIVE_WAN = 2;
            int NETWORK_ALIVE_AOL = 4;
            string outPut = null;
            if (online)//����   
            {
                if ((flags & NETWORK_ALIVE_LAN) == NETWORK_ALIVE_LAN)
                {
                    outPut = "���ߣ�NETWORK_ALIVE_LAN\n";
                }
                if ((flags & NETWORK_ALIVE_WAN) == NETWORK_ALIVE_WAN)
                {
                    outPut = "���ߣ�NETWORK_ALIVE_WAN\n";
                }
                if ((flags & NETWORK_ALIVE_AOL) == NETWORK_ALIVE_AOL)
                {
                    outPut = "���ߣ�NETWORK_ALIVE_AOL\n";
                }
            }
            else
            {
                outPut = "������\n";
            }
            #endregion
        }

        /// <summary>
        /// ������״�����ܼ�ʱ��Ӧ
        /// </summary>        
        public static bool IsNetworkConnected2()
        {
            int flags;//������ʽ 
            bool online = InternetGetConnectedState(out flags, 0);
            return online;

            #region Details
            int INTERNET_CONNECTION_MODEM = 1;
            int INTERNET_CONNECTION_LAN = 2;
            int INTERNET_CONNECTION_PROXY = 4;
            int INTERNET_CONNECTION_MODEM_BUSY = 8;
            string outPut = null;
            if (online)//����   
            {
                if ((flags & INTERNET_CONNECTION_MODEM) == INTERNET_CONNECTION_MODEM)
                {
                    outPut = "���ߣ���������\n";
                }
                if ((flags & INTERNET_CONNECTION_LAN) == INTERNET_CONNECTION_LAN)
                {
                    outPut = "���ߣ�ͨ��������\n";
                }
                if ((flags & INTERNET_CONNECTION_PROXY) == INTERNET_CONNECTION_PROXY)
                {
                    outPut = "���ߣ�����\n";
                }
                if ((flags & INTERNET_CONNECTION_MODEM_BUSY) == INTERNET_CONNECTION_MODEM_BUSY)
                {
                    outPut = "MODEM��������INTERNET����ռ��\n";
                }
            }
            else
            {
                outPut = "������\n";
            }
            #endregion
        }

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int connectionDescription, int reservedValue);
        [DllImport("sensapi.dll")]
        private extern static bool IsNetworkAlive(out int connectionDescription); 
        #endregion     
    }
        
    public struct CpuInfo
    {
        public CpuInfo(string name, uint speed, uint coreCount)
        {
            this.Name = name;
            this.ClockSpeed = speed;
            this.CoreCount = coreCount;
        }

        /// <summary>
        /// CPU���ơ�
        /// </summary>
        public string Name;
        /// <summary>
        /// ��Ƶ��
        /// </summary>
        public uint ClockSpeed;
        /// <summary>
        /// ������Ŀ
        /// </summary>
        public uint CoreCount;
    } 
}