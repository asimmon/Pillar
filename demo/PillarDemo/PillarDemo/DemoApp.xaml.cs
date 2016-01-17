namespace PillarDemo
{
    public partial class DemoApp
    {
        public DemoApp()
        {
            InitializeComponent();

            new DemoBootstrapper(this).Run();
        }
    }
}
