using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace ESBasic.Widget.Internals
{
    [ComVisible(false), StructLayout(LayoutKind.Sequential)]
	public sealed class STATDATA 
	{

		[MarshalAs(UnmanagedType.U4)]
		public   int advf;
		[MarshalAs(UnmanagedType.U4)]
		public   int dwConnection;

	}
}
