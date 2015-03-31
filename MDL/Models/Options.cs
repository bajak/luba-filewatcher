namespace Filewatcher.MDL
{
    public class Options : ValidationBase
    {
        public string OutputPath 
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }
    }
}
