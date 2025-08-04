namespace Commander.Settings
{
    [System.Flags]
    public enum CommandLogType
    {
        INFO = 1,
        WARNING = 2,
        ERROR = 4,
        COMMAND_RESULT = 8
    }
}