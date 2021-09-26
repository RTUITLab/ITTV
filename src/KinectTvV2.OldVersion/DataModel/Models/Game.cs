﻿using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Samples.Kinect.ControlsBasics.DataModel.Models
{
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed.")]
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "File is from Windows Store template")]
    public class Game : DataExecuteBase
    {
        public Game(string uniqueId, string title, string[] param) : base(uniqueId, title, param) { }
    }
}
