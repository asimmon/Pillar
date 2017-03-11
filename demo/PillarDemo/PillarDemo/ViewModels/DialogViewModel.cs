using System;
using Pillar;
using Xamarin.Forms;

namespace PillarDemo.ViewModels
{
    public class DialogViewModel : PillarViewModelBase
    {
        private readonly IDialogProvider _dialogProvider;

        private string _latestAction;

        public string LatestAction
        {
            get { return _latestAction; }
            set { Set(() => LatestAction, ref _latestAction, value); }
        }

        public Command ShowAlertCommand { get; private set; }

        public Command ShowConfirmCommand { get; private set; }

        public Command ShowActionSheetCommand { get; private set; }

        public DialogViewModel(IDialogProvider dialogProvider)
        {
            _dialogProvider = dialogProvider;

            Title = "Dialog example";
            LatestAction = "Did nothing yet";

            ShowAlertCommand = new Command(ShowAlert);
            ShowConfirmCommand = new Command(ShowConfirm);
            ShowActionSheetCommand = new Command(ShowActionSheet);
        }

        public async void ShowAlert()
        {
            await _dialogProvider.DisplayAlert("Alert dialog", "This is a simple alert dialog", "OK");

            LatestAction = "Showed an alert dialog";
        }

        public async void ShowConfirm()
        {
            bool confirmed = await _dialogProvider.DisplayAlert("Confirm dialog", "This is a simple confirm dialog", "OK", "Cancel");

            LatestAction = $"Showed an alert dialog and clicked {(confirmed ? "OK" : "Cancel")}";
        }

        public async void ShowActionSheet()
        {
            var chosenOption = await _dialogProvider.DisplayActionSheet("ActionSheet example", "Cancel", null, "Foo", "Bar", "Qux");

            LatestAction = $"Showed an action sheet and choosed {chosenOption}";
        }
    }
}
