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

        private string GetValueByColumnName(DataTable dataTable,string columnName)
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
            for(int i= 1; i <= 57; i++)
            {
                string columnName = "Channel" + i.ToString();
                string value = GetValueByColumnName(dataTable,columnName);
                if(value == "0")
                 return i.ToString();
            }
            return "0";
        }
        private string GetBandWidthOfChannel(string auctionChannel)
        {
            int channelNo=int.Parse(auctionChannel);

            if (channelNo < 41)
                return "5";
            else
                return "20";
        }

        private string GetPriceOfAuction(string bandWidth)
        {
            int channelNo = int.Parse(bandWidth);

            if (channelNo ==5)
                return "0.4";
            else
                return "1.0";
        }

        private string SetNewTimeStamp(string timeStampOld)
        {
            
            DateTime timestamp = DateTime.ParseExact(timeStampOld, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            // Increment the timestamp by one minute
            DateTime incrementedTimestamp = timestamp.AddMinutes(1);

            // Convert the incremented timestamp back to string
            string incrementedTimestampString = incrementedTimestamp.ToString("dd.MM.yyyy HH:mm:ss");

            return incrementedTimestampString;
        }

        private void LoadAuctionValuesToUI(Auction auction)
        {
           
            textBoxTimeStamp.Text = auction.TimeStamp;
            textBoxLogType.Text = auction.LogType;
            textBoxAuctionNo.Text = auction.AuctionNo;
            textBoxAuctionChannel.Text = auction.AuctionChannel;
            textBoxAuctionChannelBandwidth.Text = auction.AuctionChannelBandwidth;
            textBoxAuctionPrice.Text=auction.AuctionPrice;

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

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable table = Queries.SelectLatestRow();
            dataGridView1.DataSource=table;
            string [,] parametersAndValues = new string[548,2];
            int i = 0;

            //Load Latest Values
            foreach(DataColumn column in table.Columns)
            {
                parametersAndValues[i,0]=column.ColumnName;
                parametersAndValues[i, 1] = table.Rows[0][i].ToString();
                i++;
            }
            //Auction
            Auction auction = new Auction();
            auction.TimeStamp = SetNewTimeStamp(GetValueByColumnName(table,"TimeStamp"));
            auction.AuctionNo = (int.Parse(GetValueByColumnName(table, "AuctionNo"))+1).ToString();
            auction.AuctionChannel= GetFirstAvailableChannel(table);
            auction.AuctionChannelBandwidth = GetBandWidthOfChannel(auction.AuctionChannel);
            auction.AuctionPrice = GetPriceOfAuction(auction.AuctionChannelBandwidth);
            LoadAuctionValuesToUI(auction);

            //PU
            PrimaryUser primaryUser = new PrimaryUser();
            primaryUser.TotalSpectrumKHz = "540";
            primaryUser.AvailableSpectrumKHz = GetAvailableSpectrum(table);
            primaryUser.PrimaryUserAuctionCount= (int.Parse(GetValueByColumnName(table, "PrimaryUserAuctionCount")) + 1).ToString();
            primaryUser.PrimaryUserID = "1111";
            
            //After Auction
            primaryUser.PrimaryUserWallet = GetValueByColumnName(table, "PrimaryUserWallet");
            primaryUser.AppropriateResponseCount = "";
            primaryUser.ResponseIsAppropriate = "";
            primaryUser.PrimaryUserStatus = "";
            primaryUser.TotalRequestCount = "";
            primaryUser.PrimaryUserSuccessfulAuctionCount = "";
            primaryUser.PrimaryUserReputationScore = "";

            SpectrumResources[] channels = new SpectrumResources[57];
            //SU
            SecondaryUser[] secondaryUser = new SecondaryUser[67];

        }
    }
}