using UnityEngine;
using Commander;

public class TestCommand : MonoBehaviour
{
    [HiddenCommand]
    [Command("TestCommand", CommandRole.Any)]
    public void Test()
    {
        
    }
}
