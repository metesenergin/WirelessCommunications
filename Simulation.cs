using System.Data;
using System.Globalization;

namespace SpectrumSharing
{
    public partial class Simulation : Form
    {
        public Simulation()
        {
            InitializeComponent();
        }

        private void Simulation_Load(object sender, EventArgs e)
        {

            //Auction
            label1.Text = "TimeStamp";
            label2.Text = "LogType";
            label3.Text = "AuctionNo";
            label4.Text = "AuctionChannel";
            label5.Text = "AuctionChannelBandwidth";
            label6.Text = "AuctionPrice";

            //Transaction
            label7.Text = "TransactionID";
            label8.Text = "AllocatedChannel";
            label9.Text = "WinnerSecondaryUserID";
            label10.Text = "WinnerSecondaryUserWallet";
            label11.Text = "WinnerSecondaryUserReputationScore";
            label12.Text = "PrimaryUserID";
            label13.Text = "PrimaryUserWallet";
            label14.Text = "PrimaryUserReputationScore";

            //PrimaryUser
            label15.Text = "PrimaryUserStatus";
            label16.Text = "PrimaryUserAuctionCount";

            label20.Text = "PrimaryUserSuccessfulAuctionCount";
            label21.Text = "ResponseIsAppropriate";
            label22.Text = "AppropriateResponseCount";
            label23.Text = "TotalRequestCount";
            label24.Text = "AvailableSpectrumKHz";
            label25.Text = "TotalSpectrumKHz";

            //Secondary User
            label26.Text = "IsBiddingUser";
            label27.Text = "IsInPoolUser";
            label28.Text = "ScoreUser";
            label29.Text = "StatusUser";
            label30.Text = "AuctionCountUser";
            label31.Text = "SuccessfulAuctionCountUser";
            label32.Text = "WalletUser";
        }

        private string GetValueByColumnName(DataTable dataTable, string columnName)
        {
            DataRow row = dataTable.Rows[0];

            // Retrieve the cell value using the column name
            object cellValue = row[columnName];

            // If the column type is known, you can cast the value to the appropriate type
            // For example, if the column is of type string
            string stringValue = row[columnName].ToString();

            return stringValue;
        }
        private string GetFirstAvailableChannel(DataTable dataTable)
        {
            for (int i = 1; i <= 57; i++)
            {
                string columnName = "Channel" + i.ToString();
                string value = GetValueByColumnName(dataTable, columnName);
                if (value == "0")
                    return i.ToString();
            }
            return "0";
        }
        private string GetBandWidthOfChannel(string auctionChannel)
        {
            int channelNo = int.Parse(auctionChannel);

            if (channelNo < 41)
                return "5";
            else
                return "20";
        }

        private string GetPriceOfAuction(string bandWidth)
        {
            int channelNo = int.Parse(bandWidth);

            if (channelNo == 5)
                return "0.4";
            else
                return "1.0";
        }

