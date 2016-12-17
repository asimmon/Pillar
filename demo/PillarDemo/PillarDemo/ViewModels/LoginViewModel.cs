using System;
using System.Threading.Tasks;
using Pillar.Services;
using Pillar.ViewModels;
using GalaSoft.MvvmLight.Command;
using PillarDemo.Models;

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
                    LoginCommand.RaiseCanExecuteChanged();
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (Set(() => Password, ref _password, value))
                    LoginCommand.RaiseCanExecuteChanged();
            }
        }

        public bool RememberMe
        {
            get { return _rememberMe; }
            set { Set(() => RememberMe, ref _rememberMe, value); }
        }

        public RelayCommand LoginCommand { get; private set; }

        public LoginViewModel(INavigator navigator)
        {
            _navigator = navigator;

            IsBusy = false;
            NoHistory = true;
            Title = "Login";
            LoginCommand = new RelayCommand(Login, CanLogin);
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
