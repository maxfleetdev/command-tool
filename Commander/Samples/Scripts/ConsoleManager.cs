using Commander;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConsoleManager : MonoBehaviour, ConsoleInput.IUIActions
{
    [SerializeField] private GameObject consoleUI;
    [SerializeField] private TMP_InputField inputField;
    private ConsoleInput _consoleInput;

    private void Awake()
    {
        consoleUI.SetActive(false);
        if (_consoleInput == null)
        {
            _consoleInput = new ConsoleInput();
            _consoleInput.UI.SetCallbacks(this);
        }
        _consoleInput.UI.Enable();
        inputField.onSubmit.AddListener(InputSent);
    }

    private void OnDisable()
    {
        _consoleInput.UI.Disable();
        inputField.onSubmit.RemoveAllListeners();
    }

    public void OnConsoleToggle(InputAction.CallbackContext context)
    {
        bool toggle = !consoleUI.activeSelf;
        consoleUI.SetActive(toggle);
    }

    public void InputSent(string input)
    {
        CommandRegistry.ExecuteCommand(input);
    }
}