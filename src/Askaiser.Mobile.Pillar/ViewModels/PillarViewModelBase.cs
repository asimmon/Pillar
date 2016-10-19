namespace Askaiser.Mobile.Pillar.ViewModels
{
    /// <summary>
    /// The preffered ViewModel class of every ViewModel which can be bound to a View
    /// in the app bootstrapping configuration.
    /// It already contains some useful observable properties.
    /// </summary>
    /// <seealso cref="GalaSoft.MvvmLight.ViewModelBase" />
    /// <seealso cref="IViewModel" />
    public abstract class PillarViewModelBase : ViewModelBase, IViewModel
    {
        private string _title;
        private bool _isBusy;

        /// <summary>
        /// Represents the page Title.
        /// It's up to you to bind this property to a Page title.
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { Set(() => Title, ref _title, value); }
        }

        /// <summary>
        /// If true, the associated Page will not exists in the navigation stack.
        /// A good example is a login screen, once you are logged in and on a different page,
        /// if you press the back button you won't return to the login screen.
        /// </summary>
        public bool NoHistory { get; set; }

        /// <summary>
        /// FOR XAML USE ONLY!
        /// Magical object which allow us to use the ViewModelLocator pattern without the limitations of a static context.
        /// It means that we can use dependency injection, that we can write testable code without static classed or methods.
        /// You will need to register a class with singleton lifecycle (the ViewModelLocator instance)
        /// and add an RegistrationActivated event handler in Autofac.
        /// Every time that a IViewModel will be requested you will resolve and inject the the ViewModelLocator instance:
        /// 
        /// <example>
        /// <code lang="C#">
        /// private static void RegistrationActivated(object sender, ActivatedEventArgs&lt;object&gt; e)
        /// {
        ///     var vm = e.Instance as IViewModel;
        ///     if (vm != null)
        ///     {
        ///         vm.Locator = e.Context.Resolve&lt;ViewModelLocator&gt;();
        ///     }
        /// }
        /// </code>
        /// </example>
        /// 
        /// See the online documentation for a concrete example.
        /// </summary>
        public object Locator { get; set; }

        /// <summary>
        /// Indicates that a long running or background process is running.
        /// You might want to use this with a BooleanConverter to show / hide controls during the process.
        /// </summary>
        public bool IsBusy
        {
            get { return _isBusy; }
            set { Set(() => IsBusy, ref _isBusy, value); }
        }

        /// <summary>
        /// Method called when you navigate (go to) this ViewModel.
        /// </summary>
        public virtual void NavigatedTo()
        { }

        /// <summary>
        /// Method called when you navigate (go back) to this ViewModel.
        /// </summary>
        public virtual void NavigatedFrom()
        { }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public virtual void Dispose()
        { }
    }
}

