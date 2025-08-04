using System;
using System.Reflection;
using UnityEngine;

namespace Commander.Core
{
    public struct CommandData
    {
        #region Properties

        public string CommandName { get; }
        public string CommandDesc { get; }
        public MethodInfo MethodData { get; }
        public object TargetClass { get; }
        public CommandFlags Flags { get; }  // Set roles and flags?

        #endregion

        #region Constructors

        public CommandData(string commandName, string commandDesc, MethodInfo methodData,
            object targetClass, CommandFlags flags = CommandFlags.None)
        {
            CommandName = commandName;
            CommandDesc = commandDesc;
            MethodData = methodData;
            TargetClass = targetClass;
            Flags = flags;
        }

        #endregion

        #region Command Logic

        /// <summary>
        /// Retrieves the parameters assigned to this command
        /// </summary>
        /// <returns></returns>
        public ParameterInfo[] GetParameters()
        {
            return MethodData.GetParameters();
        }

        /// <summary>
        /// Tries to execute a method with given parameters
        /// </summary>
        /// <param name="args">Parameters of the method</param>
        /// <returns>Execution was successful</returns>
        public bool Execute(object[] args)
        {
            try
            {
                MethodData.Invoke(TargetClass, args);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[CommandData] Could not execute '{CommandName}': " + e);
                return false;
            }
        }

        #endregion
    }
}