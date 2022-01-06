﻿using System.Windows.Controls;
using ITTV.WPF.MVVM.ViewModels;

namespace ITTV.WPF.MVVM.Views
{
    public partial class MenuView : UserControl
    {
        public MenuView(MenuViewModel menuViewModel)
        {
            InitializeComponent();
            DataContext = menuViewModel;
        }
    }
}