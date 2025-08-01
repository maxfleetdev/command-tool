/// <summary>
/// Attribute to mark public and private methods as commands that can be executed.
/// This attribute can be used to define commands with a specific name and description.
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = false)]
public class Command : System.Attribute
{
    #region Properties

    /// <summary>
    /// Name of the command to call from console
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Description of the commands action
    /// </summary>
    public string Description { get; private set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Empty command with default values - command name will be the methods name
    /// </summary>
    public Command()
    {
        Name = null;
        Description = null;
    }

    /// <summary>
    /// Define a command with a name and empty description.
    /// </summary>
    /// <param name="name">Name of the command when calling</param>
    public Command(string name)
    {
        Name = name;
        Description = null;
    }

    /// <summary>
    /// Define a command with a name and description.
    /// </summary>
    /// <param name="name">Name of the command when calling</param>
    /// <param name="description">Description of the command for help</param>
    public Command(string name, string description)
    {
        Name = name;
        Description = description;
    }

    #endregion
}