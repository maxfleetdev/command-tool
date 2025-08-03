using UnityEngine;
using Commander;
using Commander.Core;

public class TestCommand : MonoBehaviour
{
    void Awake()
    {
        CommandRegistry.RegisterInstanceCommands(this);
        CommandRegistry.TryExecuteCommand("publictestcommand", new object[2] { "Test", "test" });
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
}