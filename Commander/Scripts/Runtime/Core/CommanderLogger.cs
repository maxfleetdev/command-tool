using UnityEngine;
using Commander.Settings;
using UnityEditor;

namespace Commander.Core
{
    public static class CommanderLogger
    {
        private static CommandSettings _cmdSettings;

        #region Initailise Settings

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void GetCommandSettings()
        {
            // Note: Use special folder to reduce scanning size?
            string[] assetGuids = AssetDatabase.FindAssets("t:CommandSettings", null);

            // None found so create one
            if (assetGuids.Length == 0)
            {
                CreateAsset();
                return;
            }

            // Find path of first result
            string path = AssetDatabase.GUIDToAssetPath(assetGuids[0]);
            _cmdSettings = AssetDatabase.LoadAssetAtPath<CommandSettings>(path);
            if (_cmdSettings == null)
            {
                CreateAsset();
                return;
            }
        }

        private static void CreateAsset()
        {
            _cmdSettings = (CommandSettings)ScriptableObject.CreateInstance(typeof(CommandSettings));
            AssetDatabase.CreateAsset(_cmdSettings, "Assets/settings.asset");
            Debug.Log("No CommandSettings asset found, creating new at Assets/");
        }

        #endregion

        #region Logging

        public static void LogMessage(string msg, CommandLogType type)
        {
            if (_cmdSettings == null || (_cmdSettings.LogType & type) != 0)
            {
                SendToConsole(msg, type);
            }
        }

        private static void SendToConsole(string msg, CommandLogType type)
        {
            switch (type)
            {
                case CommandLogType.WARNING:
                    Debug.LogWarning(msg);
                    break;

                case CommandLogType.ERROR:
                    Debug.LogError(msg);
                    break;

                default:
                    Debug.Log(msg);
                    break;
            }
        }

        #endregion
    }
}