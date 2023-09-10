using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SwapControl.MT.StructLib
{
    
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct ConGroupMargin
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string symbol;            // security
        public double swap_long, swap_short;  // swap size for long and short positions
        double margin_divider;        // margin divider
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        int[] reserved;
    }
}
