﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace ESBasic.Widget.Internals
{
    [StructLayout(LayoutKind.Sequential), ComVisible(false)]
    public struct STGMEDIUM
    {
        //[MarshalAs(UnmanagedType.I4)]
        public int tymed;
        public IntPtr unionmember;
        public IntPtr pUnkForRelease;
    }
}
