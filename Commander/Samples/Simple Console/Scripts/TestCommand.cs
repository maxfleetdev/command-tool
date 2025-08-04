using UnityEngine;
using Commander;

public class TestCommand : MonoBehaviour
{
    void Awake()
    {
        CommandRegistry.RegisterInstanceCommands(this);
    }

    [Command("test", "This is a test", CommandRole.Any)]
    public void PublicMyCommand(int a, string b)
    {
        //Debug.Log($"Command executed with parameters: a = {a}, b = {b}");
    }
}