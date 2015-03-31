namespace Filewatcher.MDL
{
    public interface IUow
    {   
        void Commit();
        IObservableRepository<Watch> Watches { get; }
        IObservableRepository<History> Histories { get; } 
    }
}