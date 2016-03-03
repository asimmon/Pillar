namespace Askaiser.Mobile.Pillar.Tests.Views
{
    public class NamedDummyObject
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public NamedDummyObject(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
