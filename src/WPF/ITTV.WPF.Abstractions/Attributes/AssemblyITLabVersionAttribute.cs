using System;

namespace ITTV.WPF.Abstractions.Attributes
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class AssemblyITLabVersionAttribute : Attribute
    {
        public AssemblyITLabVersionAttribute(string version = null)
        {
            Version = version ?? throw new ArgumentNullException(nameof(version));
        }

        public string Version { get; }
    }
}