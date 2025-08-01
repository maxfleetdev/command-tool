using System;
using System.Globalization;
using System.Linq;

public static class CommandParser
{
    public static void StringToCommand(string rawInput, out string command, out string[] args)
    {
        // TODO: Keep in single string when surrounded in quotes
        var parts = rawInput.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0)
        {
            command = "";
            args = Array.Empty<string>();
            return;
        }

        // Return variables
        command = parts[0];
        args = parts.Length > 1? parts.Skip(1).ToArray() : Array.Empty<string>();
    }

    // Convert string into it's given type. E.g "1" = int, "STATE_IDLE" = enum, etc...
    public static object ConvertStringToType(string raw, Type targetType)
    {
        // String Conversion
        if (targetType == typeof(string))
        {
            return raw;
        }

        // Enum Conversion
        if (targetType.IsEnum)
        {
            return Enum.Parse(targetType, raw, ignoreCase: true);
        }

        // IConvertible Primitive Converison
        if (typeof(IConvertible).IsAssignableFrom(targetType))
        {
            return Convert.ChangeType(raw, targetType, CultureInfo.InvariantCulture);
        }

        throw new InvalidOperationException($"No converter for type {targetType.Name}");
    }
}