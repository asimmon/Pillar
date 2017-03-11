using System;
using System.Collections.ObjectModel;
using Pillar;
using PillarDemo.Models;
using Xamarin.Forms;

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

        public Command<Person> SayHelloCommand { get; private set; }

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

            SayHelloCommand = new Command<Person>(SayHello);
        }

        public void SayHello(Person person)
        {
            SelectedPerson = person;
        }
    }
}
