using System;
using System.Collections.Generic;
using System.Text;

namespace PeerCover.Models
{
    public class AppShellModel
    {
        public string fullname
        {
            get
            {
                return HelperAppSettings.capName;
            }
        }
        public string commCode
        {
            get
            {
                return HelperAppSettings.community_code;
            }
        }
        public string commName1
        {
            get
            {
                return HelperAppSettings.community_name;
            }
        }
    }
}
