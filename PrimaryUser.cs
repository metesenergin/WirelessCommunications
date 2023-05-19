using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumSharing
{
    public  class PrimaryUser
    {
        public  string PrimaryUserStatus { get; set; }
        public  string PrimaryUserAuctionCount { get; set; }
        public  string PrimaryUserID { get; set; }
        public  string PrimaryUserWallet { get; set; }
        public  string PrimaryUserReputationScore { get; set; }
        public  string PrimaryUserSuccessfulAuctionCount { get; set; }
        public  string ResponseIsAppropriate { get; set; }
        public  string AppropriateResponseCount { get; set; }
        public  string TotalRequestCount { get; set; }
        public  string AvailableSpectrumKHz { get; set; }
        public  string TotalSpectrumKHz { get; set; }
        public  string CreateUser(string id)
        {
            string result="";

            // Update database for user ID return result

            return result;
        }

        public  string UpdateWallet(string coinAmount)
        {
            string result="";

            // Update database for user ID return result

            return result;
        }

    }

   
}
