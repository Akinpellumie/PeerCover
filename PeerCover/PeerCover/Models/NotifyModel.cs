using System;
using System.Collections.Generic;
using System.Text;

namespace PeerCover.Models
{
    public class NotifyModel
    {
        public string id { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public string sender { get; set; }
        public string recipient { get; set; }
        public string username { get; set; }
        public string date_sent { get; set; }
       
        public string DateFormat
        {
            get
            {

                var r = DateTime.Parse(this.date_sent.Replace("[UTC]", ""));
                return r.ToLocalTime().ToShortDateString();
            }
        }
        public string link_type { get; set; }
        public string link_type_id { get; set; }
        public int is_read { get; set; }
        public int isRead { get; set; }
        public string read_by { get; set; }
        public string date_read { get; set; }
        public string notificationId { get; set; }
    }

    public class NotificationsModel
    {
        public List<NotifyModel> notifications { get; set; }
    }

    public class UpdateNotification
    {
        public string username {get; set;}
        public int isRead { get; set; }
        public string notificationId { get; set; }
    }
}
