using System.ComponentModel;
using System.Runtime.CompilerServices;
using ITTV.WPF.MVVM.Annotations;

namespace ITTV.WPF.MVVM.Views
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        { }
        
        public string StageOfClasses { get; set; }
        public string DayLongFormat { get; set; }
        public string TimeShortFormat { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}