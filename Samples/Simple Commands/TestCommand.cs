using UnityEngine;
using Commander;
using Commander.Core;

public class TestCommand : MonoBehaviour
{
    void Awake()
    {
        CommandRegistry.RegisterInstanceCommands(this);
        CommandExecutor.ExecuteCommand("test enum 'hello world'");
    }

    [Command("test", CommandRole.Any)]
    public void PublicMyCommand(int a, string b)
    {
        Debug.Log($"Command executed with parameters: a = {a}, b = {b}");
    }
}