using System;
using System.Collections.ObjectModel;
using Askaiser.Mobile.Pillar.ViewModels;
using GalaSoft.MvvmLight.Command;
using PillarDemo.Models;

namespace PillarDemo.ViewModels
{
    public class EventToCommandViewModel : PillarViewModelBase
    {
        private Person _selectedPerson;

        public Person SelectedPerson
        {
            get { return _selectedPerson; }
            set { Set(() => SelectedPerson, ref _selectedPerson, value); }
        }

        public ObservableCollection<Person> People { get; private set; }

        public RelayCommand<Person> SayHelloCommand { get; private set; }

        public EventToCommandViewModel()
        {
            Title = "EventToCommand example";
            SelectedPerson = null;

            People = new ObservableCollection<Person>
            {
                new Person("John Doe"),
                new Person("Harry Stone"),
                new Person("Thierry Smith")
            };

            SayHelloCommand = new RelayCommand<Person>(SayHello);
        }

        public void SayHello(Person person)
        {
            SelectedPerson = person;
        }
    }
}
