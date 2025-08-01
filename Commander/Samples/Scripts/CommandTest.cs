using UnityEngine;

public class CommandTest : MonoBehaviour
{
    [Command("TestCommand")]
    private void TestCommandMethod(int a, string b)
    {
        Debug.Log($"Test command executed with parameters: a={a}, b={b}");
    }
}