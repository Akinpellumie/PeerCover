using System;
using System.Collections.Generic;
using System.Text;

namespace PeerCover.Models
{
    public class SingleMemberModel
    {
        public string Id { get; set; }
        public string username { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string phonenumber { get; set; }
        public string email { get; set; }
        public string gender { get; set; }
        public string isActive { get; set; }
        public string community_code { get; set; }
        public string community_name { get; set; }
        public string department { get; set; }
        public string address { get; set; }
        public string bankname { get; set; }
        public string bank_code { get; set; }
        public string account_number { get; set; }
        public string account_name { get; set; }

        public string profile_img_url { get; set; }
        public string date_created { get; set; }
        public string Pr_time
        {
            get
            {
                var r = DateTime.Parse(this.date_created.Replace("[UTC]", ""));
                return r.ToLocalTime().ToShortTimeString();
            }
        }

        public string Pr_date
        {
            get
            {

                var r = DateTime.Parse(this.date_created.Replace("[UTC]", ""));
                return r.ToLocalTime().ToShortDateString();
            }
        }
        public List<ClaimsListModel> claims { get; set; }
        public List<PolicyDetailsModel> policyDetails { get; set; }
        public List<string> priviledges { get; set; }
        public string FullName
        {
            get
            {
                return this.firstname + "  " + this.lastname;
            }
        }
        public string capName
        {
            get
            {
                return this.FullName.ToUpper();
            }
        }

    } 
    
    public class PolicyDetailsModel
    {
        public string subscription_id { get; set; }
        public string vehicle_make { get; set; }
        public string vehicle_registration_number { get; set; }
        public string vehicle_color { get; set; }
        public string year_of_make { get; set; }
        public string engine_number { get; set; }
        public string VIN { get; set; }
        public string driver_license_url { get; set; }
        public string utility_bill_url { get; set; }
        public string plan_id { get; set; }
        public string username { get; set; }
        public string policy_number { get; set; }
        public string valuation_amount { get; set; }
        public string premium { get; set; }
        public string coverage_period { get; set; }
        public string coverage_amount { get; set; }
        public string status { get; set; }
        public string is_expired { get; set; }

    }

    public class MemberDetailsModel
    {
        public List<SingleMemberModel> member { get; set; }
    }
}
