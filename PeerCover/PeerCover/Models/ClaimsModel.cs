using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace PeerCover.Models
{
    public class ClaimsModel
    {

        public string id { get; set; }
        public string lossAssessorquotationDoc { get; set; }
        public string quotationDocUrl { get; set; }
        public string lossAssessorAmount { get; set; }
        public string rejectionMessage { get; set; }
        public string lossAssessorUsername { get; set; }
        public string claimOwnerUsername { get; set; }

        public string policy_holder { get; set; }

        public string community_code { get; set; }

        public string status { get; set; }
        public Color Status_color
        {
            get
            {
                if (status.Contains("Reported"))
                {
                    return Color.FromHex("FA9917");
                }
                else if (status.Contains("Recommendation Accepted"))
                {
                    return Color.Green;
                }
                else if (status.Contains("Declined"))
                {
                    return Color.Red;
                }
                return Color.FromHex("FA9917");
            }
        }
        public string policy_number { get; set; }
        public string incident_cause { get; set; }
        public string incident_report { get; set; }
        public string loss_assessor_claim_amount { get; set; }
        public string LACAamountformat
        {
            get
            {
                return Math.Round(Convert.ToDouble(this.loss_assessor_claim_amount), 2).ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en-us")).Replace("$", "₦");
            }
        }
        public string policy_holder_claim_amount { get; set; }
        public string PHCAamountformat
        {
            get
            {
                return Math.Round(Convert.ToDouble(this.policy_holder_claim_amount), 2).ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en-us")).Replace("$", "₦");
            }
        }
        public string consultant_claim_amount { get; set; }
        public string CCAamountformat
        {
            get
            {
                return Math.Round(Convert.ToDouble(this.consultant_claim_amount), 2).ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en-us")).Replace("$", "₦");
            }
        }
        public string is_accepted { get; set; }
        public string claim_holder_quotationdoc_url { get; set; }
        public string loss_assessor_quotationdoc_url { get; set; }
        public string date_created { get; set; }
        public string conDate
        {
            get
            {
                return this.P_date + ", " + this.P_time;
            }
        }
        public string P_time
        {
            get
            {
                var r = DateTime.Parse(this.date_created.Replace("[UTC]", ""));
                return r.ToLocalTime().ToShortTimeString();
            }
        }

        public string P_date
        {
            get
            {

                var r = DateTime.Parse(this.date_created.Replace("[UTC]", ""));
                return r.ToLocalTime().ToShortDateString();
            }
        }

        public string community_name { get; set; }
        public List<ImgUrlArray> claimImages { get; set; }

        public string imgUrl { get; set; }
        public string lastname { get; set; }
        public string firstname { get; set; }
        public string name { get; set; }
        public string vehicle_make { get; set; }
        public string Myname
        {
            get
            {
                return this.firstname + "  " + this.lastname;
            }
        }
        public string capitalizedname
        {
            get
            {
                return this.Myname.ToUpper();
            }
        }

        public ImageSource ClaimsImage
        {

            get
            {

                Uri source = new Uri(Helper.ImageUrl + imgUrl);

                return source;
            }
            set { ClaimsImage = value; }
        }

    }
    public class Data
    {
        public string hope { get; set; }
    }

    public class ClaimsListModel
    {
        public List<ClaimsModel> claims { get; set; }
        public List<Data> data { get; set; }
        public string message { get; set; }
    }
    public class SingleClaimListModel
    {
        public List<ClaimsModel> claim { get; set; }
        
    }

    public class ImgUrlArray
    {
        public string id { get; set; }
        public string claimId { get; set; }
        public string imgUrl { get; set; }

        public ImageSource ClaimsImage
        {

            get
            {

                Uri source = new Uri(Helper.ImageUrl + imgUrl);

                return source;
            }
            set { ClaimsImage = value; }
        }
    }
    public class ImageClaimModel
    {
        public List<ImgUrlArray> claimImages { get; set; }
    }

    public class MakeClaimModel
    {
        public string policyHolder { get; set; }
        public string subscriptionId { get; set; }
        public string bankName { get; set; }
        public string bankCode { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string accountName { get; set; }
        public string accountNumber { get; set; }
        public string policyHolderClaimAmount { get; set; }
        public string claimOwnerQuotationDoc { get; set; }
        public string claimRolePlayed { get; set; }
        public string incidentCause { get; set; }
        public string incidentReport { get; set; }
        public string communityCode { get; set; }
        public string policyNumber { get; set; }
        public List<string> imgDamageUrl { get; set; }
    }

        public class LossAssModel
        {
        public string lossAssessorAmount { get; set; }
        public string status { get; set; }
        public string lossAssessorUsername { get; set; }
        public string claimId { get; set; }
        public string quotationDocUrl { get; set; }
        }

    public class AcceptAmtModel
    {
       public string claimOwnerUsername { get; set; }
       public string status { get; set; }
       public string claimId { get; set; }
    }

    public class DeclineAmtModel
    {
        public string claimOwnerUsername { get; set; }
        public string status { get; set; }
        public string claimId { get; set; }
        public string rejectionMessage { get; set; }
    }
}
