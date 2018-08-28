using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using InstaCheck.WinPage;
using InstaSharper.API;
using InstaSharper.API.Builder;
using InstaSharper.Classes;
using InstaSharper.Classes.Models;
using InstaSharper.Logger;
using Notifications.Wpf;

namespace InstaCheck
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UserSessionData _userSession;
        private BlurEffect blurEffect;
        private IInstaApi _instaApi;
        public MainWindow()
        {
            InitializeComponent();
            GridContent.Children.Add(new LoginPage(ref _userSession, ref _instaApi, UseBlereEffect, CancleBlereEffect));
            
        }

        private void UseBlereEffect()
        {
            blurEffect = new BlurEffect();
            blurEffect.Radius = 7;
            Effect = blurEffect;
        }
        private void CancleBlereEffect()
        {
            blurEffect.Radius = 0;
            Effect = blurEffect;
        }

        private void NotificationToas(string UnffolowUserName)
        {
            NotificationManager notificationManager = new NotificationManager();
            notificationManager.Show(new NotificationContent
            {
                Title = "Instagrame Checher",
                Message = $"[ {UnffolowUserName} ] unfollow you",
                Type = NotificationType.Error
            });
        }
        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            IResult<InstaUserInfo> userinfo = await _instaApi.GetUserInfoByIdAsync(_userSession.LoggedInUder.Pk);
            IResult<InstaUser> user = await _instaApi.GetUserAsync("aeroflot");
            IResult<InstaUserInfo> info = await _instaApi.GetUserInfoByIdAsync(user.Value.Pk);
            IResult<InstaUserShortList> folows = await _instaApi.GetUserFollowersAsync(_userSession.UserName, PaginationParameters.Empty);
            IResult<InstaMedia> userinfo2 = await _instaApi.GetMediaByIdAsync(folows.Value[43].ProfilePictureId);



            using (HttpClient client = new HttpClient())
            {

                HttpResponseMessage imageResponse = await client.GetAsync(userinfo2.Value.Images[0].URI);
                //InMemoryRandomAccessStream randomAccess = new InMemoryRandomAccessStream();
                //DataWriter writer = new DataWriter(randomAccess.GetOutputStreamAt(0));
                //writer.WriteBytes(await imageResponse.Content.ReadAsByteArrayAsync());
                //await writer.StoreAsync();
                //BitmapImage bm = new BitmapImage();
                //await bm.SetSourceAsync(randomAccess);
                //myImg.Source = bm;
            }

        }
    }
}
