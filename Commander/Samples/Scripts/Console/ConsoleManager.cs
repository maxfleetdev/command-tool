using Commander;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Commander.Examples
{
    public class ConsoleManager : MonoBehaviour, ConsoleInput.IUIActions
    {
        [SerializeField] private GameObject consoleUI;
        [SerializeField] private TMP_InputField inputField;

        private ConsoleInput _consoleInput;

        #region Lifecycle

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

        #endregion

        #region Input Events

        public void OnConsoleToggle(InputAction.CallbackContext context)
        {
            bool toggle = !consoleUI.activeSelf;
            consoleUI.SetActive(toggle);
        }

        #endregion

        #region UI Events

        public void InputSent(string input)
        {
            CommandRegistry.ExecuteCommand(input);
        }

        #endregion
    }
}