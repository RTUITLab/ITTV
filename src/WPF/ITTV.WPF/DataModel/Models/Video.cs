using System;
using System.Diagnostics.CodeAnalysis;

namespace ITTV.WPF.DataModel.Models
{ 
    public class Video : DataPageBase
    {
        public Video(string uniqueId, string title, Type navigationPage, string[] param)
            : base(uniqueId, title, navigationPage, param) { }
    }
}
