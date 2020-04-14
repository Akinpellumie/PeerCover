using System.Collections.Generic;

namespace PeerCover.Models
{
    public class LoginProfileModel
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string token { get; set; }
        public string fcm_token { get; set; }
        public string Id { get; set; }

        public string username { get; set; }
        public string phonenumber { get; set; }
        public string email { get; set; }
        public string Status { get; set; }
        public string gender { get; set; }
        public string isActive { get; set; }
        public string community_code { get; set; }
        public string community_name { get; set; }
        public string department { get; set; }
        public string address { get; set; }
        public string bankname { get; set; }
        public string account_number { get; set; }
        public string account_name { get; set; }

        public string profile_img_url { get; set; }
        public string date_created { get; set; }
        public List<string> priviledges { get; set; }

        public string name
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
                return this.name.ToUpper();
            }
        }
    }

    public class LoginResponseModel
    {
      public List<LoginProfileModel> user { get; set; }
      public string message { get; set; }
    }

    public class ProfileModel
    {
        public string email { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }

        public string phonenumber { get; set; }


        public string username { get; set; }

        public string name
        {
            get
            {
                return this.firstname + "  " + this.lastname;
            }
        }

        public string capitalizedName
        {
            get
            {
                return this.name.ToUpper();
            }
        }

    }
}
