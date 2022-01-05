using ITTV.WPF.Interface.Common;

namespace ITTV.WPF.DataModel.Models
{
    public abstract class DataBase : BindableBase
    {
        public enum TaskType { Page, ChangeGroup, Execute };
        
        protected DataBase(string uniqueId, string title)
        {
            UniqueId = uniqueId;
            Title = title;
        }
        public string UniqueId { get; }
        
        public string Title { get; }
    }
}
