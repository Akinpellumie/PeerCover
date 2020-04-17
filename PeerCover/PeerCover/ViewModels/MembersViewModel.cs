using PeerCover.GroupHelper;
using PeerCover.Models;
using System.Collections.ObjectModel;

namespace PeerCover.ViewModels
{
    public class MembersViewModel
    {
        public ObservableCollection<MembersModel> members { get; set; }
        public ObservableCollection<Grouping<string, MembersModel>> MembersGrouped { get; set; }

        public MembersViewModel()
        {

        }
        public MembersViewModel(bool designData)
        {
            if (designData)
            {
                members = MemberHelper.members;
                MembersGrouped = MemberHelper.MembersGrouped;
            }
            else
            {
                //Setup web requests and such
            }
        }

        public bool IsBusy { get; set; }

        public int MemberCount => members?.Count ?? 0;
    }
}
