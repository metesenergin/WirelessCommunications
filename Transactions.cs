using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumSharing
{
    public static class Transactions
    {
        public static string TimeStamp { get; set; }

        public static string LogType = "1";
        public static string TransactionID { get; set; }
        public static string AllocatedChannel { get; set; }
        public static string WinnerSecondaryUserID { get; set; }
        public static string WinnerSecondaryUserWallet { get; set; }
        public static string WinnerSecondaryUserReputationScore { get; set; }
        public static string PrimaryUserID { get; set; }
        public static string PrimaryUserWallet { get; set; }
        public static string PrimaryUserReputationScore { get; set; }

    }
}
