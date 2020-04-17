using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PeerCover.Models
{
    public class PlansModel
    {
        public string plan_description { get; set; }
        public string plan_name { get; set; }
        public string plan_id { get; set; }
        public string date_created { get; set; }
    }

    public class PlansListModel
    {
        public List<PlansModel> plans { get; set; }
    }

    public class NewPlanModel
    {
        public string planId { get; set; }
        public string vin { get; set; }
        public string vehicleMake { get; set; }
         public string vehicleColor { get; set; }
        public string vehicleRegNum { get; set; }
        public string yearOfMake { get; set; }
        public string engineNumber { get; set; }
        public string driverLicenseUrl { get; set; }
        public string utilityBillUrl { get; set; }
        public string vehicleImage { get; set; }
        public string username { get; set; }
        public string premium { get; set; }
        public string policyNumber { get; set; }
        public string subscriptionId { get; set; }
        public string coveragePeriod { get; set; }
        public string valuationAmount { get; set; }

        public NewPlanModel(string username, string vin, string vehicleMake, string vehicleRegNum, string engineNumber, string yearOfMake, string vehicleColor, string valuationAmount)
        {
            this.username = username;
            this.vehicleMake = vehicleMake;
            this.engineNumber = engineNumber;
            this.vehicleRegNum = vehicleRegNum;
            this.yearOfMake = yearOfMake;
            this.vehicleColor = vehicleColor;
            this.vin = vin;
            this.valuationAmount = valuationAmount;

        }

        public NewPlanModel()
        {
        }

        public bool CheckInformation()
        {
            if (!this.username.Equals("") && !this.vehicleMake.Equals("") && !this.vehicleColor.Equals("") && !this.vehicleRegNum.Equals("") && !this.valuationAmount.Equals("") && !this.premium.Equals("") && !this.yearOfMake.Equals(""))
                return true;
            else
                return false;
        }

    }

    public class SubscriptionModel
    {
        public string subscription_id {get; set;}
        public string vehicle_make { get; set; }
        public string vehicle_registration_number { get; set; }
        public string vehicle_color { get; set; }
        public string year_of_make { get; set; }
        public string engine_number { get; set; }
        public string VIN { get; set; }
        public string driver_license_url { get; set; }
        public string fileName { get; set; }
        public string utility_bill_url { get; set; }
        public string vehicle_image { get; set; }
        public string plan_id { get; set; }
        public string username { get; set; }
        public string policy_number { get; set; }
        public string valuation_amount {get; set;}
        public string VMamountformat
        {
            get
            {
                return Math.Round(Convert.ToDouble(this.valuation_amount), 2).ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en-us")).Replace("$", "₦");
            }
        }
        public double premium { get; set; }
        public string PRamountformat
        {
            get
            {
                return Math.Round(Convert.ToDouble(this.premium), 2).ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en-us")).Replace("$", "₦");
            }
        }
        
        public string coverage_period { get; set; }
        public string coverage_amount { get; set; }
        public string CAamountformat
        {
            get
            {
                return Math.Round(Convert.ToDouble(this.coverage_amount), 2).ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en-us")).Replace("$", "₦");
            }
        }
        public string status { get; set; }
        public Color Status_color
        {
            get
            {
                if (status.Contains("Not Paid"))
                {
                    return Color.FromHex("FA9917");
                }
                else if (status.Contains("Paid"))
                {
                    return Color.Green;
                }
                return Color.FromHex("FA9917");
            }
        }
        public string is_expired { get; set; }
        public string exDate
        {
            get
            {
                if (string.IsNullOrEmpty(expiry_date))
                {
                    return this.expiry_date;
                }
                else if (!string.IsNullOrEmpty(expiry_date))
                {
                    return this.expDate;
                }
                return expiry_date;
            }
        }
        public string expiry_date { get; set; }
        public string expDate
        {
            get
            {

                var date = DateTime.Parse(this.expiry_date.Replace("[UTC]", ""));
                return date.ToLocalTime().ToShortDateString();
            }
        }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string community_name { get; set; }
        public ImageSource UserImage
        {

            get
            {

                Uri source = new Uri(Helper.ImageUrl + vehicle_image);

                return source;
            }
            set { UserImage = value; }
        }
        public ImageSource PlanImage
        {

            get
            {

                Uri source = new Uri(Helper.ImageUrl + vehicle_image);

                return source;
            }
            set { PlanImage = value; }
        }
    }

    public class ActiveSubModel
    {
        public List<SubscriptionModel> subscription { get; set; }
    }

    public class ActSubModel
    {
        public List<SubscriptionModel> subscriptions { get; set; }
    }
}
