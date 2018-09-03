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
        private Action _blureAction;
        private Action _cancelBlure;
        private Action _close;
        private Action<IInstaApi> _setInstaApi;
        private Action<UserSessionData> _setUserSession;
        public LoginPage(ref UserSessionData user, ref IInstaApi instaApi, Action methdod, Action cancelBlureAction, Action closeLoadung, Action<IInstaApi> setInstaApi, Action<UserSessionData> setUSerUserSession)
        {
            InitializeComponent();
            _userSession = user;
            _instaApi = instaApi;
            _blureAction = methdod;
            _cancelBlure = cancelBlureAction;
            _close = closeLoadung;
            _setInstaApi = setInstaApi;
            _setUserSession = setUSerUserSession;
        }

        private async void Autorithation_OnClick(object sender, RoutedEventArgs e)
        {
            if (FieldIsNotNull())
            {
                _blureAction();
                Loading load = new Loading();
                load.Show();
                bool result = await Login(UserLogin.Text, Password.Password);
                _setInstaApi(_instaApi);
                _setUserSession(_userSession);
                _cancelBlure();
                load.Close();
                _close();

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
