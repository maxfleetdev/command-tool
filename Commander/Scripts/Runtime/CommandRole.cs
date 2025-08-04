namespace Commander
{
    public enum CommandRole
    {
        /// <summary>
        /// No specific role, can be executed by any user.
        /// </summary>
        Any,

        /// <summary>
        /// Role for regular users, typically with limited permissions.
        /// </summary>
        User,

        /// <summary>
        /// Role for users with elevated permissions, such as moderators.
        /// </summary>
        Admin
    }
}
