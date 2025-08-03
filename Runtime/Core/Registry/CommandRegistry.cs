using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Commander.Core
{
    public static class CommandRegistry
    {
        private static Dictionary<string, CommandData> _registeredCommands =
            new Dictionary<string, CommandData>();

        public static Dictionary<string, CommandData> RegisteredCommands
        {
            get { return _registeredCommands; }
        }

        public static void ClearCommands()
        {
            _registeredCommands.Clear();
        }

        public static void RegisterCommands(object targetClass = null)
        {
            var targetMethods = targetClass.GetType().GetMethods(
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.NonPublic
            );

            foreach (var methodInfo in targetMethods)
            {
                // Check if method has attribute
                var cmdAttribute = methodInfo.GetCustomAttribute<CommandAttribute>();
                if (cmdAttribute == null)
                {
                    continue;
                }

                // Use method name if commandname not present
                string cmdName = cmdAttribute.Name?.Trim() switch
                {
                    string s when s.Length > 0 => s,
                    _ => methodInfo.Name
                };

                // Assign value of dictionary as new CommandData
                _registeredCommands[cmdName] = new CommandData(cmdName, methodInfo, targetClass);
                Debug.Log($"[CommandRegistry] Registered Command {cmdName}");
            }
            Debug.Log($"[CommandRegistry] Registered Class {targetClass.GetType().Name} with {_registeredCommands.Count} methods");
        }

        public static bool TryExecuteCommand(string commandName, object[] args)
        {
            // Check if command even exists
            if (!_registeredCommands.TryGetValue(commandName, out var data))
            {
                Debug.Log($"No command with name {commandName}");
                return false;
            }

            // Try executing command from CommandData
            return data.Execute(args);
        }
    }
}