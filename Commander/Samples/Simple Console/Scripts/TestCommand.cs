using UnityEngine;
using Commander;

public class TestCommand : MonoBehaviour
{
    void Awake()
    {
        CommandRegistry.RegisterInstanceCommands(this);
        CommandExecutor.ExecuteCommand("helpp");
        CommandExecutor.ExecuteCommand("help");
        CommandExecutor.ExecuteCommand("test 1 test");
        CommandExecutor.ExecuteCommand("test test 1");
    }

    [Command("test", "This is a test", CommandRole.Any)]
    public void PublicMyCommand(int a, string b)
    {
        Debug.Log($"Command executed with parameters: a = {a}, b = {b}");
    }

    [Command("AnotherTest", "This is yet another test", CommandRole.Any)]
    public void AnotherCommand(int a)
    {
        Debug.Log($"Command executed with parameters: a = {a}");
    }
}