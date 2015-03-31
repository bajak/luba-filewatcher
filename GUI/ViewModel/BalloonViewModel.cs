using Filewatcher.MDL;

namespace Filewatcher.GUI
{
    public class BalloonViewModel : PropertyBase
    {
        public BalloonViewModel(string name, string target)
        {
            NameText = name;
            TargetText = target;
        }

        public string NameText
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }   
        }

        public string TargetText
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }
    }
}
