using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Commander
{
    public class CommandManager : MonoBehaviour
    {
        [SerializeField] private bool _showDebugLogs = true;

        #region Lifecycle

        private void Awake()
        {
            CommandRegistry.ShowDebugLogs = _showDebugLogs;
            CommandRegistry.ClearCommands();
            RegisterAllSceneCommands();
        }

        private void OnDestroy()
        {
            CommandRegistry.ClearCommands();
        }

        private void OnApplicationQuit()
        {
            CommandRegistry.ClearCommands();
        }

        #endregion

        #region Command Registration

        private void RegisterAllSceneCommands()
        {
            // Find all types deriving MonoBehaviour that have at least one [ConsoleCommand]
            var commandTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(MonoBehaviour).IsAssignableFrom(t))
                .Where(t => t.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Any(mi => mi.GetCustomAttribute<Command>() != null))
                .ToList();

            // For each such type, grab all live instances in scene
            foreach (var type in commandTypes)
            {
                // Use reflection to call FindObjectsByType<T>(FindObjectsSortMode) with the correct type argument
                var findObjectsMethod = typeof(UnityEngine.Object)
                    .GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .FirstOrDefault(m => m.Name == "FindObjectsByType" && m.IsGenericMethod && m.GetParameters().Length == 1);

                if (findObjectsMethod != null)
                {
                    // Create a generic method with the current type
                    var genericMethod = findObjectsMethod.MakeGenericMethod(type);

                    // Use FindObjectsSortMode.None to match default behavior
                    var objects = genericMethod.Invoke(null, new object[] { FindObjectsSortMode.None }) as UnityEngine.Object[];
                    foreach (var inst in objects.Cast<MonoBehaviour>())
                    {
                        CommandRegistry.RegisterCommand(inst);
                    }
                }
            }
        }

        #endregion
    }
}