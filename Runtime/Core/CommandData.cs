using System;
using System.Reflection;
using UnityEngine;

namespace Commander.Core
{
    public struct CommandData
    {
        public string CommandName { get; }
        public MethodInfo MethodData { get; }
        public object TargetClass { get; }
        public CommandFlags Flags { get; }  // Set roles and flags?

        public CommandData(string commandName, MethodInfo methodData,
            object targetClass, CommandFlags flags = CommandFlags.None)
        {
            CommandName = commandName;
            MethodData = methodData;
            TargetClass = targetClass;
            Flags = flags;
        }

        public bool Execute(object[] args)
        {
            try
            {
                MethodData.Invoke(TargetClass, args);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Could not execute {CommandName}: " + e);
                return false;
            }
        }
    }
}