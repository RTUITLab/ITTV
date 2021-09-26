using System.Diagnostics.CodeAnalysis;
using Microsoft.Samples.Kinect.ControlsBasics.Interface.Common;

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
        
        protected DataBase(string uniqueId, string title)
        {
            UniqueId = uniqueId;
            Title = title;
        }
        public string UniqueId { get; }
        
        public string Title { get; }
    }
}
