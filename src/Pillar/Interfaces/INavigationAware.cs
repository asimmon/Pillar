namespace Pillar.Interfaces
{
    /// <summary>
    /// Provides navigation events
    /// </summary>
    public interface INavigationAware
    {
        /// <summary>
        /// Method called before that the associated view is pushed on the navigation stack
        /// </summary>
        void ViewEntering();

        /// <summary>
        /// Method called after that the associated view is pushed on the navigation stack
        /// </summary>
        void ViewEntered();

        /// <summary>
        /// Method called before that the associated view is poped from the navigation stack
        /// </summary>
        void ViewLeaving();

        /// <summary>
        /// Method called after that the associated view is poped from the navigation stack
        /// </summary>
        void ViewLeaved();
    }
}

