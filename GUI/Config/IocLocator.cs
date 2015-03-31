using Filewatcher.DAL;
using Filewatcher.MDL;
using Ninject;

namespace Filewatcher.GUI
{
    internal class IocLocator
    {
        public static StandardKernel Kernel = null;

        public IocLocator()
        {
            if (Kernel == null)
                Kernel = new StandardKernel();
            Kernel.Bind<IUow>().To<Uow>();
            Kernel.Bind<RepositoryFactories>().To<RepositoryFactories>().InSingletonScope();
            Kernel.Bind<IRepositoryProvider>().To<RepositoryProvider>();
            Kernel.Bind<IDispatcherContext>().To<DispatcherContext>();
        }

        public MainViewModel MainViewModel
        {
            get { return Kernel.Get<MainViewModel>(); }
        }

        public MacroViewModel MacroViewModel
        {
            get { return Kernel.Get<MacroViewModel>(); }
        }
    }
}
