using System.Threading;

namespace Filewatcher.GUI
{
    public partial class App
    {
        private static Mutex mutex;

        protected override void OnStartup(System.Windows.StartupEventArgs e)
        {
            bool result;
            mutex = new Mutex(true, "ab331c8f-a681-4129-85a8-bf4ebc262543", out result);

            if (!result) {
                Shutdown();
                return;
            }
            base.OnStartup(e);
            LoadSplashView();
        }

        private static void LoadSplashView()
        {
            var splashView = new SplashView(new MainView(), ViewConfig.SPLASH_USETIMER, ViewConfig.SPLASH_TIMERDURATION);
            splashView.Show();
        }
    }
}
