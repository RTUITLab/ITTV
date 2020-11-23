//------------------------------------------------------------------------------
// <copyright file="SampleDataSource.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.Samples.Kinect.ControlsBasics.DataModel
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Microsoft.Samples.Kinect.ControlsBasics.Common;
    using System.Globalization;
    using Microsoft.Samples.Kinect.ControlsBasics.Pages;

    // The data model defined by this file serves as a representative example of a strongly-typed
    // model that supports notification when members are added, removed, or modified.  The property
    // names chosen coincide with data bindings in the standard item templates.
    // Applications may use this model as a starting point and build on it, or discard it entirely and
    // replace it with something appropriate to their needs.

    /// <summary>
    /// Creates a collection of groups and items with hard-coded content.
    /// SampleDataSource initializes with placeholder data rather than live production
    /// data so that sample data is provided at both design-time and run-time.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "File is from Windows Store template")]
    public sealed class SampleDataSource
    {
        private static SampleDataSource sampleDataSource = new SampleDataSource();

        private ObservableCollection<SampleDataCollection> allGroups = new ObservableCollection<SampleDataCollection>();

        private static Uri darkGrayImage = new Uri("Assets/DarkGray.png", UriKind.Relative);
        private static Uri mediumGrayImage = new Uri("assets/mediumGray.png", UriKind.Relative);
        private static Uri lightGrayImage = new Uri("assets/lightGray.png", UriKind.Relative);

        public SampleDataSource()
        {
            var group_main = new SampleDataCollection(
                "Menu",
                "Menu",
                SampleDataCollection.GroupType.Menu
                );
            group_main.Items.Add(
                new SampleDataItem(
                    "Menu-1",
                    "Расписание",
                    SampleDataItem.TaskType.ChangeGroup,
                    "Courses"
                    ));
            group_main.Items.Add(
                new SampleDataItem(
                    "Menu-2",
                    "Новости",
                    SampleDataItem.TaskType.Page,
                    typeof(NewsList),
                    StringToArr()
                    ));
            group_main.Items.Add(
                new SampleDataItem(
                    "Menu-3",
                    "Видео",
                    SampleDataItem.TaskType.Page,
                    typeof(VideoList),
                    StringToArr()
                    ));
            group_main.Items.Add(
                new SampleDataItem(
                    "Menu-4",
                    "Игры",
                    SampleDataItem.TaskType.ChangeGroup,
                    "Games"
                    ));

            this.AllGroups.Add(group_main);


            //var group_course = new SampleDataCollection(
            //    "Courses",
            //    "Курсы",
            //    SampleDataCollection.GroupType.Courses);

            //group_course.Items.Add(
            //    new SampleDataItem(
            //        "Courses-1",
            //        "Первый курс бакалавриата",
            //        SampleDataItem.TaskType.Page,
            //        typeof(ScrollViewerSample),
            //        StringToArr("/Images/TimeTables/1.jpg")));
            //group_course.Items.Add(
            //    new SampleDataItem(
            //        "Courses-2",
            //        "Второй курс бакалавриата",
            //        SampleDataItem.TaskType.Page,
            //        typeof(ScrollViewerSample),
            //        StringToArr("/Images/TimeTables/2.jpg")));
            //group_course.Items.Add(
            //    new SampleDataItem(
            //        "Courses-3",
            //        "Третий курс бакалавриата",
            //        SampleDataItem.TaskType.Page,
            //        typeof(ScrollViewerSample),
            //        StringToArr("/Images/TimeTables/3.jpg")));
            //group_course.Items.Add(
            //    new SampleDataItem(
            //        "Courses-4",
            //        "Четвертый курс бакалавриата",
            //        SampleDataItem.TaskType.Page,
            //        typeof(ScrollViewerSample),
            //        StringToArr("/Images/TimeTables/4.jpg")));
            //group_course.Items.Add(
            //    new SampleDataItem(
            //        "Courses-5",
            //        "Первый курс магистратуры",
            //        SampleDataItem.TaskType.Page,
            //        typeof(ScrollViewerSample),
            //        StringToArr("/Images/TimeTables/5.jpg")));
            //group_course.Items.Add(
            //    new SampleDataItem(
            //        "Courses-6",
            //        "Второй курс магистратуры",
            //        SampleDataItem.TaskType.Page,
            //        typeof(ScrollViewerSample),
            //        StringToArr("/Images/TimeTables/6.jpg")));

            //this.AllGroups.Add(group_course);

        }

        public ObservableCollection<SampleDataCollection> AllGroups
        {
            get { return this.allGroups; }
        }

        public static void AddToGroups(SampleDataCollection group)
        {
            if (SampleDataSource.GetGroup(group.UniqueId) != null)
            {
                sampleDataSource.AllGroups.Remove(SampleDataSource.GetGroup(group.UniqueId));
            }
            sampleDataSource.AllGroups.Add(group);
        }

        public static SampleDataCollection GetGroup(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = sampleDataSource.AllGroups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1)
            {
                return matches.First();
            }

            return null;
        }

        public static SampleDataItem GetItem(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = sampleDataSource.AllGroups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1)
            {
                return matches.First();
            }

            return null;
        }

        public static string[] StringToArr(params string[] param)
        {
            return param;
        }
    }

    /// <summary>
    /// Base class for <see cref="SampleDataItem"/> and <see cref="SampleDataCollection"/> that
    /// defines properties common to both.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:FileHeaderFileNameDocumentationMustMatchTypeName", Justification = "Reviewed.")]
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "File is from Windows Store template")]
    public abstract class SampleDataCommon : BindableBase
    {
        /// <summary>
        /// Field to store uniqueId
        /// </summary>
        private string uniqueId = string.Empty;

        /// <summary>
        /// Field to store title
        /// </summary>
        private string title = string.Empty;

        /// <summary>
        /// Field to store description
        /// </summary>
        private string description = string.Empty;

        private ImageSource source = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SampleDataCommon" /> class.
        /// </summary>
        /// <param name="uniqueId">The unique id of this item.</param>
        /// <param name="title">The title of this item.</param>
        /// <param name="description">A description of this item.</param>
        protected SampleDataCommon(string uniqueId, string title, string description, string source)
        {
            this.uniqueId = uniqueId;
            this.title = title;
            this.description = description;
            if (source != "")
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(source);
                image.EndInit();
                this.source = image;
            }
        }

        /// <summary>
        /// Gets or sets UniqueId.
        /// </summary>
        public string UniqueId
        {
            get { return this.uniqueId; }
            set { this.SetProperty(ref this.uniqueId, value); }
        }

        public string Title
        {
            get { return this.title; }
            set { this.SetProperty(ref this.title, value); }
        }

        public string Description
        {
            get { return this.description; }
            set { this.SetProperty(ref this.description, value); }
        }

        public override string ToString()
        {
            return this.Title;
        }

        public ImageSource Source
        {
            get { return this.source; }
        }
    }

    /// <summary>
    /// Generic item data model.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed.")]
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "File is from Windows Store template")]
    public class SampleDataItem : SampleDataCommon
    {
        private Type navigationPage;
        private TaskType task;
        private string newgroup;
        private object[] parametrs;
        private string content;

        public enum TaskType { Page, ChangeGroup, Execute };

        public SampleDataItem(string uniqueId, string subgroup_title, TaskType task, string new_group)
            : base(uniqueId, subgroup_title, string.Empty, string.Empty)
        {
            this.task = task;
            this.newgroup = new_group;
            this.navigationPage = null;
            this.content = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SampleDataItem" /> class.
        /// </summary>
        /// <param name="uniqueId">The unique id of this item.</param>
        /// <param name="title">The title of this item.</param>
        /// <param name="description">A description of this item.</param>
        /// <param name="content">The content of this item.</param>
        /// <param name="group">The group of this item.</param>
        /// <param name="navigationPage">What page should launch when clicking this item.</param>
        public SampleDataItem(string uniqueId, string page_title, TaskType task, Type navigationPage, string[] param)
            : base(uniqueId, page_title, string.Empty, string.Empty)
        {
            this.task = task;
            this.navigationPage = navigationPage;
            this.newgroup = string.Empty;
            this.parametrs = param;
            this.content = string.Empty;
        }

        public SampleDataItem(string uniqueId, string news_title, string source, TaskType task, Type navigationPage, string content)
            : base(uniqueId, news_title, string.Empty, source)
        {
            this.task = task;
            this.navigationPage = navigationPage;
            this.content = content;
            this.parametrs = SampleDataSource.StringToArr(content);
        }

        public SampleDataItem(string uniqueId, string video_title, string source, TaskType task, Type navigationPage, string[] param)
            : base(uniqueId, video_title, string.Empty, source)
        {
            this.task = task;
            this.navigationPage = navigationPage;
            this.parametrs = param;
        }

        public SampleDataItem(string uniqueId, string game_title, TaskType task, string[] param)
            : base(uniqueId, game_title, string.Empty, string.Empty)
        {
            this.task = task;
            this.parametrs = param;
        }

        public Type NavigationPage
        {
            get { return this.navigationPage; }
            set { this.SetProperty(ref this.navigationPage, value); }
        }

        public TaskType Task
        {
            get { return this.task; }
        }

        public string NewGroup
        {
            get { return this.newgroup; }
        }

        public object[] Parametrs
        {
            get { return this.parametrs; }
        }

        public string Content
        {
            get { return this.content; }
        }
    }

    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class SampleDataCollection : SampleDataCommon, IEnumerable
    {
        private ObservableCollection<SampleDataItem> items = new ObservableCollection<SampleDataItem>();
        private ObservableCollection<SampleDataItem> topItem = new ObservableCollection<SampleDataItem>();
        private GroupType typeGroup;

        public enum GroupType { Menu, News, Courses, Video };

        public SampleDataCollection(string uniqueId, string title, GroupType groupType)
            : base(uniqueId, title, string.Empty, string.Empty)
        {
            this.typeGroup = groupType;
            this.Items.CollectionChanged += this.ItemsCollectionChanged;
        }

        public ObservableCollection<SampleDataItem> Items
        {
            get { return this.items; }
        }

        public ObservableCollection<SampleDataItem> TopItems
        {
            get { return this.topItem; }
        }

        public IEnumerator GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }

        public GroupType TypeGroup
        {
            get { return typeGroup; }
        }

        private void ItemsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Provides a subset of the full items collection to bind to from a GroupedItemsPage
            // for two reasons: GridView will not virtualize large items collections, and it
            // improves the user experience when browsing through groups with large numbers of
            // items.
            // A maximum of 12 items are displayed because it results in filled grid columns
            // whether there are 1, 2, 3, 4, or 6 rows displayed
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewStartingIndex < 12)
                    {
                        this.TopItems.Insert(e.NewStartingIndex, this.Items[e.NewStartingIndex]);
                        if (this.TopItems.Count > 12)
                        {
                            this.TopItems.RemoveAt(12);
                        }
                    }

                    break;
                case NotifyCollectionChangedAction.Move:
                    if (e.OldStartingIndex < 12 && e.NewStartingIndex < 12)
                    {
                        this.TopItems.Move(e.OldStartingIndex, e.NewStartingIndex);
                    }
                    else if (e.OldStartingIndex < 12)
                    {
                        this.TopItems.RemoveAt(e.OldStartingIndex);
                        this.TopItems.Add(Items[11]);
                    }
                    else if (e.NewStartingIndex < 12)
                    {
                        this.TopItems.Insert(e.NewStartingIndex, this.Items[e.NewStartingIndex]);
                        this.TopItems.RemoveAt(12);
                    }

                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldStartingIndex < 12)
                    {
                        this.TopItems.RemoveAt(e.OldStartingIndex);
                        if (this.Items.Count >= 12)
                        {
                            this.TopItems.Add(this.Items[11]);
                        }
                    }

                    break;
                case NotifyCollectionChangedAction.Replace:
                    if (e.OldStartingIndex < 12)
                    {
                        this.TopItems[e.OldStartingIndex] = this.Items[e.OldStartingIndex];
                    }

                    break;
                case NotifyCollectionChangedAction.Reset:
                    this.TopItems.Clear();
                    while (this.TopItems.Count < this.Items.Count && this.TopItems.Count < 12)
                    {
                        this.TopItems.Add(this.Items[this.TopItems.Count]);
                    }

                    break;
            }
        }
    }
}
