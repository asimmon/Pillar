using System;
using Pillar;
using PillarDemo.Models;
using Xamarin.Forms;

namespace PillarDemo.ViewModels
{
    public class HomeViewModel : PillarViewModelBase, IDisposable
    {
        private readonly INavigator _navigator;
        private readonly IMessenger _messenger;

        private Person _currentUser;

        public Person CurrentUser
        {
            get { return _currentUser; }
            set { Set(() => CurrentUser, ref _currentUser, value); }
        }

        public Command GoToEventToCommandCommand { get; private set; }

        public Command GoToTemplateSelectorCommand { get; private set; }

        public Command GoToMessengerCommand { get; private set; }

        public Command GoToDialogCommand { get; private set; }

        public HomeViewModel(INavigator navigator, IMessenger messenger)
        {
            _navigator = navigator;
            _messenger = messenger;

            Title = "Demos";

            GoToEventToCommandCommand = new Command(GoToEventToCommand);
            GoToTemplateSelectorCommand = new Command(GoToTemplateSelector);
            GoToMessengerCommand = new Command(GoToMessenger);
            GoToDialogCommand = new Command(GoToDialog);

            messenger.Subscribe<MessengerViewModel, Person>(this, Constants.CurrentUserChanged, CurrentUserChanged);
        }

        public async void GoToEventToCommand()
        {
            await _navigator.PushAsync<EventToCommandViewModel>();
        }

        public async void GoToTemplateSelector()
        {
            await _navigator.PushAsync<TemplateSelectorViewModel>();
        }

        public async void GoToMessenger()
        {
            await _navigator.PushAsync<MessengerViewModel>();
        }

        public async void GoToDialog()
        {
            await _navigator.PushAsync<DialogViewModel>();
        }

        public void CurrentUserChanged(MessengerViewModel sender, Person person)
        {
            CurrentUser = person;
        }

        public void Dispose()
        {
            _messenger.Unsubscribe<MessengerViewModel, string>(this, Constants.CurrentUserChanged);
        }
    }
}
