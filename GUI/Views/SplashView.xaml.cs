using System.Windows;
using System.Windows.Media.Animation;

namespace Filewatcher.GUI
{
    /// <summary>
    /// Interaktionslogik für SplashView.xaml
    /// </summary>
    public partial class SplashView : Window
    {
        public SplashView(Window startView, bool useTimer, int timerDuration)
        {
            InitializeComponent();
            DataContext = new SplashViewModel(new DispatcherContext(Dispatcher), startView, Close, useTimer, timerDuration);
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var animation = (Storyboard)Resources["FadeOut"];
            Closing -= WindowClosing;
            e.Cancel = true;
            animation.Completed += (s, f) => Close();
            animation.Begin();
        }
    }
}
