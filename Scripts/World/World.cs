using ClickerGame;
using Godot;

public partial class World : Node2D
{
    private static Node pauseMenu;
    public override void _Ready()
    {
    }
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey keyEvent) {
            if (Input.IsActionPressed("esq") && pauseMenu == null) {
                OpenPauseMenu();
            }
        }
    }
    public override void _Process(double delta)
    {
    }
    private void OpenPauseMenu()
    {
        PackedScene menuScene = GD.Load<PackedScene>("res://Scenes/World/PauseMenu.tscn");
        pauseMenu = menuScene.Instantiate();
        AddChild(pauseMenu);
    }
    public static void ClosePauseMenu()
    {
        pauseMenu.QueueFree();
        pauseMenu = null;
    }
}
