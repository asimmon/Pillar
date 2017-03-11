using Pillar;
using PillarDemo.Models;
using Xamarin.Forms;

namespace PillarDemo.ViewModels
{
    public class MessengerViewModel : PillarViewModelBase
    {
        private readonly IMessenger _messenger;

        public Command ChangeCurrentUserCommand { get; private set; }

        public MessengerViewModel(IMessenger messenger)
        {
            _messenger = messenger;
            Title = "Messenger example";

            ChangeCurrentUserCommand = new Command(ChangeCurrentUser);
        }

        public void ChangeCurrentUser()
        {
            var user = new Person("Bob Smith", "foo@bar.com");

            _messenger.Send(this, Constants.CurrentUserChanged, user);
        }
    }
}
