using UnityEngine;

namespace Commander.Settings
{
    [CreateAssetMenu(fileName = "New Command Settings", menuName = "Commander/Settings", order = 1)]
    public class CommandSettings : ScriptableObject
    {
        [Tooltip("Type of logs to display")]
        [SerializeField] private CommandLogType _logType;
        public CommandLogType LogType
        {
            get { return _logType; }
        }
    }
}