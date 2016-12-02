namespace Askaiser.Mobile.Pillar.Interfaces
{
    public interface INavigationAware
    {
        void ViewEntering();

        void ViewEntered();

        void ViewLeaving();

        void ViewLeaved();
    }
}

