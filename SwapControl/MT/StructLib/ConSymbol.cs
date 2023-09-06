using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using COLORREF = System.UInt32;
using __time32_t = System.Int32;

namespace SwapControl.MT.StructLib
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct ConSymbol
    {
        //--- common settings
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string symbol;                  // name
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        string description;             // description
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        string source;                  // synonym
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        string currency;                // currency
        int type;                        // security group (see ConSymbolGroup)
        int digits;                      // security precision
        public int trade;                       // trade mode
                                         //--- external settings
        COLORREF background_color;            // background color
        int count;                       // symbols index
        int count_original;              // symbols index in market watch
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        int[] external_unused;
        //--- sessions
        int realtime;                    // allow real time quotes
        __time32_t starting;                    // trades starting date (UNIX time)
        __time32_t expiration;                  // trades end date      (UNIX time)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        ConSessions[] sessions;                 // quote & trade sessions
                                                 //--- profits
        int profit_mode;                 // profit calculation mode
        int profit_reserved;             // reserved
                                         //--- filtration
        int filter;                      // filter value
        int filter_counter;              // filtration parameter
        double filter_limit;                // max. permissible deviation from last quote (percents)
        int filter_smoothing;            // smoothing
        float filter_reserved;             // reserved
        int logging;                     // enable to log quotes
                                         //--- spread & swaps
        public int spread;                      // spread
        public int spread_balance;              // spread balance
        public int exemode;                     // execution mode
        public int swap_enable;                 // enable swaps
        public int swap_type;                   // swap type
        public double swap_long, swap_short;        // swaps values for long & short postions
        int swap_rollover3days;          // triple rollover day-0-Monday,1-Tuesday...4-Friday
        double contract_size;               // contract size
        double tick_value;                  // one tick value
        double tick_size;                   // one tick size
        int stops_level;                 // stops deviation value
        int gtc_pendings;                // GTC mode { ORDERS_DAILY, ORDERS_GTC, ORDERS_DAILY_NO_STOPS }
                                         //--- margin calculation
        int margin_mode;                 // margin calculation mode
        double margin_initial;              // initial margin
        double margin_maintenance;          // margin maintenance
        double margin_hedged;               // hedged margin
        double margin_divider;              // margin divider
                                            //--- calclulated variables (internal data)
        double point;                       // point size-(1/(10^digits)
        double multiply;                    // multiply 10^digits
        double bid_tickvalue;               // tickvalue for bid
        double ask_tickvalue;               // tickvalue for ask
                                            //---
        int long_only;                   // allow only BUY positions
        int instant_max_volume;          // max. volume for Instant Execution
                                         //---
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        string margin_currency;         // currency of margin requirments
        int freeze_level;                // modification freeze level
        int margin_hedged_strong;        // strong hedged margin mode
        __time32_t value_date;                  // value date
        int quotes_delay;                // quotes delay after session start
        int swap_openprice;               // use open price at swaps calculation in SWAP_BY_INTEREST mode
        int swap_variation_margin;       // charge variation margin on rollover
                                         //---
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
        int[] unused;             	  // reserved
    }
}
