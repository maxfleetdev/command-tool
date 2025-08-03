using System;

namespace Commander
{
    /// <summary>
    /// Attribute to mark a method as an executable command.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CommandAttribute : Attribute
    {
        #region Properties

        public string Name { get; }
        public string Descrption { get; }
        public CommandRole Role { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor for Command attribute.
        /// </summary>
        public CommandAttribute()
        {
            Name = string.Empty;
            Descrption = string.Empty;
            Role = CommandRole.Any;
        }

        /// <summary>
        /// Constructor for Command attribute with the comamnd name.
        /// </summary>
        /// <param name="name">Command name called in command-line</param>
        public CommandAttribute(string name)
        {
            Name = name;
            Descrption = string.Empty;
            Role = CommandRole.Any;
        }

        /// <summary>
        /// Constructor for Command attribute with the command name and it's description.
        /// </summary>
        /// <param name="name">This command's name called in command-line</param>
        /// <param name="description">This command's description of what it does</param>
        public CommandAttribute(string name, string description)
        {
            Name = name;
            Descrption = description;
            Role = CommandRole.Any;
        }

        /// <summary>
        /// Constructor for Command attribute with the command name, description, and role.
        /// </summary>
        /// <param name="name">This command's name called in command-line</param>
        /// <param name="description">This command's description of what it does</param>
        /// <param name="role">This command's execution permission role</param>
        public CommandAttribute(string name, string description, CommandRole role)
        {
            Name = name;
            Descrption = description;
            Role = role;
        }

        /// <summary>
        /// Constructor for Command attribute with the command name and role.
        /// </summary>
        /// <param name="name">This command's name called in command-line</param>
        /// <param name="role">This command's execution permission role</param>
        public CommandAttribute(string name, CommandRole role)
        {
            Name = name;
            Role = CommandRole.Any;
            Descrption = string.Empty;
        }

        /// <summary>
        /// Constructor for Command attribute with the command role only.
        /// </summary>
        /// <param name="role">This command's execution permission role</param>
        public CommandAttribute(CommandRole role)
        {
            Name = string.Empty;
            Role = CommandRole.Any;
            Descrption = string.Empty;
        }

        #endregion
    }
}