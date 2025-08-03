using System;
using Commander.Core;
using UnityEngine;

namespace Commander
{
    public static class CommandExecutor
    {
        public static void ExecuteCommand(string userInput)
        {
            // Split the users input into its command name and it's arguments
            CommandParser.StringToCommand(userInput, out string command, out string[] args);
            var parameters = CommandRegistry.GetParameters(command);

            // Ensure enough parameters from input is present
            if (parameters.Length != args.Length)
            {
                Debug.Log($"Command {command}: Not enough parameters passed");
                return;
            }

            // Convert the raw input into the methods required parameters
            var converted = new object[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                var t = parameters[i].ParameterType;
                try
                {
                    // Try converting the string into the parameter type
                    converted[i] = CommandParser.ConvertStringToType(args[i], t);
                }
                catch (Exception e)
                {
                    // Failed (probably because DataType is not defined)
                    Debug.LogWarning($"Command '{command}': Failed to convert \"{args[i]}\" to {t.Name}: {e.Message}");
                    return;
                }
            }

            // Try executing the command
            CommandRegistry.TryExecuteCommand(command, converted);
        }
    }
}