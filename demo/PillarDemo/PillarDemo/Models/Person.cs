using Pillar;

namespace PillarDemo.Models
{
    public class Person : ObservableObject
    {
        private string _name;
        private string _email;

        public string Name
        {
            get { return _name; }
            set { Set(() => Name, ref _name, value); }
        }

        public string Email
        {
            get { return _email; }
            set { Set(() => Email, ref _email, value); }
        }

        public Person(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public Person(string name)
            : this(name, null)
        { }

        public override string ToString()
        {
            return $"{Name} <{Email}>";
        }
    }
}
