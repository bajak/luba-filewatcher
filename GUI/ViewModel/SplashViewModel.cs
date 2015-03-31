using System;
using System.Timers;
using System.Windows;
using Filewatcher.MDL;

namespace Filewatcher.GUI
{
    public class SplashViewModel : PropertyBase
    {
        private readonly DispatcherContext _dispatcherContext;

        public SplashViewModel(DispatcherContext dispatcher, Window startView, Action closeSplashView, bool useTimer, int timerDuration)
        {
            _dispatcherContext = dispatcher;
            UseTimer = useTimer;
            TimerDuration = timerDuration;

            InitializeCommands();
            InitailizeMainView(startView, closeSplashView);
        }


        public RelayCommand OpenInBrowser 
        { 
            get { return GetProperty<RelayCommand>(); } 
            set { SetProperty(value); }
        }

        public object TimerText 
        { 
            get { return GetProperty<object>(); } 
            set { SetProperty(value); } 
        }

        public bool UseTimer
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        public int TimerDuration 
        { 
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        private void InitailizeMainView(Window startView, Action closeSplashView)
        {
            if (!UseTimer)
            {
                startView.Loaded += (s, e) => closeSplashView();
                startView.Show();
            }
            else 
            {
                TimerText = TimerDuration + Properties.Resources.SPLASH_LABEL_TIMELEFT;
                var timer = new Timer { Enabled = true, Interval = 1000, AutoReset = true };
                var timerCounter = TimerDuration;
                timer.Elapsed += (sa, ea) => _dispatcherContext.Invoke(() => {
                    timerCounter--;
                    TimerText = timerCounter + Properties.Resources.SPLASH_LABEL_TIMELEFT;
                    if (timerCounter > 0)
                        return;
                    timer.Enabled = false;
                    if (startView.IsVisible)
                        return;
                    startView.Show();
                    closeSplashView();
                });
                timer.Start();
            }
        }

        private void InitializeCommands()
        {
            OpenInBrowser = new RelayCommand(param =>
                ViewHelper.OpenBrowser(param.ToString()),
            param => true);
        }
    }
}
