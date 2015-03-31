using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Animation;
using Timer = System.Timers.Timer;

namespace Filewatcher.GUI
{
    public partial class BalloonView
    {
        public BalloonView(string name, string target, int duration)
        {
            InitializeComponent();
            SetPosition();
            DataContext = new BalloonViewModel(name, target);
            var timer  = new Timer {Enabled = true, Interval = duration, AutoReset = true };
            timer.Elapsed += (s, e) => { Dispatcher.Invoke(Close); };
            timer.Start();
        }

        private void SetPosition()
        {
            Activated += delegate
            {
                var workingArea = Screen.PrimaryScreen.WorkingArea;
                var transform = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
                var corner = transform.Transform(new Point(workingArea.Right, workingArea.Bottom));

                Left = corner.X - ActualWidth - 10;
                Top = corner.Y - ActualHeight - 10;
            };

        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var animation = (Storyboard)Resources["FadeOut"];
            Closing -= WindowClosing;
            e.Cancel = true;
            animation.Completed += (s, f) => Close();
            animation.Begin();
        }

        public static void ShowBalloon(string name, string target, int duration)
        {
            new BalloonView(name, target, duration).Show();
        }
    }
}
