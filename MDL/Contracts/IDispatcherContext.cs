using System;

namespace Filewatcher.MDL
{
    public interface IDispatcherContext
    {
        bool IsSynchronized { get; }
        void Invoke(Action action);
        void BeginInvoke(Action action);
    }
}
