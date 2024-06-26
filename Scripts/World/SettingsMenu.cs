using Godot;
using System;

public partial class SettingsMenu : CanvasLayer
{
    Button exitButton;
    CheckBox debugScreen;
    CheckBox crtEffect;
    public override void _Ready()
	{
        exitButton = GetNode<Button>("CenterContainer/Control/Panel/Exit");
        debugScreen = GetNode<CheckBox>("CenterContainer/Control/Panel/VBoxContainer/Debug Screen");
        crtEffect = GetNode<CheckBox>("CenterContainer/Control/Panel/VBoxContainer/CRT Effect");
        exitButton.Pressed += ExitButtonPressed;
        debugScreen.Pressed += ToggleDebugScreen;
        crtEffect.Pressed += ToggleCRTEffect;
    }
	public override void _Process(double delta)
	{
	}
    private void ExitButtonPressed()
    {
        World.CloseSettingsMenu();
        World.OpenPauseMenu();
    }
    private void ToggleDebugScreen()
    {
        SettingsController.settings.DebugScreen = !SettingsController.settings.DebugScreen;
        SettingsController.UpdateWorld();
    }
    private void ToggleCRTEffect()
    {
        SettingsController.settings.CRTEffect = !SettingsController.settings.CRTEffect;
        SettingsController.UpdateWorld();
    }
}
