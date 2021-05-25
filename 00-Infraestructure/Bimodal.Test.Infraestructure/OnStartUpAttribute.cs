using System;

namespace Bimodal.Test.Infraestructure
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class OnStartUpAttribute : Attribute
    {
        public string Module { get; set; }
        public int Order { get; set; }
    }
}
