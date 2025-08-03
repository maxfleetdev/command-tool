using UnityEngine;
using Commander;
using Commander.Core;

public class TestCommand : MonoBehaviour
{
    void Awake()
    {
        CommandRegistry.RegisterCommands(this);
        CommandRegistry.TryExecuteCommand("PublicTestCommand", new object[2] { 1, "String" });
        CommandRegistry.TryExecuteCommand("NoCommand", new object[2] { 1, "String" });
        CommandRegistry.TryExecuteCommand("PrivateTestCommand", new object[2] { 1, "String" });
    }

    [Command("PublicTestCommand", CommandRole.Any)]
    public void PublicMyCommand(int a, string b)
    {
        Debug.Log($"Command executed with parameters: a = {a}, b = {b}");
    }

    [Command("PrivateTestCommand", CommandRole.Any)]
    private void PrivateMyCommand(int a, string b)
    {
        Debug.Log($"Command executed with parameters: a = {a}, b = {b}");
    }

    private void NonCommandMethod()
    {
        Debug.Log("This method is not a command and will not be registered.");
    }
}