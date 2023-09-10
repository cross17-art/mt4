using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SwapControl.MT.StructLib
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct ConGroupSec
    {
        int show, trade;            // enable show and trade for this group of securites
        int execution;             // dealing mode-EXECUTION_MANUAL,EXECUTION_AUTO,EXECUTION_ACTIVITY
                                   //--- comission settings
        double comm_base;             // standart commission
        int comm_type;             // commission type-COMM_TYPE_MONEY,COMM_TYPE_PIPS,COMM_TYPE_PERCENT
        int comm_lots;             // commission lots mode-COMMISSION_PER_LOT,COMMISSION_PER_DEAL
        double comm_agent;            // agent commission
        int comm_agent_type;       // agent commission mode-COMM_TYPE_MONEY, COMM_TYPE_PIPS
                                   //---
        int spread_diff;           // spread difference in compare with default security spread
                                   //---
        int lot_min, lot_max;       // allowed minimal and maximal lot values
        int lot_step;              // allowed step value (10 lot-1000, 1 lot-100, 0.1 lot-10)
        int ie_deviation;          // maximum price deviation in Instant Execution mode
        int confirmation;          // use confirmation in Request mode
        int trade_rights;          // clients trade rights-bit mask see TRADE_DENY_NONE,TRADE_DENY_CLOSEBY,TRADE_DENY_MUCLOSEBY
        int ie_quick_mode;         // do not resend request to the dealer when client uses deviation
        int autocloseout_mode;     // auto close-out method { CLOSE_OUT_NONE, CLOSE_OUT_HIHI, CLOSE_OUT_LOLO, CLOSE_OUT_HILO, CLOSE_OUT_LOHI, CLOSE_OUT_LOHI, CLOSE_OUT_FIFO, CLOSE_OUT_LIFO, CLOSE_OUT_INTRDAY_FIFO }
        double comm_tax;              // commission taxes
        int comm_agent_lots;       // agent commission per lot/per deal { COMMISSION_PER_LOT,COMMISSION_PER_DEAL }
        int freemargin_mode;       // "soft" margin check
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        int[] reserved;           // reserved
    }
}
