﻿using System.ComponentModel;
using System.Threading.Tasks;

namespace ITTV.WPF.Abstractions.Base.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public bool IsLoaded
        {
            get => _isLoaded;
            set
            {
                if (Equals(_isLoaded, value))
                    return;
                _isLoaded = value;
                OnPropertyChanged(nameof(IsLoaded));
            }
        }
        private bool _isLoaded;

        protected void SetLoaded()
        {
            IsLoaded = true;
        }
        
        protected void SetUnloaded()
        {
            IsLoaded = false;
        }

        public virtual Task Recalculate()
        {
            return Task.CompletedTask;
        }

        
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void Dispose() { }
    }
}