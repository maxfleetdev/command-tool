using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Commander.Core
{
    public static class CommandRegistry
    {
        #region Properties

        // key = unqiue command name
        // value = CommandData
        private static Dictionary<string, CommandData> _registeredCommands =
            new Dictionary<string, CommandData>();

        /// <summary>
        /// All command name keys and their CommandData value
        /// </summary>
        public static Dictionary<string, CommandData> RegisteredCommands
        {
            get { return _registeredCommands; }
        }

        #endregion

        #region Lifecycle

        /// <summary>
        /// Clears all commands within the Registry
        /// </summary>
        public static void ClearCommands()
        {
            _registeredCommands.Clear();
        }

        /// <summary>
        /// Called before the first scene loads. Registers all application static commands
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void AutoRegister()
        {
            ClearCommands();
            RegisterStaticCommands();
        }

        #endregion

        #region Command Registering

        /// <summary>
        /// Registers a MonoBehaviour class which includes the Command Attribute. 
        /// </summary>
        /// <param name="targetClass">Class that contains Command Methods</param>
        public static void RegisterInstanceCommands(object targetClass = null)
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
                string cmdName = cmdAttribute.Name?.ToLower().Trim() switch
                {
                    string s when s.Length > 0 => s,
                    _ => methodInfo.Name
                };

                if (_registeredCommands.ContainsKey(cmdName))
                {
                    Debug.Log($"[CommandRegistry] Comamnd already registered with name '{cmdName}'");
                    continue;
                }

                // Assign value of dictionary as new CommandData
                _registeredCommands[cmdName] = new CommandData(cmdName, methodInfo, targetClass);
                Debug.Log($"[CommandRegistry] Registered Command '{cmdName}'");
            }
        }

        /// <summary>
        /// Registers all static classes found within the App Domain. Called at initialisation
        /// </summary>
        public static void RegisterStaticCommands()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    var methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                    foreach (var method in methods)
                    {
                        var attr = method.GetCustomAttribute<CommandAttribute>();
                        if (attr == null)
                        {
                            continue;
                        }
                        string cmdName = string.IsNullOrWhiteSpace(attr.Name) ? method.Name : attr.Name;
                        _registeredCommands[cmdName] = new CommandData(cmdName, method, null);
                        Debug.Log($"[CommandRegistry] Registered Static Command '{cmdName}'");
                    }
                }
            }
        }

        #endregion

        #region Command Execution

        public static bool TryExecuteCommand(string commandName, object[] args)
        {
            // Ensures no character-case mistakes occur
            commandName = commandName.ToLower();

            // Check if command even exists
            if (!_registeredCommands.TryGetValue(commandName, out var data))
            {
                Debug.Log($"[CommandRegistry] No command with name '{commandName}'");
                return false;
            }
            
            // Try executing command from CommandData
            return data.Execute(args);
        }

        #endregion
    }
}