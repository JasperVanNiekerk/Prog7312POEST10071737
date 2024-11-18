using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Prog7312POEST10071737.Core
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        //___________________________________________________________________________________________________________

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        //___________________________________________________________________________________________________________

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
//____________________________________EOF_________________________________________________________________________