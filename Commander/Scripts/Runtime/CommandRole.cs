namespace Commander
{
    [System.Flags]
    public enum CommandRole
    {
        /// <summary>
        /// No specific role, can be executed by any user.
        /// </summary>
        Any = 0,

        /// <summary>
        /// Role for regular users, typically with limited permissions.
        /// </summary>
        User = 1,

        /// <summary>
        /// Role for users with elevated permissions, such as moderators.
        /// </summary>
        Admin = 2,

        /// <summary>
        /// Role for developers to use only
        /// </summary>
        Developer = 4
    }
}
