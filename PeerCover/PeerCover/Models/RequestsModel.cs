using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PeerCover.Models
{
    public class RequestsModel
    {
        public string requested_by { get; set; }
	    public string firstname { get; set; }
	    public string lastname { get; set; }
	    public string community_code { get; set; }
	    public string request_id { get; set; }
	    public string date_reviewed { get; set; }
	    public string is_reviewed { get; set; }
	    public string date_created { get; set; }
	    public string status { get; set; }
        public Color ReqStatus_color
        {
            get
            {
                if (status.Contains("Accepted"))
                {
                    return Color.FromHex("37CA4B");
                }
                else if (status.Contains("Declined"))
                {
                    return Color.Red;
                }
                return Color.Orange;
            }
        }
        public string reviewedBy { get; set; }
        public string requestId { get; set; }
        public string reviewed_by { get; set; }
        public string Rename
        {
            get
            {
                return this.firstname + "  " + this.lastname + " " + "wants to join the community."; 
            }
        }
    }

    public class ReqModel
    {
        public List<RequestsModel> requests { get; set; }
    }

    public class AcceptReqModel
    {
        public string requested_by { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string community_code { get; set; }
        public string request_id { get; set; }
        public string date_reviewed { get; set; }
        public string is_reviewed { get; set; }
        public string date_created { get; set; }
        public string status { get; set; }
        public Color ReqStatus_color
        {
            get
            {
                if (status.Contains("Accepted"))
                {
                    return Color.FromHex("37CA4B");
                }
                else if (status.Contains("Declined"))
                {
                    return Color.Red;
                }
                return Color.Orange;
            }
        }
        public string reviewedBy { get; set; }
        public string requestId { get; set; }
        public string reviewed_by { get; set; }
        public string Rename
        {
            get
            {
                return this.firstname + "  " + this.lastname + " " + "has been accepted to the community.";
            }
        }
    }

    public class AccReqModel
    {
        public List<AcceptReqModel> requests { get; set; }
    }

    public class DeclineReqModel
    {
        public string requested_by { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string community_code { get; set; }
        public string request_id { get; set; }
        public string date_reviewed { get; set; }
        public string is_reviewed { get; set; }
        public string date_created { get; set; }
        public string status { get; set; }
        public Color ReqStatus_color
        {
            get
            {
                if (status.Contains("Accepted"))
                {
                    return Color.Green;
                }
                else if (status.Contains("Declined"))
                {
                    return Color.Red;
                }
                return Color.Orange;
            }
        }
        public string reviewedBy { get; set; }
        public string requestId { get; set; }
        public string reviewed_by { get; set; }
        public string Rename
        {
            get
            {
                return this.firstname + "  " + this.lastname + " " + "request has been declined.";
            }
        }
    }

    public class DecReqModel
    {
        public List<DeclineReqModel> requests { get; set; }
    }
}
