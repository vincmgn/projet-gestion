using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace projet_gestion.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifie la Vue lorsqu'une propriété change.
        /// </summary>
        /// <param name="propertyName">Nom de la propriété (automatiquement rempli par l'appelant).</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Met à jour une propriété et notifie la Vue si la valeur a changé.
        /// </summary>
        /// <typeparam name="T">Type de la propriété.</typeparam>
        /// <param name="field">Champ contenant la valeur actuelle.</param>
        /// <param name="value">Nouvelle valeur à affecter.</param>
        /// <param name="propertyName">Nom de la propriété (automatiquement rempli par l'appelant).</param>
        /// <returns>True si la valeur a changé, sinon False.</returns>
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
