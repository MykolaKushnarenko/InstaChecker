using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using InstaSharper.API;
using InstaSharper.Classes;
using InstaSharper.Classes.Models;

namespace InstaCheck.WinPage
{
    /// <summary>
    /// Interaction logic for UserProfil.xaml
    /// </summary>
    public partial class UserProfil : UserControl
    {
        private IInstaApi _instaApi;
        private UserSessionData _userSession;
        public UserProfil(ref IInstaApi instaApi, ref UserSessionData userSessionData)
        {
            InitializeComponent();
            _instaApi = instaApi;
            _userSession = userSessionData;
            UpdateProfilInfo();
        }

        private async void UpdateProfilInfo()
        {
            IResult<InstaUserInfo> userinfo = await _instaApi.GetUserInfoByIdAsync(_userSession.LoggedInUder.Pk);
            FollowersCount.Text = userinfo.Value.FollowerCount.ToString();
            FollowingCount.Text = userinfo.Value.FollowingCount.ToString();
            ProfilPhoto.Source = BitmapFrame.Create(new Uri(userinfo.Value.ProfilePicUrl));
            
            //IResult<InstaUser> user = await _instaApi.GetUserAsync("aeroflot");
            //IResult<InstaUserInfo> info = await _instaApi.GetUserInfoByIdAsync(user.Value.Pk);
            //IResult<InstaUserShortList> folows = await _instaApi.GetUserFollowersAsync(_userSession.UserName, PaginationParameters.Empty);
            //IResult<InstaMedia> userinfo2 = await _instaApi.GetMediaByIdAsync(folows.Value[43].ProfilePictureId);
        }

    }
}
