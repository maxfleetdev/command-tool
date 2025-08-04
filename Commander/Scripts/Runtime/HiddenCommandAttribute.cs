using System;

namespace Commander
{
    /// <summary>
    /// Attribute to hide a command from the command list.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class HiddenCommandAttribute : Attribute
    {
        // This attribute does not require any properties or methods.
        // It serves as a marker to indicate that the command should not be 
        // displayed in the command list
    }
}
