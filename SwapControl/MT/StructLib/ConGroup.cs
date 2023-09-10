using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


using UINT = System.UInt32;
//const int MAX_SEC_GROUPS = 32;
//const int MAX_SEC_GROPS_MARGIN = 128;

namespace SwapControl.MT.StructLib
{

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct ConGroup
    {

        //--- common settings
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string group;                   // group name
        public int enable;                      // enable group
        public int timeout;                     // trade confirmation timeout (seconds)
        public int otp_mode;                    // one-time password mode
                                         //--- statements
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string company;                // company name
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string signature;              // statements signature
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string support_page;           // company support page
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string smtp_server;             // statements SMTP server
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string smtp_login;              // statements SMTP login
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string smtp_password;           // statements SMTP password
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string support_email;           // support email
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string templates;               // path to directory with custom templates
        public int copies;                      // copy statements on support email
        public int reports;                     // enable statements
                                                //--- default settings
        public int default_leverage;            // default leverage (user don't specify leverage himself)
        public double default_deposit;             // default deposit  (user don't specify balance  himself)
                                                   //--- securities
        public int maxsecurities;               // maximum simultaneous securities
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        ConGroupSec[] secgroups;   // security group settings
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
        public ConGroupMargin[] secmargins; // special securities settings
        public int secmargins_total;            // count of special securities settings
                                         //--- margin & interest
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string currency;                // deposit currency
        public double credit;                      // virtual credit
        public int margin_call;                 // margin call level (percents)
        public int margin_mode;                 // margin mode-MARGIN_DONT_USE,MARGIN_USE_ALL,MARGIN_USE_PROFIT,MARGIN_USE_LOSS
        public int margin_stopout;              // stop out level
        public double interestrate;                // annual interest rate (percents)
        public int use_swap;                    // use rollovers & interestrate
                                                //--- rights
        public int news;                        // news mode
        public int rights;                      // rights bit mask-ALLOW_FLAG_EMAIL
        public int check_ie_prices;             // check IE prices on requests
        public int maxpositions;                // maximum orders and open positions
        public int close_reopen;                // partial close mode (if !=0 original position will be fully closed and remain position will be fully reopened)
        public int hedge_prohibited;            // hedge prohibition flag
        public int close_fifo;                  // fifo rule 
        public int hedge_largeleg;              // use large leg margin for hedged positions
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public int[] unused_rights;            // reserved

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string securities_hash;         // internal data
                                                 //---
        public int margin_type;                 // margin controlling type { MARGIN_TYPE_PERCENT,  MARGIN_TYPE_CURRENCY }
                                                //--- archives
        public int archive_period;              // inactivity period after which account moves to archive base (in days)
        public int archive_max_balance;         // maxumum balance of accounts to move in archive base
                                                //---
        public int stopout_skip_hedged;         // skip fully hedged accounts when checking for stopout
        public int archive_pending_period;      // pendings clean period
                                         //--- allowed news languages
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public UINT[] news_languages;           // LANGID array
        public UINT news_languages_total;        // news languages total
        //--- reserved
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
        public int[] reserved;
    }
}
