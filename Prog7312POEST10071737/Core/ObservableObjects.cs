using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Prog7312POEST10071737.Core
{
    public class ObservableObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Event that is raised when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        //___________________________________________________________________________________________________________

        /// <summary>
        /// Raises the PropertyChanged event for the specified property.
        /// </summary>
        /// <param name="name">The name of the property that changed. If not provided, the calling member name will be used.</param>
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Raises the PropertyChanged event for the specified property.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
//____________________________________EOF_________________________________________________________________________