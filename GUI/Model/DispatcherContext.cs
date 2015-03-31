using System.Threading;
using System.Windows.Threading;
using Filewatcher.MDL;
using System;

namespace Filewatcher.GUI
{
    public class DispatcherContext : IDispatcherContext
    {
        public DispatcherContext() : this(Dispatcher.CurrentDispatcher){}

        public DispatcherContext(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        private readonly Dispatcher _dispatcher;

        public bool IsSynchronized { get { return _dispatcher.Thread == Thread.CurrentThread; } }

        public void Invoke(Action action)
        {
            _dispatcher.Invoke(action);
        }

        public void BeginInvoke(Action action)
        {
            _dispatcher.BeginInvoke(action);
        }
    }
}
