using System;
using System.Diagnostics.CodeAnalysis;

namespace ITTV.WPF.DataModel.Models
{
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed.")]
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "File is from Windows Store template")]
    public class Video : DataPageBase
    {
        public Video(string uniqueId, string title, Type navigationPage, string[] param)
            : base(uniqueId, title, navigationPage, param) { }
    }
}
