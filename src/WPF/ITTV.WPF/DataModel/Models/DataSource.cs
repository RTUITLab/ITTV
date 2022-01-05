using System.Collections.ObjectModel;
using System.Linq;
using ITTV.WPF.Interface.Pages;

namespace ITTV.WPF.DataModel.Models
{
    public sealed class DataSource
    {
        private static readonly DataSource Source = new DataSource();

        public DataSource()
        {
            const string uniqueId = "Menu";
            var groupMain = new DataCollection<object>(
                uniqueId,
                uniqueId,
                DataCollection<object>.GroupType.Menu
                );
            groupMain.Items.Add(
                new DataGroupBase(
                    $"{uniqueId}-1",
                    "Расписание"
                    ));
            groupMain.Items.Add(
                new DataPageBase(
                    $"{uniqueId}-2",
                    "Новости",
                    typeof(NewsList),
                    StringToArr()
                    ));
            groupMain.Items.Add(
                new DataPageBase(
                    $"{uniqueId}-3",
                    "Видео",
                    typeof(VideoList),
                    StringToArr()
                    ));
            groupMain.Items.Add(
                new DataGroupBase(
                    $"{uniqueId}-4",
                    "Игры"
                    ));

            AllGroups.Add(groupMain);
        }

        public ObservableCollection<DataCollection<object>> AllGroups { get; } = new ObservableCollection<DataCollection<object>>();

        public void AddToGroups(DataCollection<object> group)
        {
            if (GetGroup(group.UniqueId) != null)
            {
                Source.AllGroups.Remove(GetGroup(group.UniqueId));
            }
            Source.AllGroups.Add(group);
        }

        public static DataCollection<object> GetGroup(string uniqueId)
        {
            var matches = Source.AllGroups.Where(x =>
                    x.UniqueId.Equals(uniqueId))
                .ToArray();
            return matches.FirstOrDefault();
        }

        public static string[] StringToArr(params string[] param)
        {
            return param;
        }
    }
}
