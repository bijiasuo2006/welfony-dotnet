using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Welfony.App.Common
{
    public class ObservableObject : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private bool disposedValue;

        public ObservableObject()
        {
        }

        public T? Clone<T>() where T : ObservableObject
        {
            if (this is null)
            {
                return null;
            }
            return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(this as T));
        }

        protected virtual void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new NotSupportedException("Raising the PropertyChanged event with an empty string or null is not supported.");
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(((MemberExpression)propertyExpression.Body).Member.Name));
            }
        }

        protected void Set<T>(Expression<Func<T>> propertyExpression, ref T field, T newValue)
        {
            ArgumentNullException.ThrowIfNull(propertyExpression);
            Set(((MemberExpression)propertyExpression.Body).Member.Name, ref field, newValue);
        }

        protected virtual void Set<T>(string propertyName, ref T field, T newValue)
        {
            ArgumentNullException.ThrowIfNull(propertyName);
            if (!EqualityComparer<T>.Default.Equals(field, newValue))
            {
                field = newValue;
                RaisePropertyChanged(propertyName);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
