using System;
using System.Collections.Generic;
using System.Linq;
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
using InstaSharper.API.Builder;
using InstaSharper.Classes;
using InstaSharper.Classes.Models;
using InstaSharper.Logger;

namespace InstaCheck.WinPage
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : UserControl
    {
        private UserSessionData _userSession;
        private IInstaApi _instaApi;
        private Action blureAction;
        private Action cancelBlure;
        public LoginPage(ref UserSessionData user, ref IInstaApi instaApi, Action methdod, Action cancelBlureAction)
        {
            InitializeComponent();
            _userSession = user;
            _instaApi = instaApi;
            blureAction = methdod;
            cancelBlure = cancelBlureAction;
        }

        private async void Autorithation_OnClick(object sender, RoutedEventArgs e)
        {
            if (FieldIsNotNull())
            {
                blureAction();
                Loading load = new Loading();
                load.Show();
                bool result = await Login(UserLogin.Text, Password.Password);
                cancelBlure();
                load.Close();
                

            }
           
        }

        private async Task<bool> Login(string name, string password)
        {
            _userSession = new UserSessionData
            {
                UserName = name,
                Password = password
            };

            IRequestDelay delay = RequestDelay.FromSeconds(2, 2);
            _instaApi = InstaApiBuilder.CreateBuilder()
                .SetUser(_userSession)
                .UseLogger(new DebugLogger(LogLevel.Exceptions))
                .SetRequestDelay(delay)
                .Build();
            IResult<InstaLoginResult> loggedIn = await _instaApi.LoginAsync();
            return loggedIn.Succeeded;
           
        }

        private bool FieldIsNotNull()
        {
            if (UserLogin.Text != string.Empty && UserLogin.Text.Replace(" ", "") != string.Empty)
            {
                return true;
            }
            return false;
        }
    }
}
