using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Commander
{
    public static class CommandRegistry
    {
        #region Properties

        /// <summary>
        /// Name of command and it's tuple of method info and target MonoBehaviour
        /// </summary>
        private static Dictionary<string, List<(MethodInfo mi, MonoBehaviour target)>> _commands
            = new Dictionary<string, List<(MethodInfo, MonoBehaviour)>>
            (StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Enable or disable debug logs
        /// </summary>
        public static bool ShowDebugLogs = true;

        #endregion

        #region Lifecycle

        /// <summary>
        /// Register given MonoBehaviour's methods as commands.
        /// Scans all methods in the MonoBehaviour for the Command attribute.
        /// </summary>
        /// <param name="target">The MonoBehaviour with the Command attribute</param>
        public static void RegisterCommand(MonoBehaviour target)
        {
            // Get all methods in the MonoBehaviour
            var methods = target.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            // Iterate through each MonoBehaviour methods
            foreach (var mi in methods)
            {
                // Check if the method has the Command attribute
                var attr = mi.GetCustomAttribute<Command>();
                if (attr == null)
                {
                    continue;
                }

                // Use the attribute's name if provided, otherwise use the method's name
                var name = attr.Name?.Trim() switch
                {
                    // Use provided name
                    string s when s.Length > 0 => s,
                    // Use method name
                    _ => mi.Name
                };

                // If the name is empty, skip this command
                if (!_commands.TryGetValue(name, out var list))
                {
                    list = new List<(MethodInfo, MonoBehaviour)>();
                    _commands[name] = list;
                }

                // Add the method info and target MonoBehaviour to the dictionary value
                list.Add((mi, target));
            }

            if (ShowDebugLogs)
            {
                // Log the registration of commands
                Debug.Log($"[CommandRegistry] Registered {target.GetType().Name} with {_commands.Count} methods.");
            }
        }

        public static void ClearCommands()
        {
            _commands.Clear();
            if (ShowDebugLogs)
            {
                Debug.Log("[CommandRegistry] Cleared all commands.");
            }
        }

        #endregion

        #region Command Execution

        public static void ExecuteCommand(string commandInput)
        {
            CommandParser.StringToCommand(commandInput, out string command, out string[] arguments);
            ExecuteCommand(command, arguments);
        }

        private static void ExecuteCommand(string commandName, string[] rawArgs)
        {
            if (!_commands.TryGetValue(commandName, out var entries))
            {
                Debug.LogWarning($"Command '{commandName}' not found.");
                return;
            }

            foreach (var (mi, target) in entries)
            {
                var paramInfos = mi.GetParameters();
                if (paramInfos.Length != rawArgs.Length)
                {
                    Debug.LogError($"Command '{commandName}' expects {paramInfos.Length} args, got {rawArgs.Length}");
                    continue;
                }

                var converted = new object[rawArgs.Length];
                bool conversionFailed = false;
                for (int i = 0; i < rawArgs.Length; i++)
                {
                    var t = paramInfos[i].ParameterType;
                    try
                    {
                        converted[i] = CommandParser.ConvertStringToType(rawArgs[i], t);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(
                          $"Command '{commandName}': failed to convert \"{rawArgs[i]}\" to {t.Name}: {e.Message}");
                        conversionFailed = true;
                        break;
                    }
                }

                if (conversionFailed)
                {
                    continue;
                }

                try
                {
                    mi.Invoke(target, converted);
                }
                catch (Exception ex)
                {
                    Debug.LogError(
                      $"Error executing '{commandName}': {ex.InnerException?.Message ?? ex.Message}");
                }
            }
        }
    }

    #endregion
}