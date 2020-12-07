using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Microsoft.Samples.Kinect.ControlsBasics.DataModel.Models
{
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed.")]
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "File is from Windows Store template")]
    public class Video : DataPageBase
    {
        private string source;

        public Video(string uniqueId, string title, Type navigationPage, string[] param, string source) : base(uniqueId, title, navigationPage, param)
        {
            this.source = source;
        }

        public Uri Source
        {
            get
            {
                return new Uri(source, UriKind.Relative);
            }
        }
    }
}
