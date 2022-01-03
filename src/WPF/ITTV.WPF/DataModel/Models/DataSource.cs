using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ITTV.WPF.Interface.Pages;

namespace ITTV.WPF.DataModel.Models
{
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "File is from Windows Store template")]
    public sealed class DataSource : Singleton<DataSource>
    {
        private static readonly DataSource Source = new DataSource();

        public DataSource()
        {
            var groupMain = new DataCollection<object>(
                "Menu",
                "Menu",
                DataCollection<object>.GroupType.Menu
                );
            groupMain.Items.Add(
                new DataGroupBase(
                    "Menu-1",
                    "Расписание"
                    ));
            groupMain.Items.Add(
                new DataPageBase(
                    "Menu-2",
                    "Новости",
                    typeof(NewsList),
                    StringToArr()
                    ));
            groupMain.Items.Add(
                new DataPageBase(
                    "Menu-3",
                    "Видео",
                    typeof(VideoList),
                    StringToArr()
                    ));
            groupMain.Items.Add(
                new DataGroupBase(
                    "Menu-4",
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
            return matches.Length == 1 ? matches.First() : null;
        }

        public static string[] StringToArr(params string[] param)
        {
            return param;
        }
    }
}
