using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MarketSentimentFinal.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        private bool _isBusy;

        // משמש להצגת אינדיקטור טעינה (Loading) במסכים
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        // הפונקציה שמעדכנת את ה-UI כשיש שינוי בנתונים
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}