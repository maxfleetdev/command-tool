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
            // Note 2: Also use async to ensure this is loaded before register
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

            Debug.Log("CommandSettings Loaded @ " + path);
        }

        private static void CreateAsset()
        {
            _cmdSettings = (CommandSettings)ScriptableObject.CreateInstance(typeof(CommandSettings));
            AssetDatabase.CreateAsset(_cmdSettings, "Assets/settings.asset");
            Debug.Log("No CommandSettings asset found, creating new at Assets/");
        }

        #endregion

        public static void LogMessage(string msg, CommandLogType type)
        {
            if (_cmdSettings == null)
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

            else if (_cmdSettings.LogType == type)
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
        }
    }
}