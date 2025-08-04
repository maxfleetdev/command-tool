using System;

namespace Commander
{
    /// <summary>
    /// Attribute to mark a method with a specific role.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CommandRoleAttribute : Attribute
    {
        public CommandRole Role { get; }

        /// <summary>
        /// Default constructor for CommandRole attribute.
        /// </summary>
        /// <param name="role">This command's execution permission role</param>
        public CommandRoleAttribute(CommandRole role)
        {
            Role = role;
        }
    }
}
