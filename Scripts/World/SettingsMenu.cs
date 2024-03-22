using Godot;
using System;

public partial class SettingsMenu : CanvasLayer
{
    Button exitButton;
    CheckBox debugScreen;
    CheckBox crtEffect;
    public static bool settingDebugScreen;
    public static bool settingCRTEffect;
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
        settingDebugScreen = !settingDebugScreen;
    }
    private void ToggleCRTEffect()
    {
        settingCRTEffect = !settingCRTEffect;
    }
}
