using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumSharing
{
    public class Transactions
    {
        public string TimeStamp { get; set; }

        public string LogType = "1";
        public string TransactionID { get; set; }
        public string AllocatedChannel { get; set; }
        public string WinnerSecondaryUserID { get; set; }
        public string WinnerSecondaryUserWallet { get; set; }
        public string WinnerSecondaryUserReputationScore { get; set; }
        public string PrimaryUserWallet { get; set; }
       
       
    }
}
