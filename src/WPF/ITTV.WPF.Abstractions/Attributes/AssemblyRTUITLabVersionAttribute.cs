using System;

namespace ITTV.WPF.Abstractions.Attributes
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class AssemblyRTUITLabVersionAttribute : Attribute
    {
        public AssemblyRTUITLabVersionAttribute(string version = null)
        {
            Version = version ?? throw new ArgumentNullException(nameof(version));
        }

        public string Version { get; }
    }
}