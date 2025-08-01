# Command Executer for Unity
Custom command executer for Unity versions 2019+

Requires TextMeshPro Package (Free)

## How to use in 3-steps
- Assign 'Command' attribute to a function (public or private)
- Use either the CommandManager OR register the command at Awake/Start to the CommandRegister class
- Call ExecuteCommand in CommandRegister with the user's input
