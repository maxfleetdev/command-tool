using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Commander
{
    public static class CommandParser
    {
        private static readonly Regex _tokeniser = new Regex(
            @"[\""].+?[\""]|['].+?[']|[^ ]+",
            RegexOptions.Compiled);

        /// <summary>
        /// Takes the raw input of the user and returns the command name and it's converted parameters
        /// </summary>
        /// <param name="rawInput">Input sent by user</param>
        /// <param name="command">The commands name</param>
        /// <param name="args">The command parameters</param>
        public static void StringToCommand(string rawInput, out string command, out string[] args)
        {
            // Skip empty input
            if (string.IsNullOrWhiteSpace(rawInput))
            {
                command = "";
                args = Array.Empty<string>();
                return;
            }

            // Keep strings which are surrounded by " or '
            var matches = _tokeniser.Matches(rawInput.Trim());
            var tokens = new List<string>(matches.Count);
            foreach (Match m in matches)
            {
                var t = m.Value;

                // Strip the quotes
                if ((t.StartsWith("\"") && t.EndsWith("\"")) || (t.StartsWith("'") && t.EndsWith("'")))
                {
                    t = t.Substring(1, t.Length - 2);
                }
                tokens.Add(t);
            }

            // Skip if we found no tokens from past stripping
            if (tokens.Count == 0)
            {
                command = "";
                args = Array.Empty<string>();
                return;
            }

            command = tokens[0];
            args = tokens.Skip(1).ToArray();
        }

        /// <summary>
        /// Convert string into it's given type. E.g "1" = int, "STATE_IDLE" = enum, etc...
        /// </summary>
        /// <param name="raw">The raw string</param>
        /// <param name="targetType">Type of data we will convert to</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
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

            // Data needs
            throw new InvalidOperationException($"No converter for type {targetType.Name}! Create conversion in .../Commander/Runtime/Core/CommandParse.cs");
        }
    }
}