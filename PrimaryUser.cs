using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumSharing
{
    public static class PrimaryUser
    {
        public static string PrimaryUserStatus { get; set; }
        public static string PrimaryUserAuctionCount { get; set; }
        public static string PrimaryUserID { get; set; }
        public static string PrimaryUserWallet { get; set; }
        public static string PrimaryUserReputationScore { get; set; }
        public static string PrimaryUserSuccessfulAuctionCount { get; set; }
        public static string ResponseIsAppropriate { get; set; }
        public static string AppropriateResponseCount { get; set; }
        public static string TotalRequestCount { get; set; }
        public static string AvailableSpectrumKHz { get; set; }
        public static string TotalSpectrumKHz { get; set; }
        public static string CreateUser(string id)
        {
            string result="";

            // Update database for user ID return result

            return result;
        }

        public static string UpdateWallet(string coinAmount)
        {
            string result="";

            // Update database for user ID return result

            return result;
        }

    }

   
}
