using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.ControlsBasics.DataModel.Models
{
    public class DataCollection <T> : DataBase, IEnumerable
    {
        public enum GroupType { Menu, News, Courses, Video, Games };

        public DataCollection(string uniqueId, string title, GroupType groupType) : base(uniqueId, title)
        {
            TypeGroup = groupType;
            Items.CollectionChanged += ItemsCollectionChanged;
        }

        public ObservableCollection<T> Items { get; } = new ObservableCollection<T>();

        private ObservableCollection<T> TopItems { get; } = new ObservableCollection<T>();

        public IEnumerator GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        public GroupType TypeGroup { get; }

        private void ItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewStartingIndex < 12)
                    {
                        TopItems.Insert(e.NewStartingIndex, Items[e.NewStartingIndex]);
                        if (TopItems.Count > 12)
                        {
                            TopItems.RemoveAt(12);
                        }
                    }
                    break;
                
                case NotifyCollectionChangedAction.Move:
                    switch (e.OldStartingIndex < 12)
                    {
                        case true when e.NewStartingIndex < 12:
                            TopItems.Move(e.OldStartingIndex, e.NewStartingIndex);
                            break;
                        
                        case true:
                            TopItems.RemoveAt(e.OldStartingIndex);
                            TopItems.Add(Items[11]);
                            break;
                        
                        default:
                        {
                            if (e.NewStartingIndex < 12)
                            {
                                TopItems.Insert(e.NewStartingIndex, Items[e.NewStartingIndex]);
                                TopItems.RemoveAt(12);
                            }
                            break;
                        }
                    }

                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldStartingIndex < 12)
                    {
                        TopItems.RemoveAt(e.OldStartingIndex);
                        if (Items.Count >= 12)
                        {
                            TopItems.Add(Items[11]);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    if (e.OldStartingIndex < 12)
                    {
                        TopItems[e.OldStartingIndex] = Items[e.OldStartingIndex];
                    }
                    break;
                
                case NotifyCollectionChangedAction.Reset:
                    TopItems.Clear();
                    while (TopItems.Count < Items.Count && TopItems.Count < 12)
                    {
                        TopItems.Add(Items[TopItems.Count]);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}