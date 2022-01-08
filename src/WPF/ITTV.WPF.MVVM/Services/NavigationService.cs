﻿using ITTV.WPF.MVVM.Stores;
using ITTV.WPF.MVVM.ViewModels;

namespace ITTV.WPF.MVVM.Services
{
    public class NavigationService<TViewModel> where TViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly TViewModel _createdViewModel;

        public NavigationService(NavigationStore navigationStore, TViewModel createdViewModel)
        {
            _navigationStore = navigationStore;
            _createdViewModel = createdViewModel;
        }

        public void Navigate()
        {
            _navigationStore.CurrentViewModel = _createdViewModel;
        }
    }
}