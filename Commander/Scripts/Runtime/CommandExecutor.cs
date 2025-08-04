using System;
using Commander.Core;
using Commander.Settings;

namespace Commander
{
    public static class CommandExecutor
    {
        public static void ExecuteCommand(string userInput)
        {
            // Split the users input into its command name and it's arguments
            CommandParser.StringToCommand(userInput, out string command, out string[] args);
            var parameters = CommandRegistry.GetParameters(command);

            // Check if command has no parameter requirements
            if (parameters == null)
            {
                CommandRegistry.TryExecuteCommand(command, null);
                return;
            }

            // Otherwise, Ensure enough parameters from input is present
            else if (parameters.Length != args.Length)
            {
                CommanderLogger.LogMessage($"Command {command}: Not enough parameters passed",
                    CommandLogType.INFO);
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
                    // Conversion Failure (DataType is not defined)
                    CommanderLogger.LogMessage($"Command '{command}': Failed to convert \"{args[i]}\" to {t.Name}: {e.Message}",
                        CommandLogType.ERROR);
                    return;
                }
            }

            // Try executing the command from CommandData struct
            CommandRegistry.TryExecuteCommand(command, converted);
        }
    }
}