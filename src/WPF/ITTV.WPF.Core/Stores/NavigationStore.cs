using System;
using System.Collections.Generic;
using System.Linq;
using ITTV.WPF.Abstractions.Base.ViewModel;

namespace ITTV.WPF.Core.Stores
{
    public sealed class NavigationStore
    {
        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                if (Equals(_currentViewModel, value))
                    return;

                if (_currentViewModel != null && 
                    value != null && !_historyViewModels.Contains(value))
                {
                    _historyViewModels.Add(_currentViewModel);
                    OnHistoryViewModelsUpdated();
                }
                
                _currentViewModel = value;
                
                OnCurrentViewModelChanged();
            }
        }

        private readonly List<ViewModelBase> _historyViewModels = new();
        public IReadOnlyCollection<ViewModelBase> HistoryViewModels => _historyViewModels;
        public event Action HistoryViewModelsUpdated;

        public event Action CurrentViewModelChanged;

        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }

        private void OnHistoryViewModelsUpdated()
        {
            HistoryViewModelsUpdated?.Invoke();
        }

        public bool CanNavigateBack()
            => HistoryViewModels.Count > 1;

        public void NavigateBack()
        {
            if (!CanNavigateBack()) 
                return;
            
            CurrentViewModel.Dispose();
            CurrentViewModel = _historyViewModels.Last();
                
            _historyViewModels.Remove(_historyViewModels.Last());
            OnHistoryViewModelsUpdated();
        }
    }
}