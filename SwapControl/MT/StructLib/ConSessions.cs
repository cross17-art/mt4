using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SwapControl.MT.StructLib
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct ConSessions
    {
        //---
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        ConSession[] quote;                    // quote sessions
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        ConSession[] trade;                    // trade sessions
                                                //---
        int quote_overnight;             // internal data
        int trade_overnight;             // internal data
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        int[] reserved;                 // reserved
    };
}
