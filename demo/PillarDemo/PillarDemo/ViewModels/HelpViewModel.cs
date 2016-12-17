using System;
using Pillar.Interfaces;
using Pillar.ViewModels;
using GalaSoft.MvvmLight.Command;

namespace PillarDemo.ViewModels
{
    public class HelpViewModel : PillarViewModelBase
    {
        private readonly IDialogProvider _dialogProvider;

        public RelayCommand ShowLoginHelpCommand { get; private set; }

        public HelpViewModel(IDialogProvider dialogProvider)
        {
            _dialogProvider = dialogProvider;

            ShowLoginHelpCommand = new RelayCommand(ShowLoginHelp);
        }

        public async void ShowLoginHelp()
        {
            string[] help = {
                "Please checkout the code source to see how this ViewModelLocator works.",
                "There isn't any class declared as static here. It's all wired up with Autofac,",
                "so you can use D.I. inside the ViewModelLocator class."
            };

            await _dialogProvider.DisplayAlert("ViewModelLocator example", String.Join(Environment.NewLine, help), "OK");
        }
    }
}
