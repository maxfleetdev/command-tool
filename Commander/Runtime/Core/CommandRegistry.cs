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
        /// Enable or disable debug logs
        /// </summary>
        public static bool ShowDebugLogs = true;

        /// <summary>
        /// Name of command and it's tuple of method info and target MonoBehaviour
        /// </summary>
        private static Dictionary<string, List<(MethodInfo mi, MonoBehaviour target)>> _commands
            = new Dictionary<string, List<(MethodInfo, MonoBehaviour)>>
            (StringComparer.OrdinalIgnoreCase);

        #endregion

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
                ExecuteCommand("TestCommand", 1, "Hello World");
            }
        }

        public static void ExecuteCommand(string commandName, params object[] parameters)
        {
            if (_commands.TryGetValue(commandName, out var list))
            {
                foreach (var (mi, target) in list)
                {
                    try
                    {
                        // Invoke the method with the provided parameters
                        mi.Invoke(target, parameters);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Error executing command '{commandName}': {ex.Message}");
                    }
                }
            }
            else
            {
                Debug.LogWarning($"Command '{commandName}' not found.");
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
    }
}