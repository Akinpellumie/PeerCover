using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PeerCover.Models
{
    public class TransactionModel
    {
        public string subscriptionId { get; set; }
	    public string paymentAmount { get; set; }
        public string vehicleImgUrl { get; set; }
        public string  policyNumber { get; set; }
        public string depositorsUsername { get; set; }
        public string recipientUsername { get; set; }
        public string depositorsName { get; set; }
        public string recipientName { get; set; }
        public string accountNumber { get; set; }
        public string accountName { get; set; }
        public string bankName { get; set; }
        public string transactionType { get; set; }
        public string communityCode { get; set; }
        public string vehicleMake { get; set; }
        public string policyHolder { get; set; }
        public string policyHolderContact { get; set; }
        public string paymentMethod { get; set; }
    }

    public class TransResModel
    {
        public string transactionReferenceNumber { get; set; }
        public string transactionId { get; set; }
        public string inHubRefNum { get; set; }
    }

    public class TransHisModel
    {
        public string transaction_id { get; set; }
        public string subscription_id { get; set; }
        public string claim_id {get; set;}
        public string payment_amount { get; set; }
        public string PmtAmountFormat
        {
            get
            {
                return Math.Round(Convert.ToDouble(this.payment_amount), 2).ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en-us")).Replace("$", "₦");
            }
        }
        public string policy_number { get; set; }
        public string payment_status {get; set;}
        public Color PayStatusColor
        {
            get
            {
                if (payment_status.Contains("Pending"))
                {
                    return Color.FromHex("FA9917");
                }
                else if (payment_status.Contains("Successful"))
                {
                    return Color.Green;
                }
                else if (payment_status.Contains("Failed"))
                {
                    return Color.Red;
                }
                return Color.FromHex("FA9917");
            }
        }
        public string transaction_type { get; set; }
        public string PaymentBody
        {
            get
            {
                if (payment_status.Contains("Pending"))
                {
                    return ("Your premium subscription process is ongoing. Kindly wait for further confirmation.");
                }
                else if (payment_status.Contains("Successful"))
                {
                    return ("You have successfully subscribed for your " + vehicle_make + " Insurance");
                }
                else if (payment_status.Contains("Failed"))
                {
                    return ("Your premium subscription process has failed. Please Try Again.");
                }
                return ("Premium subscription Ongoing!");
            }
        }
        public string transaction_ref_number {get; set;}
        public string policy_holder { get; set; }
        public string recipients_username { get; set; }
        public string recipient_name { get; set; }
        public string depositor_username { get; set; }
        public string account_number { get; set; }
        public string account_name { get; set; }
        public string date_created { get; set; }
        public string DC_time
        {
            get
            {
                var r = DateTime.Parse(this.date_created.Replace("[UTC]", ""));
                return r.ToLocalTime().ToShortTimeString();
            }
        }

        public string DC_date
        {
            get
            {

                var r = DateTime.Parse(this.date_created.Replace("[UTC]", ""));
                return r.ToLocalTime().ToShortDateString();
            }
        }
        public string date_paid { get; set; }
        public string DP_time
        {
            get
            {
                var r = DateTime.Parse(this.date_created.Replace("[UTC]", ""));
                return r.ToLocalTime().ToShortTimeString();
            }
        }

        public string DP_date
        {
            get
            {

                var r = DateTime.Parse(this.date_created.Replace("[UTC]", ""));
                return r.ToLocalTime().ToShortDateString();
            }
        }
        public string DapDate
        {
            get
            {
                return this.DP_date + ", " + this.DP_time;
            }
        }
        public string depositors_name { get; set; }
        public string payment_method { get; set; }
        public string bank_name { get; set; }
        public string community_code { get; set; }
        public string vehicle_make { get; set; }
        public string community_name { get; set; }
    }

    public class TransModel
    {
        public List<TransHisModel> transactions { get; set; }
    }

    public class PayMethModel
    {
        public string username { get; set; }
        public string paymentMethod { get; set; }
        public string transRefNum { get; set; }
        public string transactionId { get; set; }
    }
}
