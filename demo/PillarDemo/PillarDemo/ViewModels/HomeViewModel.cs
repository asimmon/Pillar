using Askaiser.Mobile.Pillar.Services;
using Askaiser.Mobile.Pillar.ViewModels;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using PillarDemo.Models;

namespace PillarDemo.ViewModels
{
    public class HomeViewModel : PillarViewModelBase
    {
        private readonly INavigator _navigator;
        private readonly IMessenger _messenger;

        private Person _currentUser;

        public Person CurrentUser
        {
            get { return _currentUser; }
            set { Set(() => CurrentUser, ref _currentUser, value); }
        }

        public RelayCommand GoToEventToCommandCommand { get; private set; }

        public RelayCommand GoToTemplateSelectorCommand { get; private set; }

        public RelayCommand GoToMessengerCommand { get; private set; }

        public RelayCommand GoToDialogCommand { get; private set; }

        public HomeViewModel(INavigator navigator, IMessenger messenger)
        {
            _navigator = navigator;
            _messenger = messenger;

            Title = "Demos";

            GoToEventToCommandCommand = new RelayCommand(GoToEventToCommand);
            GoToTemplateSelectorCommand = new RelayCommand(GoToTemplateSelector);
            GoToMessengerCommand = new RelayCommand(GoToMessenger);
            GoToDialogCommand = new RelayCommand(GoToDialog);

            messenger.Register<NotificationMessage<Person>>(this, CurrentUserChanged);
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

        public void CurrentUserChanged(NotificationMessage<Person> msg)
        {
            if (msg.Notification == Constants.CurrentUserChanged)
            {
                CurrentUser = msg.Content;
            }
        }

        public override void Dispose()
        {
            _messenger.Unregister(this);
        }
    }
}
