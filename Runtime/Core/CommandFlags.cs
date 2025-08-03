namespace Commander.Core
{
    [System.Flags]
    public enum CommandFlags
    {
        None = 0,
        Hidden = 1,
        RequiresRole = 2
        // more command flags here...
    }
}