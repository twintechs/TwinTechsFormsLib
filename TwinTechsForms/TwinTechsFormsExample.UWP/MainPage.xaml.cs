using Windows.UI.Xaml.Input;

namespace TwinTechsFormsExample.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.LoadApplication(new TwinTechs.App());
        }
    }
}
