using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumSharing
{
    public static class ReputationScoring
    {
        public static string CalculateReputationScorePrimaryUser(PrimaryUser userInfo)
        {

            string updatedScore;

            int totalSpectrum=int.Parse(userInfo.TotalSpectrumKHz);
            int availableSpectrum=int.Parse(userInfo.AvailableSpectrumKHz);

            int totalAuction = int.Parse(userInfo.PrimaryUserAuctionCount);
            int successfulAuction=int.Parse(userInfo.PrimaryUserSuccessfulAuctionCount);

            int numberofConnections = int.Parse(userInfo.TotalRequestCount);
            int successfullConecctions = int.Parse(userInfo.TotalRequestCount);

            int numberofResponses=int.Parse(userInfo.TotalRequestCount);
            int successfulResponses=int.Parse(userInfo.AppropriateResponseCount);

            float score = (float)(((float)(totalSpectrum/availableSpectrum)) * 0.35 + ((float)(totalAuction/successfulAuction)) * 0.25 + ((float)(numberofConnections/successfullConecctions)) * 0.2 + ((float)(numberofResponses / successfulResponses)) * 0.2);

            updatedScore = score.ToString("0.00", CultureInfo.InvariantCulture);

            return updatedScore;

        }
        public static string CalculateReputationScoreSecondaryUser(SecondaryUser userInfo, PrimaryUser priUserInfo,Auction auction)
        {

            string updatedScore;

            int totalBids = int.Parse(userInfo.AuctionCountUser);
            int successfulBids = int.Parse(userInfo.SuccessfulAuctionCountUser);

            int totalHour = 1;
            int availableSpectrumHours = 1;

            int spectrumUsage = int.Parse(auction.AuctionChannelBandwidth);
            int spectrumTotal = int.Parse(priUserInfo.AvailableSpectrumKHz);

            float score = (float)(((float)(totalBids / (successfulBids+1))) * 0.35 + ((float)(totalHour / (availableSpectrumHours+1))) * 0.35 + ((float)(spectrumUsage / (spectrumTotal+1))) * 0.3 );

            updatedScore = score.ToString("0.00", CultureInfo.InvariantCulture);

            return updatedScore;

        }
    }
}
