using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SwapControl.MT.StructLib
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct ConSession
    {
        short open_hour, open_min;          // session open  time: hour & minute
        short close_hour, close_min;        // session close time: hour & minute
        int open, close;                  // internal data
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        short[] align;                    // internal data
    };
}
