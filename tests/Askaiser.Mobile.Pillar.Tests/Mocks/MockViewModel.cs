
using Askaiser.Mobile.Pillar.ViewModels;

namespace Askaiser.Mobile.Pillar.Tests.Mocks
{
    public class MockViewModel : PillarViewModelBase
    {
        private string _foo;
        public string Foo
        {
            get { return _foo; }
            set { Set(() => Foo, ref _foo, value); }
        }

    }
}
