using Pillar.Interfaces;

namespace Pillar.ViewModels
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