        private string SetNewTimeStamp(string timeStampOld, string type)
        {

            DateTime timestamp = DateTime.ParseExact(timeStampOld, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            DateTime incrementedTimestamp = timestamp.AddMinutes(1);
            if (type == "t")
                incrementedTimestamp = timestamp.AddMinutes(2);

            string incrementedTimestampString = incrementedTimestamp.ToString("yyyy-MM-ddTHH:mm:ss");


            return incrementedTimestampString;
        }

        private void LoadAuctionValuesToUI(Auction auction)
        {

            textBoxTimeStamp.Text = auction.TimeStamp;
            textBoxLogType.Text = auction.LogType;
            textBoxAuctionNo.Text = auction.AuctionNo;
            textBoxAuctionChannel.Text = auction.AuctionChannel;
            textBoxAuctionChannelBandwidth.Text = auction.AuctionChannelBandwidth;
            textBoxAuctionPrice.Text = auction.AuctionPrice;

        }
        private string GetAvailableSpectrum(DataTable dataTable)
        {
            int availableSpectrum = 0;
            for (int i = 1; i <= 57; i++)
            {
                string columnName = "Channel" + i.ToString();
                string value = GetValueByColumnName(dataTable, columnName);
                if (value == "0")
                    if (i < 41)
                        availableSpectrum += 5;
                    else
                        availableSpectrum += 20;
            }
            return availableSpectrum.ToString();

        }
        private string GetSpectrumInfo(DataTable dataTable, int channelNo)
        {
            string columnName = "Channel" + channelNo.ToString();
            string value = GetValueByColumnName(dataTable, columnName);

            return value;

        }

        private void GetInfoAndGenerateAnEvent(bool useReputation)
        {

            DataTable table = Queries.SelectLatestRow(useReputation);
            dataGridView1.DataSource = table;
            string[,] parametersAndValues = new string[548, 2];
            int i = 0;

            //Load Latest Values
            foreach (DataColumn column in table.Columns)
            {
                parametersAndValues[i, 0] = column.ColumnName;
                parametersAndValues[i, 1] = table.Rows[0][i].ToString();
                i++;
            }
            //Auction
            Auction auction = new Auction();
            auction.TimeStamp = SetNewTimeStamp(GetValueByColumnName(table, "TimeStamp"), "a");
            auction.AuctionNo = (int.Parse(GetValueByColumnName(table, "AuctionNo")) + 1).ToString();
            auction.AuctionChannel = GetFirstAvailableChannel(table);
            auction.AuctionChannelBandwidth = GetBandWidthOfChannel(auction.AuctionChannel);
            auction.AuctionPrice = GetPriceOfAuction(auction.AuctionChannelBandwidth);
            LoadAuctionValuesToUI(auction);

            //PU
            PrimaryUser primaryUser = new PrimaryUser();
            primaryUser.TotalSpectrumKHz = "540";
            primaryUser.AvailableSpectrumKHz = GetAvailableSpectrum(table);
            primaryUser.PrimaryUserAuctionCount = (int.Parse(GetValueByColumnName(table, "PrimaryUserAuctionCount")) + 1).ToString();
            primaryUser.PrimaryUserID = "1111";
            primaryUser.PrimaryUserStatus = "1";
            primaryUser.PrimaryUserWallet = GetValueByColumnName(table, "PrimaryUserWallet");
            primaryUser.PrimaryUserReputationScore = GetValueByColumnName(table, "PrimaryUserReputationScore");

            //Fill 57 Channels Info
            SpectrumResources[] channels = new SpectrumResources[57];
            int channelNo = 1;
            for (int j = 0; j < channels.Length; j++)
            {
                channels[j] = new SpectrumResources(); // Create an instance of SpectrumResources

                channels[j].AllocationStatus = GetSpectrumInfo(table, channelNo);
                channels[j].Bandwidth = GetBandWidthOfChannel(channelNo.ToString());
                channelNo++;
            }


            //Generate Secondary Users
            SecondaryUser[] secondaryUsers = new SecondaryUser[67];
            string[] ListOfSU = new string[67];
            for (int j = 0; j < secondaryUsers.Length; j++)
            {
                secondaryUsers[j] = new SecondaryUser(); // Create an instance of SecondaryUser

                secondaryUsers[j].ID = (j + 1).ToString();
                ListOfSU[j] = secondaryUsers[j].ID;
            }

            var random = new Random(int.Parse(auction.AuctionNo) + 1);
            int statusOKCount = random.Next(50, 67);
            int bidRequestCount = random.Next(40, statusOKCount);
            int successorCount = random.Next(0, 10);
            int inappropriateResponseCount = random.Next(0, 2);


            //Assign Random Status
            string[] randomStatusList = BlockChain.RandomStatus(ListOfSU, int.Parse(auction.AuctionNo));
            string[] statusOKList = randomStatusList.TakeLast(statusOKCount).ToArray();
            string[] randomIsBiddingList = BlockChain.RandomIsBidding(statusOKList, int.Parse(auction.AuctionNo) + 1);
            string[] biddingPoolAll = randomIsBiddingList.TakeLast(bidRequestCount).ToArray();
            string[] biddingPoolOnlyAppropriateResponse = biddingPoolAll.TakeLast(bidRequestCount - inappropriateResponseCount).ToArray();
            string[] statusNOKList = randomStatusList.Take(67 - statusOKCount).ToArray();

            for (int j = 0; j < 67; j++)
            {
                secondaryUsers[j].StatusUser = "0";
                secondaryUsers[j].IsBiddingUser = "0";
                secondaryUsers[j].IsInPoolUser = "0";
                secondaryUsers[j].SuccessfulAuctionCountUser = GetValueByColumnName(table, "SuccessfulAuctionCountUser" + (j + 1).ToString());
                secondaryUsers[j].ScoreUser = GetValueByColumnName(table, "ScoreUser" + (j + 1).ToString());
                secondaryUsers[j].WalletUser = GetValueByColumnName(table, "WalletUser" + (j + 1).ToString());
                secondaryUsers[j].AuctionCountUser = GetValueByColumnName(table, "AuctionCountUser" + (j + 1).ToString());
            }
            for (int j = 0; j < statusNOKList.Length; j++)
            {
                for (int k = 0; k < 57; k++)
                    if (channels[k].AllocationStatus == statusNOKList[j])
                    {
                        channels[k].AllocationStatus = "0";
                    }
            }

            for (int j = 0; j < statusOKCount; j++)
            {
                secondaryUsers[int.Parse(statusOKList[j]) - 1].StatusUser = "1";

            }

            for (int j = 0; j < bidRequestCount; j++)
            {
                secondaryUsers[int.Parse(biddingPoolAll[j]) - 1].IsBiddingUser = "1";
            }

            for (int j = 0; j < biddingPoolOnlyAppropriateResponse.Length; j++)
                secondaryUsers[int.Parse(biddingPoolOnlyAppropriateResponse[j]) - 1].IsInPoolUser = "1";

            List<string> list = new List<string>();
            for (int j = 0; j < 67; j++)
            {
                if (secondaryUsers[j].IsInPoolUser == "1")
                {
                    list.Add((j + 1).ToString());
                    secondaryUsers[j].AuctionCountUser = (int.Parse(GetValueByColumnName(table, "AuctionCountUser" + (j + 1).ToString())) + 1).ToString();
                }

            }

            string[] biddingPoolFinal = list.ToArray();

            string[] winnerSU = BlockChain.Mine(biddingPoolFinal, int.Parse(auction.AuctionNo) + 2).Take(successorCount).ToArray();

            if (successorCount > 0)
                primaryUser.PrimaryUserSuccessfulAuctionCount = (int.Parse(GetValueByColumnName(table, "PrimaryUserSuccessfulAuctionCount")) + 1).ToString();
            else if (successorCount == 0)
                primaryUser.PrimaryUserSuccessfulAuctionCount = GetValueByColumnName(table, "PrimaryUserSuccessfulAuctionCount");

            primaryUser.AppropriateResponseCount = biddingPoolOnlyAppropriateResponse.Length.ToString();
            primaryUser.TotalRequestCount = bidRequestCount.ToString();

            if (inappropriateResponseCount == 0)
                primaryUser.ResponseIsAppropriate = "1";
            else
                primaryUser.ResponseIsAppropriate = "0";

            if (successorCount > 0)
            {
                //After Auction
                //decide winner update scores 
                //Insert Auction into Event
                string InsertCommand;
                if (!useReputation)
                    InsertCommand = Queries.insertQuery;
                else
                    InsertCommand = Queries.insertQueryUseReputation;

                InsertCommand = InsertCommand.Replace("<TimeStamp>", "'" + auction.TimeStamp + "'");
                InsertCommand = InsertCommand.Replace("<LogType>", auction.LogType);
                InsertCommand = InsertCommand.Replace("<TransactionID>", "0");
                InsertCommand = InsertCommand.Replace("<AllocatedChannel>", "0");
                InsertCommand = InsertCommand.Replace("<WinnerSecondaryUserID>", "0");
                InsertCommand = InsertCommand.Replace("<WinnerSecondaryUserWallet>", "0");
                InsertCommand = InsertCommand.Replace("<WinnerSecondaryUserReputationScore>", "0");
                InsertCommand = InsertCommand.Replace("<PrimaryUserID>", primaryUser.PrimaryUserID);
                InsertCommand = InsertCommand.Replace("<PrimaryUserWallet>", primaryUser.PrimaryUserWallet);
                InsertCommand = InsertCommand.Replace("<PrimaryUserReputationScore>", primaryUser.PrimaryUserReputationScore);
                InsertCommand = InsertCommand.Replace("<PrimaryUserStatus>", primaryUser.PrimaryUserStatus);
                InsertCommand = InsertCommand.Replace("<AuctionNo>", auction.AuctionNo);
                InsertCommand = InsertCommand.Replace("<AuctionChannel>", auction.AuctionChannel);
                InsertCommand = InsertCommand.Replace("<AuctionChannelBandwidth>", auction.AuctionChannelBandwidth);
                InsertCommand = InsertCommand.Replace("<PrimaryUserAuctionCount>", primaryUser.PrimaryUserAuctionCount);
                InsertCommand = InsertCommand.Replace("<PrimaryUserSuccessfulAuctionCount>", primaryUser.PrimaryUserSuccessfulAuctionCount);
                InsertCommand = InsertCommand.Replace("<ResponseIsAppropriate>", primaryUser.ResponseIsAppropriate);
                InsertCommand = InsertCommand.Replace("<AppropriateResponseCount>", primaryUser.AppropriateResponseCount);
                InsertCommand = InsertCommand.Replace("<TotalRequestCount>", primaryUser.TotalRequestCount);
                InsertCommand = InsertCommand.Replace("<AvailableSpectrumKHz>", primaryUser.AvailableSpectrumKHz);
                InsertCommand = InsertCommand.Replace("<TotalSpectrumKHz>", primaryUser.TotalSpectrumKHz);
                InsertCommand = InsertCommand.Replace("<AuctionPrice>", auction.AuctionPrice);

                for (int j = 1; j < 68; j++)
                {
                    InsertCommand = InsertCommand.Replace("<AuctionCountUser" + j.ToString() + ">", secondaryUsers[j - 1].AuctionCountUser);
                    InsertCommand = InsertCommand.Replace("<IsBiddingUser" + j.ToString() + ">", secondaryUsers[j - 1].IsBiddingUser);
                    InsertCommand = InsertCommand.Replace("<IsInPoolUser" + j.ToString() + ">", secondaryUsers[j - 1].IsInPoolUser);
                    InsertCommand = InsertCommand.Replace("<ScoreUser" + j.ToString() + ">", secondaryUsers[j - 1].ScoreUser);
                    InsertCommand = InsertCommand.Replace("<StatusUser" + j.ToString() + ">", secondaryUsers[j - 1].StatusUser);
                    InsertCommand = InsertCommand.Replace("<SuccessfulAuctionCountUser" + j.ToString() + ">", secondaryUsers[j - 1].SuccessfulAuctionCountUser);
                    InsertCommand = InsertCommand.Replace("<WalletUser" + j.ToString() + ">", secondaryUsers[j - 1].WalletUser);
                    if (j < 58)
                        InsertCommand = InsertCommand.Replace("<Channel" + j.ToString() + ">", channels[j - 1].AllocationStatus);
                }
                File.AppendAllText(@"C:\Users\Administrator\Desktop\TEL505E-Progress2\a.txt", InsertCommand);

                Queries.InsertEvent(InsertCommand);
                primaryUser.PrimaryUserReputationScore = ReputationScoring.CalculateReputationScorePrimaryUser(primaryUser);
                foreach (SecondaryUser secondaryUser in secondaryUsers)
                    secondaryUser.ScoreUser = ReputationScoring.CalculateReputationScoreSecondaryUser(secondaryUser,primaryUser,auction);

                for (int j = 0; j < successorCount; j++)
                {
                    float WalletOfSecondaryUser = float.Parse(secondaryUsers[int.Parse(winnerSU[j])].WalletUser);
                    float WalletOfPrimaryUser = float.Parse(primaryUser.PrimaryUserWallet);
                    float AuctionPrice = float.Parse(auction.AuctionPrice);
                    float contributionPercentage = (float)(bidRequestCount - inappropriateResponseCount) / bidRequestCount;
                    float winnersCoefficient = (float)(((bidRequestCount - successorCount) / bidRequestCount) + 1);
                    float AddtoSecondaryUsersWallet = AuctionPrice * winnersCoefficient;
                    float AddtoPrimaryUsersWallet = AuctionPrice * contributionPercentage;
                    float withDrawFromSecondaryUser = AddtoPrimaryUsersWallet;

                    secondaryUsers[int.Parse(winnerSU[j])].WalletUser = (WalletOfSecondaryUser + AddtoSecondaryUsersWallet).ToString("0.00", CultureInfo.InvariantCulture);
                    primaryUser.PrimaryUserWallet = (WalletOfPrimaryUser + AddtoPrimaryUsersWallet).ToString("0.00", CultureInfo.InvariantCulture);
                    WalletOfSecondaryUser = float.Parse(secondaryUsers[int.Parse(winnerSU[j])].WalletUser);
                    if (WalletOfSecondaryUser >= withDrawFromSecondaryUser)
                    {
                        //create a transaction log with log type 1
                        secondaryUsers[int.Parse(winnerSU[j])].SuccessfulAuctionCountUser = (int.Parse(secondaryUsers[int.Parse(winnerSU[j])].SuccessfulAuctionCountUser) + 1).ToString();
                        Transactions transaction = new Transactions();
                        transaction.TransactionID = (int.Parse(GetValueByColumnName(table, "TransactionID")) + 1).ToString();
                        transaction.TimeStamp = SetNewTimeStamp(GetValueByColumnName(table, "TimeStamp"), "t");
                        transaction.WinnerSecondaryUserID = winnerSU[j];
                        transaction.WinnerSecondaryUserWallet = secondaryUsers[int.Parse(winnerSU[j])].WalletUser;
                        transaction.WinnerSecondaryUserReputationScore = secondaryUsers[int.Parse(winnerSU[j])].ScoreUser;
                        transaction.PrimaryUserWallet = primaryUser.PrimaryUserWallet;
                        transaction.AllocatedChannel = auction.AuctionChannel;
                        channels[int.Parse(auction.AuctionChannel)].AllocationStatus = winnerSU[j];

                        string InsertTransactionCommand;
                        if (!useReputation)
                            InsertTransactionCommand = Queries.insertQuery;
                        else
                            InsertTransactionCommand = Queries.insertQueryUseReputation;


                        InsertTransactionCommand = InsertTransactionCommand.Replace("<TimeStamp>", "'" + transaction.TimeStamp + "'");
                        InsertTransactionCommand = InsertTransactionCommand.Replace("<LogType>", transaction.LogType);
                        InsertTransactionCommand = InsertTransactionCommand.Replace("<TransactionID>", transaction.TransactionID);
                        InsertTransactionCommand = InsertTransactionCommand.Replace("<AllocatedChannel>", transaction.AllocatedChannel);
                        InsertTransactionCommand = InsertTransactionCommand.Replace("<WinnerSecondaryUserID>", transaction.WinnerSecondaryUserID);
                        InsertTransactionCommand = InsertTransactionCommand.Replace("<WinnerSecondaryUserWallet>", transaction.WinnerSecondaryUserWallet);
                        InsertTransactionCommand = InsertTransactionCommand.Replace("<WinnerSecondaryUserReputationScore>", transaction.WinnerSecondaryUserReputationScore);
                        InsertTransactionCommand = InsertTransactionCommand.Replace("<PrimaryUserID>", primaryUser.PrimaryUserID);
                        InsertTransactionCommand = InsertTransactionCommand.Replace("<PrimaryUserWallet>", transaction.PrimaryUserWallet);
                        InsertTransactionCommand = InsertTransactionCommand.Replace("<PrimaryUserReputationScore>", primaryUser.PrimaryUserReputationScore);
                        InsertTransactionCommand = InsertTransactionCommand.Replace("<PrimaryUserStatus>", primaryUser.PrimaryUserStatus);
                        InsertTransactionCommand = InsertTransactionCommand.Replace("<AuctionNo>", auction.AuctionNo);
                        InsertTransactionCommand = InsertTransactionCommand.Replace("<AuctionChannel>", auction.AuctionChannel);
                        InsertTransactionCommand = InsertTransactionCommand.Replace("<AuctionChannelBandwidth>", auction.AuctionChannelBandwidth);
                        InsertTransactionCommand = InsertTransactionCommand.Replace("<PrimaryUserAuctionCount>", primaryUser.PrimaryUserAuctionCount);
                        InsertTransactionCommand = InsertTransactionCommand.Replace("<PrimaryUserSuccessfulAuctionCount>", primaryUser.PrimaryUserSuccessfulAuctionCount);
                        InsertTransactionCommand = InsertTransactionCommand.Replace("<ResponseIsAppropriate>", primaryUser.ResponseIsAppropriate);
                        InsertTransactionCommand = InsertTransactionCommand.Replace("<AppropriateResponseCount>", primaryUser.AppropriateResponseCount);
                        InsertTransactionCommand = InsertTransactionCommand.Replace("<TotalRequestCount>", primaryUser.TotalRequestCount);
                        InsertTransactionCommand = InsertTransactionCommand.Replace("<AvailableSpectrumKHz>", primaryUser.AvailableSpectrumKHz);
                        InsertTransactionCommand = InsertTransactionCommand.Replace("<TotalSpectrumKHz>", primaryUser.TotalSpectrumKHz);
                        InsertTransactionCommand = InsertTransactionCommand.Replace("<AuctionPrice>", auction.AuctionPrice);

                        for (int z = 1; z < 68; z++)
                        {
                            InsertTransactionCommand = InsertTransactionCommand.Replace("<AuctionCountUser" + z.ToString() + ">", secondaryUsers[z - 1].AuctionCountUser);
                            InsertTransactionCommand = InsertTransactionCommand.Replace("<IsBiddingUser" + z.ToString() + ">", secondaryUsers[z - 1].IsBiddingUser);
                            InsertTransactionCommand = InsertTransactionCommand.Replace("<IsInPoolUser" + z.ToString() + ">", secondaryUsers[z - 1].IsInPoolUser);
                            InsertTransactionCommand = InsertTransactionCommand.Replace("<ScoreUser" + z.ToString() + ">", secondaryUsers[z - 1].ScoreUser);
                            InsertTransactionCommand = InsertTransactionCommand.Replace("<StatusUser" + z.ToString() + ">", secondaryUsers[z - 1].StatusUser);
                            InsertTransactionCommand = InsertTransactionCommand.Replace("<SuccessfulAuctionCountUser" + z.ToString() + ">", secondaryUsers[z - 1].SuccessfulAuctionCountUser);
                            InsertTransactionCommand = InsertTransactionCommand.Replace("<WalletUser" + z.ToString() + ">", secondaryUsers[z - 1].WalletUser);
                            if (z < 58)
                                InsertTransactionCommand = InsertTransactionCommand.Replace("<Channel" + z.ToString() + ">", channels[z - 1].AllocationStatus);
                        }

                        File.AppendAllText(@"C:\Users\Administrator\Desktop\TEL505E-Progress2\b.txt", InsertTransactionCommand);

                        Queries.InsertEvent(InsertTransactionCommand);
                        break;
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 2; i++)
            {
                GetInfoAndGenerateAnEvent(true);
            }
            for (int i = 0; i < 2; i++)
            {
                GetInfoAndGenerateAnEvent(false);
            }
        }



    }
}