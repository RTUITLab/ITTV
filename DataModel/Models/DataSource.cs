using Microsoft.Samples.Kinect.ControlsBasics.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Samples.Kinect.ControlsBasics.DataModel.Models.DataBase;

namespace Microsoft.Samples.Kinect.ControlsBasics.DataModel.Models
{
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "File is from Windows Store template")]
    public sealed class DataSource : Singleton<DataSource>
    {
        private static DataSource dataSource = new DataSource();

        private ObservableCollection<DataCollection<object>> allGroups = new ObservableCollection<DataCollection<object>>();

        public DataSource()
        {
            var group_main = new DataCollection<object>(
                "Menu",
                "Menu",
                DataCollection<object>.GroupType.Menu
                );
            group_main.Items.Add(
                new DataGroupBase(
                    "Menu-1",
                    "Расписание",
                    "Courses"
                    ));
            group_main.Items.Add(
                new DataPageBase(
                    "Menu-2",
                    "Новости",
                    typeof(NewsList),
                    StringToArr()
                    ));
            group_main.Items.Add(
                new DataPageBase(
                    "Menu-3",
                    "Видео",
                    typeof(VideoList),
                    StringToArr()
                    ));
            group_main.Items.Add(
                new DataGroupBase(
                    "Menu-4",
                    "Игры",
                    "Games"
                    ));

            AllGroups.Add(group_main);
        }

        public ObservableCollection<DataCollection<object>> AllGroups
        {
            get { return allGroups as ObservableCollection<DataCollection<object>>; }
        }

        public void AddToGroups(DataCollection<object> group)
        {
            if (GetGroup(group.UniqueId) != null)
            {
                dataSource.AllGroups.Remove(GetGroup(group.UniqueId));
            }
            dataSource.AllGroups.Add(group);
        }

        public DataCollection<object> GetGroup(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = dataSource.AllGroups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1)
            {
                return matches.First();
            }

            return null;
        }

        public object GetItem(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = dataSource.AllGroups.SelectMany(group => group.Items).Where((item) => ((DataBase) item).UniqueId.Equals(uniqueId));
            if (matches.Count() == 1)
            {
                return matches.First();
            }

            return null;
        }

        public string[] StringToArr(params string[] param)
        {
            return param;
        }
    }
}
