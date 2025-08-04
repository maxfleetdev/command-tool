using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Commander.Core;
using Commander.Settings;

namespace Commander
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
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
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
                    CommanderLogger.LogMessage($"[CommandRegistry] Comamnd already registered with name '{cmdName}'",
                        CommandLogType.WARNING);
                    continue;
                }

                // Assign value of dictionary as new CommandData
                _registeredCommands[cmdName] = new CommandData(
                    cmdName,
                    cmdAttribute.Descrption,
                    methodInfo,
                    targetClass
                    // add flags here
                );

                CommanderLogger.LogMessage($"[CommandRegistry] Registered Command '{cmdName}'",
                    CommandLogType.INFO);
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
                        _registeredCommands[cmdName] = new CommandData(cmdName, attr.Descrption, method, null);
                        CommanderLogger.LogMessage($"[CommandRegistry] Registered Static Command '{cmdName}'",
                            CommandLogType.INFO);
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
                CommanderLogger.LogMessage($"[CommandRegistry] No command with name '{commandName}'",
                    CommandLogType.INFO);
                return false;
            }

            // Try executing command from CommandData
            return data.Execute(args);
        }

        /// <summary>
        /// Gets the parameters of a given command and it's linked method
        /// </summary>
        /// <param name="commandName">The command name</param>
        /// <returns>ParameterInfo array</returns>
        public static ParameterInfo[] GetParameters(string commandName)
        {
            if (_registeredCommands.TryGetValue(commandName, out var data))
            {
                return data.GetParameters();
            }
            return null;
        }

        #endregion

        [Command("help", "Shows all commands and their functions")]
        public static void ShowAllCommands()
        {
            foreach (KeyValuePair<string, CommandData> kvp in _registeredCommands)
            {
                if (kvp.Value.Flags == CommandFlags.Hidden)
                {
                    continue;
                }
                Debug.Log(kvp.Key + ": " + kvp.Value.CommandDesc);
            }
        }
    }
}