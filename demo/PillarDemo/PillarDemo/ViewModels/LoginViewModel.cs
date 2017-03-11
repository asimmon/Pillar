using System;
using System.Threading.Tasks;
using Pillar;
using PillarDemo.Models;
using Xamarin.Forms;

namespace PillarDemo.ViewModels
{
    public class LoginViewModel : PillarViewModelBase
    {
        private readonly INavigator _navigator;

        private string _email;
        private string _password;
        private bool _rememberMe;

        public string Email
        {
            get { return _email; }
            set
            {
                if (Set(() => Email, ref _email, value))
                    LoginCommand.ChangeCanExecute();
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (Set(() => Password, ref _password, value))
                    LoginCommand.ChangeCanExecute();
            }
        }

        public bool RememberMe
        {
            get { return _rememberMe; }
            set { Set(() => RememberMe, ref _rememberMe, value); }
        }

        public Command LoginCommand { get; private set; }

        public LoginViewModel(INavigator navigator)
        {
            _navigator = navigator;

            IsBusy = false;
            NoHistory = true;
            Title = "Login";
            LoginCommand = new Command(Login, CanLogin);
        }

        public async void Login()
        {
            IsBusy = true;

            var user = new Person("John Doe", Email);

            await Task.Delay(TimeSpan.FromSeconds(1));
            await _navigator.PushAsync<HomeViewModel>(vm =>
            {
                vm.CurrentUser = user;
            });

            IsBusy = false;
        }

        public bool CanLogin()
        {
            return !String.IsNullOrWhiteSpace(_email)
                && !String.IsNullOrWhiteSpace(_password)
                && !IsBusy;
        }
    }
}
