﻿using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ITTV.WPF.DataModel.Models;
using ITTV.WPF.Views;

namespace ITTV.WPF.Interface.Pages
{
    /// <summary>
    /// Логика взаимодействия для News.xaml
    /// </summary>
    public partial class NewsPage : UserControl
    {
        private readonly MainWindow _mainWindow;
        private List<BitmapImage> ImageList = new List<BitmapImage>();
        int index = 0;

        public NewsPage(News news, MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            InitializeComponent();

            Left.Height = Left.Width;
            Right.Height = Right.Width;

            ImageList = news.ImageList;

            Title.Text = news.Title;
            Content.Text = news.Content;
            CurrentImage.Source = ImageList[index];

            if (ImageList.Count == 1)
            {
                Right.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _mainWindow.UiInvoked();

            switch (((Button)sender).Name)
            {
                case "Left":
                    if (index - 1 >= 0)
                    {
                        CurrentImage.Source = ImageList[index - 1];

                        Right.Visibility = System.Windows.Visibility.Visible;
                        index--;

                        if (index == 0)
                        {
                            Left.Visibility = System.Windows.Visibility.Hidden;
                        }
                    }
                    break;
                case "Right":
                    if (index + 1 < ImageList.Count && index > -1)
                    {
                        CurrentImage.Source = ImageList[index + 1];
                        index++;

                        Left.Visibility = System.Windows.Visibility.Visible;

                        if(index + 1 == ImageList.Count)
                        {
                            Right.Visibility = System.Windows.Visibility.Hidden;
                        }
                    }
                    break;
                }
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            _mainWindow.UiInvoked();
        }
    }
}
