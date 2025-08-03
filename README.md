# Command Tool for Unity 2019+

## Important Note
This is an ongoing project, so expect changes. All code is not final and is subject to change. I am currently working on restructuring the repo to contain less git fluff

## Planned Features
- Automatic Command Registering (source code generator?)
- Async for Executing/Registering
- Attribute Flags (eg Hidden, Roles etc)
- Role assignment and command permissions
- Command Line Example
- Abstraction for user customisation


## Installation
### Option A (GIT Package):
1) Open Unity Package Manager
2) Click '+' icon and select 'Install from git URL'
3) Paste <code>https://github.com/maxfleetdev/unity-command-tool.git</code> into prompt
4) Hit Enter/Click done
5) Package will begin installing

### Option B (Package):
1) Download commander.unitypackage from latest release page
2) Double click on downloaded file
3) Unity will automatically install inside the open project

## How To Use
### Command Attribute
Commands are assigned by using the <code>Command[...]</code> attribute:
```c#
[Command("give", "Gives player an item using ID and Quantity")]
private void GiveCommand(int id, int quantity)
{
  ...
```
Attributes have many overloads, so you can provide descriptions, hidden commands, certain role access etc (working on improving with extra attributes)

Commands can be assigned in any class, static or instanced (MonoBehaviour)
- Static Command methods are registered on startup automatically by CommandRegistry
- Instanced Commands methods **must** be registered on creation (Awake/Start etc)

### Static Command Example:
```c#
[Command("give", "Gives player an item using ID and Quantity")]
private static void GiveCommand(int id, int quantity)
{
  AddItem(id, quantity);
}
```

### Instance Command Example:
```c#
private void Awake()
{
  // Called Only Once
  CommandRegistry.RegisterInstanceCommands(this);
}

[Command("give", "Gives player an item using ID and Quantity")]
private static void GiveCommand(int id, int quantity)
{
  AddItem(id, quantity);
}
```

### Executing Commands
To execute commands, you simply pass the raw inputted string (eg "give 192 1") to the CommandExecuter class.
The CommandExecuter class automatically converts the string into 2 parts: The command name, and the arguments
- The command name is what we are looking to invoke (eg give)
- The arguments are the method parameters the command method takes (eg int id, int quantity)

It achieves this by converting the split string into each parameter Data Type. You can take a look inside CommandParser.cs for how it works.
```c#
// userInput = "give 192 1"
private void OnInputSubmitted(string userInput)
{
  CommandExecutor.ExecuteCommand(userInput);
  // Returns success - player recieves 1x of item ID 192
}
```
If the command is successful, a log will be created. Otherwise, a warning will appear notifying the user that the command was incorrect.
