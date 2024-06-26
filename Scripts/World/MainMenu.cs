using Godot;

public partial class MainMenu : CanvasLayer
{
    Button continueButton;
    Button newGameButton;
    Button settingsButton;
    Button exitButton;
    public override void _Ready()
    {
        continueButton = GetNode<Button>("CenterContainer/Control/Panel/VBoxContainer/Continue");
        newGameButton = GetNode<Button>("CenterContainer/Control/Panel/VBoxContainer/New Game");
        settingsButton = GetNode<Button>("CenterContainer/Control/Panel/VBoxContainer/Settings");
        exitButton = GetNode<Button>("CenterContainer/Control/Panel/VBoxContainer/Exit");
        continueButton.Pressed += ContinueButtonPressed;
        newGameButton.Pressed += NewGameButtonPressed;
        settingsButton.Pressed += SettingsButtonPressed;
        exitButton.Pressed += ExitButtonPressed;
    }
    private void ContinueButtonPressed()
    {

    }
    private void NewGameButtonPressed()
    {
        GetTree().ChangeSceneToPacked(GD.Load<PackedScene>("res://Scenes/World/World.tscn"));
    }
    private void SettingsButtonPressed()
    {

    }
    private void ExitButtonPressed()
    {
        GetTree().Quit();
    }
}
