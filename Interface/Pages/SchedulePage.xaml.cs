using Microsoft.Samples.Kinect.ControlsBasics.DataModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Microsoft.Samples.Kinect.ControlsBasics.Interface.Pages
{
    /// <summary>
    /// Логика взаимодействия для SchedulePage.xaml
    /// </summary>
    public partial class SchedulePage : UserControl
    {
        public SchedulePage(List<Lesson> lessons)
        {
            InitializeComponent();

            mainGrid.Margin = new Thickness(100, 50, 100, 50);

            bool empty = true;
            foreach (Lesson lesson in lessons)
            {
                if (lesson.lesson != null)
                {
                    empty = false;
                    break;
                }
            }

            if (!empty) {
                createDayLessons(lessons, mainGrid);
            } else {
                OutList.Visibility = Visibility.Visible;
            }
        }

        public SchedulePage(FullSchedule lessons)
        {
            InitializeComponent();

            mainGrid.Margin = new Thickness(100, 50, 100, 50);

            for (int i = 0; i < 12; i++)
            {
                RowDefinition lessonsRow = new RowDefinition();
                lessonsRow.Height = new GridLength(1, GridUnitType.Star);
                mainGrid.RowDefinitions.Add(lessonsRow);


                TextBlock dayText = new TextBlock();
                dayText.TextAlignment = TextAlignment.Center;
                dayText.FontSize = 30;
                dayText.FontWeight = FontWeights.Bold;
                Grid.SetRow(dayText, i);
                dayText.Margin = new Thickness(0, 100, 0, 0);
                if (i % 2 == 0) mainGrid.Children.Add(dayText);


                Grid lessonsGrid = new Grid();
                lessonsGrid.Margin = new Thickness(15);
                Grid.SetRow(lessonsGrid, i);
                if (i % 2 == 1) mainGrid.Children.Add(lessonsGrid);


                switch (i)
                {
                    case 0:
                        dayText.Text = "Понедельник";
                        dayText.Margin = new Thickness(0);
                        break;
                    case 1:
                        createDayLessons(lessons.first.monday, lessons.second.monday, lessonsGrid);
                        break;
                    case 2:
                        dayText.Text = "Вторник";
                        break;
                    case 3:
                        createDayLessons(lessons.first.tuesday, lessons.second.tuesday, lessonsGrid);
                        break;
                    case 4:
                        dayText.Text = "Среда";
                        break;
                    case 5:
                        createDayLessons(lessons.first.wednesday, lessons.second.wednesday, lessonsGrid);
                        break;
                    case 6:
                        dayText.Text = "Четверг";
                        break;
                    case 7:
                        createDayLessons(lessons.first.thursday, lessons.second.thursday, lessonsGrid);
                        break;
                    case 8:
                        dayText.Text = "Пятница";
                        break;
                    case 9:
                        createDayLessons(lessons.first.friday, lessons.second.friday, lessonsGrid);
                        break;
                    case 10:
                        dayText.Text = "Суббота";
                        break;
                    case 11:
                        createDayLessons(lessons.first.saturday, lessons.second.saturday, lessonsGrid);
                        break;
                }
            }
        }

        private void createDayLessons(List<Lesson> dayLessons, Grid grid)
        {
            for (int i = 0; i < 9; i++)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                if (i % 2 == 0) {
                    switch (i)
                    {
                        case 0:
                            columnDefinition.Width = new GridLength(2, GridUnitType.Star);
                            break;
                        case 2:
                            columnDefinition.Width = new GridLength(20, GridUnitType.Star);
                            break;
                        case 4:
                            columnDefinition.Width = new GridLength(3, GridUnitType.Star);
                            break;
                        case 6:
                            columnDefinition.Width = new GridLength(7, GridUnitType.Star);
                            break;
                        case 8:
                            columnDefinition.Width = new GridLength(5, GridUnitType.Star);
                            break;
                    }
                } else
                {
                    columnDefinition.Width = GridLength.Auto;
                }
                grid.ColumnDefinitions.Add(columnDefinition);

                if (i % 2 == 1)
                {
                    Rectangle separator = new Rectangle();
                    separator.Fill = Brushes.Black;
                    separator.Width = 2;
                    Grid.SetColumn(separator, i);
                    Grid.SetRowSpan(separator, dayLessons.Count * 2 - 1);
                    grid.Children.Add(separator);
                }
            }

            for (int i = 0; i < dayLessons.Count * 2 - 1; i++)
            {
                RowDefinition rowDefinition = new RowDefinition();
                if (i % 2 == 0) {
                    rowDefinition.Height = GridLength.Auto;
                    rowDefinition.MinHeight = 100;
                } 
                else
                {
                    rowDefinition.Height = GridLength.Auto;
                }
                grid.RowDefinitions.Add(rowDefinition);

                if (i % 2 == 1)
                {
                    Rectangle separator = new Rectangle();
                    separator.Fill = Brushes.Black;
                    separator.Height = 2;
                    Grid.SetRow(separator, i);
                    Grid.SetColumnSpan(separator, grid.ColumnDefinitions.Count);
                    grid.Children.Add(separator);
                }
            }

            for (int i = 0; i < dayLessons.Count; i++)
            {
                TextBlock number = new TextBlock();
                number.Text = (i + 1).ToString();
                number.Margin = new Thickness(30);
                number.FontWeight = FontWeights.SemiBold;
                number.TextAlignment = TextAlignment.Center;
                Grid.SetRow(number, i * 2);
                Grid.SetColumn(number, 0);
                grid.Children.Add(number);

                if (dayLessons[i].lesson != null)
                {
                    TextBlock name = new TextBlock();
                    name.Margin = new Thickness(30);
                    name.Text = dayLessons[i].lesson.name.Replace('\n', ' ');
                    name.TextAlignment = TextAlignment.Center;
                    name.TextWrapping = TextWrapping.Wrap;
                    name.FontSize = 28;
                    name.FontWeight = FontWeights.SemiBold;
                    Grid.SetRow(name, i * 2);
                    Grid.SetColumn(name, 2);
                    grid.Children.Add(name);

                    TextBlock pair = new TextBlock();
                    pair.Text = dayLessons[i].lesson.type;
                    pair.Margin = new Thickness(30);
                    pair.TextAlignment = TextAlignment.Center;
                    pair.TextWrapping = TextWrapping.Wrap;
                    pair.FontSize = 28;
                    pair.FontWeight = FontWeights.SemiBold;
                    Grid.SetRow(pair, i * 2);
                    Grid.SetColumn(pair, 4);
                    grid.Children.Add(pair);

                    TextBlock teacher = new TextBlock();
                    teacher.Text = dayLessons[i].lesson.teacher;
                    teacher.Margin = new Thickness(30);
                    teacher.TextAlignment = TextAlignment.Center;
                    teacher.TextWrapping = TextWrapping.Wrap;
                    teacher.FontSize = 28;
                    teacher.FontWeight = FontWeights.SemiBold;
                    Grid.SetRow(teacher, i * 2);
                    Grid.SetColumn(teacher, 6);
                    grid.Children.Add(teacher);

                    TextBlock classRoom = new TextBlock();
                    classRoom.Margin = new Thickness(30);
                    classRoom.Text = dayLessons[i].lesson.classRoom;
                    classRoom.TextAlignment = TextAlignment.Center;
                    classRoom.TextWrapping = TextWrapping.Wrap;
                    classRoom.FontSize = 28;
                    classRoom.FontWeight = FontWeights.SemiBold;
                    Grid.SetRow(classRoom, i * 2);
                    Grid.SetColumn(classRoom, 8);
                    grid.Children.Add(classRoom);
                }
            }
        }

        private void createDayLessons(List<Lesson> neChetLessons, List<Lesson> chetLessons, Grid grid)
        {
            for (int i = 0; i < 11; i++)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                if (i % 2 == 0)
                {
                    switch (i)
                    {
                        case 0:
                        case 2:
                            columnDefinition.Width = new GridLength(2, GridUnitType.Star);
                            break;
                        case 4:
                            columnDefinition.Width = new GridLength(20, GridUnitType.Star);
                            break;
                        case 6:
                            columnDefinition.Width = new GridLength(3, GridUnitType.Star);
                            break;
                        case 8:
                            columnDefinition.Width = new GridLength(7, GridUnitType.Star);
                            break;
                        case 10:
                            columnDefinition.Width = new GridLength(5, GridUnitType.Star);
                            break;
                    }
                }
                else
                {
                    columnDefinition.Width = GridLength.Auto;
                }
                grid.ColumnDefinitions.Add(columnDefinition);

                if (i % 2 == 1)
                {
                    Rectangle separator = new Rectangle();
                    separator.Fill = Brushes.Black;
                    separator.Width = 2;
                    Grid.SetColumn(separator, i);
                    Grid.SetRowSpan(separator, maxLessons(neChetLessons, chetLessons) * 4 - 1);
                    grid.Children.Add(separator);
                }
            }

            for (int i = 0; i < maxLessons(neChetLessons, chetLessons) * 4 - 1; i++)
            {
                RowDefinition rowDefinition = new RowDefinition();
                if (i % 2 == 0)
                {
                    rowDefinition.Height = GridLength.Auto;
                    rowDefinition.MinHeight = 100;
                }
                else
                {
                    rowDefinition.Height = GridLength.Auto;
                }
                grid.RowDefinitions.Add(rowDefinition);

                if (i % 2 == 1)
                {
                    Rectangle separator = new Rectangle();
                    separator.Fill = Brushes.Black;
                    separator.Height = 2;
                    Grid.SetRow(separator, i);
                    Grid.SetColumn(separator, ((i + 1) % 4 == 0) ? 0 : 2);
                    Grid.SetColumnSpan(separator, grid.ColumnDefinitions.Count - ((i + 1) % 4 == 0 ? 0 : 2));
                    grid.Children.Add(separator);
                }
            }

            for (int i = 0; i < maxLessons(neChetLessons, chetLessons); i++)
            {
                TextBlock number = new TextBlock();
                number.Text = (i + 1).ToString();
                number.Margin = new Thickness(30);
                number.FontWeight = FontWeights.SemiBold;
                number.TextAlignment = TextAlignment.Center;
                Grid.SetRow(number, i * 4);
                Grid.SetColumn(number, 0);
                Grid.SetRowSpan(number, 3);
                grid.Children.Add(number);

                TextBlock evenWeek = new TextBlock();
                evenWeek.Text = "|";
                evenWeek.Margin = new Thickness(30);
                evenWeek.FontWeight = FontWeights.SemiBold;
                evenWeek.TextAlignment = TextAlignment.Center;
                Grid.SetRow(evenWeek, i * 4);
                Grid.SetColumn(evenWeek, 2);
                grid.Children.Add(evenWeek);

                TextBlock unevenWeek = new TextBlock();
                unevenWeek.Text = "||";
                unevenWeek.Margin = new Thickness(30);
                unevenWeek.FontWeight = FontWeights.SemiBold;
                unevenWeek.TextAlignment = TextAlignment.Center;
                Grid.SetRow(unevenWeek, i * 4 + 2);
                Grid.SetColumn(unevenWeek, 2);
                grid.Children.Add(unevenWeek);

                if (neChetLessons[i].lesson != null)
                {
                    TextBlock name = new TextBlock();
                    name.Margin = new Thickness(30);
                    name.Text = neChetLessons[i].lesson.name.Replace('\n', ' ');
                    name.TextAlignment = TextAlignment.Center;
                    name.TextWrapping = TextWrapping.Wrap;
                    name.FontSize = 28;
                    name.FontWeight = FontWeights.SemiBold;
                    Grid.SetRow(name, i * 4);
                    Grid.SetColumn(name, 4);
                    grid.Children.Add(name);

                    TextBlock pair = new TextBlock();
                    pair.Text = neChetLessons[i].lesson.type;
                    pair.Margin = new Thickness(30);
                    pair.TextAlignment = TextAlignment.Center;
                    pair.TextWrapping = TextWrapping.Wrap;
                    pair.FontSize = 28;
                    pair.FontWeight = FontWeights.SemiBold;
                    Grid.SetRow(pair, i * 4);
                    Grid.SetColumn(pair, 6);
                    grid.Children.Add(pair);

                    TextBlock teacher = new TextBlock();
                    teacher.Text = neChetLessons[i].lesson.teacher;
                    teacher.Margin = new Thickness(30);
                    teacher.TextAlignment = TextAlignment.Center;
                    teacher.TextWrapping = TextWrapping.Wrap;
                    teacher.FontSize = 28;
                    teacher.FontWeight = FontWeights.SemiBold;
                    Grid.SetRow(teacher, i * 4);
                    Grid.SetColumn(teacher, 8);
                    grid.Children.Add(teacher);

                    TextBlock classRoom = new TextBlock();
                    classRoom.Margin = new Thickness(30);
                    classRoom.Text = neChetLessons[i].lesson.classRoom;
                    classRoom.TextAlignment = TextAlignment.Center;
                    classRoom.TextWrapping = TextWrapping.Wrap;
                    classRoom.FontSize = 28;
                    classRoom.FontWeight = FontWeights.SemiBold;
                    Grid.SetRow(classRoom, i * 4);
                    Grid.SetColumn(classRoom, 10);
                    grid.Children.Add(classRoom);
                }

                if (chetLessons[i].lesson != null)
                {
                    TextBlock name = new TextBlock();
                    name.Margin = new Thickness(30);
                    name.Text = chetLessons[i].lesson.name.Replace('\n', ' ');
                    name.TextAlignment = TextAlignment.Center;
                    name.TextWrapping = TextWrapping.Wrap;
                    name.FontSize = 28;
                    name.FontWeight = FontWeights.SemiBold;
                    Grid.SetRow(name, i * 4 + 2);
                    Grid.SetColumn(name, 4);
                    grid.Children.Add(name);

                    TextBlock pair = new TextBlock();
                    pair.Text = chetLessons[i].lesson.type;
                    pair.Margin = new Thickness(30);
                    pair.TextAlignment = TextAlignment.Center;
                    pair.TextWrapping = TextWrapping.Wrap;
                    pair.FontSize = 28;
                    pair.FontWeight = FontWeights.SemiBold;
                    Grid.SetRow(pair, i * 4 + 2);
                    Grid.SetColumn(pair, 6);
                    grid.Children.Add(pair);

                    TextBlock teacher = new TextBlock();
                    teacher.Text = chetLessons[i].lesson.teacher;
                    teacher.Margin = new Thickness(30);
                    teacher.TextAlignment = TextAlignment.Center;
                    teacher.TextWrapping = TextWrapping.Wrap;
                    teacher.FontSize = 28;
                    teacher.FontWeight = FontWeights.SemiBold;
                    Grid.SetRow(teacher, i * 4 + 2);
                    Grid.SetColumn(teacher, 8);
                    grid.Children.Add(teacher);

                    TextBlock classRoom = new TextBlock();
                    classRoom.Margin = new Thickness(30);
                    classRoom.Text = chetLessons[i].lesson.classRoom;
                    classRoom.TextAlignment = TextAlignment.Center;
                    classRoom.TextWrapping = TextWrapping.Wrap;
                    classRoom.FontSize = 28;
                    classRoom.FontWeight = FontWeights.SemiBold;
                    Grid.SetRow(classRoom, i * 4 + 2);
                    Grid.SetColumn(classRoom, 10);
                    grid.Children.Add(classRoom);
                }
            }
        }

        private int maxLessons(List<Lesson> lessons1, List<Lesson> lesson2) { return lessons1.Count > lesson2.Count ? lessons1.Count : lesson2.Count; }
    }
}
