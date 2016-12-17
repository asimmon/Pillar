using System;
using Pillar.Interfaces;
using Pillar.ViewModels;
using GalaSoft.MvvmLight.Command;

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

        public RelayCommand ShowAlertCommand { get; private set; }

        public RelayCommand ShowConfirmCommand { get; private set; }

        public RelayCommand ShowActionSheetCommand { get; private set; }

        public DialogViewModel(IDialogProvider dialogProvider)
        {
            _dialogProvider = dialogProvider;

            Title = "Dialog example";
            LatestAction = "Did nothing yet";

            ShowAlertCommand = new RelayCommand(ShowAlert);
            ShowConfirmCommand = new RelayCommand(ShowConfirm);
            ShowActionSheetCommand = new RelayCommand(ShowActionSheet);
        }

        public async void ShowAlert()
        {
            await _dialogProvider.DisplayAlert("Alert dialog", "This is a simple alert dialog", "OK");

            LatestAction = "Showed an alert dialog";
        }

        public async void ShowConfirm()
        {
            bool confirmed = await _dialogProvider.DisplayAlert("Confirm dialog", "This is a simple confirm dialog", "OK", "Cancel");

            LatestAction = String.Format("Showed an alert dialog and clicked {0}", (confirmed ? "OK" : "Cancel"));
        }

        public async void ShowActionSheet()
        {
            var chosenOption = await _dialogProvider.DisplayActionSheet("ActionSheet example", "Cancel", null, "Foo", "Bar", "Qux");

            LatestAction = String.Format("Showed an action sheet and choosed {0}", chosenOption);
        }
    }
}
