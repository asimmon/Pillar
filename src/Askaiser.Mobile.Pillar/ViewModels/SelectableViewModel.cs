using Askaiser.Mobile.Pillar.Interfaces;

namespace Askaiser.Mobile.Pillar.ViewModels
{
    public class SelectableViewModel : PillarViewModelBase, ISelectable
    {
        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set  {  Set(() => IsSelected, ref _isSelected, value); }
        }
    }
}

