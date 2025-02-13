
using System.Collections.ObjectModel;


namespace HotelAsgard.ViewModels
{
    public class userViewModel
    {
        public ObservableCollection<UserModel> UserModels { get; set; }


        public userViewModel()
        {
            UserModels = new ObservableCollection<UserModel>();
        }
    }
}
