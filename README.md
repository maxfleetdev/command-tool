# Command Tool for Unity 2019+

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
