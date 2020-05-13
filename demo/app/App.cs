using xf = Xamarin.Forms;

namespace Laconic.Demo
{
    public class App : xf.Application
    {
        public App()
        {
            var shell = new xf.Shell();
            xf.Shell.SetForegroundColor(shell, xf.Color.Chocolate);

            void AddSample<T>(string name) where T : xf.ContentPage, new()
            {
                var template = new xf.DataTemplate(() => new T {Title = name});

                var tab = new xf.Tab {Items = {new xf.ShellContent {ContentTemplate = template}}};
                shell.Items.Add(new xf.FlyoutItem {Title = name, Items = {tab}});
            }

            MainPage = shell;

            AddSample<CounterPage>("Counter");
            AddSample<Calculator>("Calculator (Grid)");
            AddSample<DynamicGrid>("Dynamic Grid");
            AddSample<GroupedCollectionView>("Collection View");
            AddSample<DancingBars>("Dancing Bars");
        }
    }
}