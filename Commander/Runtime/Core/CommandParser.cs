using System;

public static class CommandParser
{
    public static void Parse(string rawInput, out string command, out string[] args)
    {
        if (string.IsNullOrWhiteSpace(rawInput))
        {
            command = "";
            args    = Array.Empty<string>();
            return;
        }

        int idx = rawInput.IndexOf(' ');
        if (idx < 0)
        {
            command = rawInput;
            args    = Array.Empty<string>();
        }
        else
        {
            command = rawInput.Substring(0, idx);
            var rest = rawInput.Substring(idx + 1);

            args = rest.Split(new[]{' '}, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}