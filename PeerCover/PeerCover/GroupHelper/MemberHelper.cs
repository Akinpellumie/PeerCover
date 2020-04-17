using PeerCover.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace PeerCover.GroupHelper
{
    public static class MemberHelper
    {

        private static Random random;

        public static MembersModel GetRandomMember()
        {
            //var output = Newtonsoft.Json.JsonConvert.SerializeObject(Monkeys);
            return members[random.Next(0, members.Count)];
        }


        public static ObservableCollection<Grouping<string, MembersModel>> MembersGrouped { get; set; }

        public static ObservableCollection<MembersModel> members { get; set; }

        static MemberHelper()
        {
            random = new Random();
            members = new ObservableCollection<MembersModel>();
            

            var sorted = from member in members
                         orderby member.firstname
                         group member by member.NameSort into MemberGroup
                         select new Grouping<string, MembersModel>(MemberGroup.Key, MemberGroup);

            MembersGrouped = new ObservableCollection<Grouping<string, MembersModel>>(sorted);

        }
    }
}
