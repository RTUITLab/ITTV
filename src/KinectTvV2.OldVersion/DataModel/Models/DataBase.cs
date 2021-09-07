using Microsoft.Samples.Kinect.ControlsBasics.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.ControlsBasics.DataModel.Models
{
    /// <summary>
    /// Base class for <see cref="SampleDataItem"/> and <see cref="SampleDataCollection"/> that
    /// defines properties common to both.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:FileHeaderFileNameDocumentationMustMatchTypeName", Justification = "Reviewed.")]
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "File is from Windows Store template")]
    public abstract class DataBase : BindableBase
    {
        public enum TaskType { Page, ChangeGroup, Execute };

        /// <summary>
        /// Field to store uniqueId
        /// </summary>
        private string uniqueId = string.Empty;

        /// <summary>
        /// Field to store title
        /// </summary>
        private string title = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="SampleDataCommon" /> class.
        /// </summary>
        /// <param name="uniqueId">The unique id of this item.</param>
        /// <param name="title">The title of this item.</param>
        /// <param name="description">A description of this item.</param>
        protected DataBase(string uniqueId, string title)
        {
            this.uniqueId = uniqueId;
            this.title = title;
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
    }
}
