using Askaiser.Mobile.Pillar.ViewModels;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using PillarDemo.Models;

namespace PillarDemo.ViewModels
{
    public class MessengerViewModel : PillarViewModelBase
    {
        private readonly IMessenger _messenger;

        public RelayCommand ChangeCurrentUserCommand { get; private set; }

        public MessengerViewModel(IMessenger messenger)
        {
            _messenger = messenger;
            Title = "Messenger example";

            ChangeCurrentUserCommand = new RelayCommand(ChangeCurrentUser);
        }

        public void ChangeCurrentUser()
        {
            var user = new Person("Bob Smith", "foo@bar.com");

            _messenger.Send(new NotificationMessage<Person>(this, user, Constants.CurrentUserChanged));
        }
    }
}
