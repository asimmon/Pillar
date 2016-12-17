using System.Collections.ObjectModel;
using Pillar.ViewModels;
using PillarDemo.Models;

namespace PillarDemo.ViewModels
{
    public class TemplateSelectorViewModel : PillarViewModelBase
    {
        public ObservableCollection<Animal> Animals { get; private set; }

        public TemplateSelectorViewModel()
        {
            Title = "TemplateSelector example";

            Animals = new ObservableCollection<Animal>
            {
                new Cat(),
                new Dog(),
                new Cat(),
                new Giraffe(),
                new Giraffe(),
                new Dog()
            };
        }
    }
}
