using ClickerGame;
using Godot;
using System;

public partial class SideScreen : CanvasLayer
{
    Label playerData;
    Label oreData;
	public override void _Ready()
	{
        playerData = GetNode<Label>("Control/Panel/Panel/VBoxContainer/Player");
        oreData = GetNode<Label>("Control/Panel/Panel/VBoxContainer/Ore");

    }
	public override void _Process(double delta)
	{
        playerData.Text = "Player:\n" +
            $"    Health: {ProfileBase.playerHealth}\n" +
            $"    Depth Level: {ProfileBase.depthLevel}";
        oreData.Text = "Inventory:\n" +
            $"    Coal: {ProfileBase.oreOwned[0]}\n" +
            $"    Iron: {ProfileBase.oreOwned[1]}\n" +
            $"    Gold: {ProfileBase.oreOwned[2]}\n" +
            $"    Lapis: {ProfileBase.oreOwned[3]}\n" +
            $"    Emerald: {ProfileBase.oreOwned[4]}\n" +
            $"    Diamond: {ProfileBase.oreOwned[5]}\n" +
            $"    Avorion: {ProfileBase.oreOwned[6]}";
    }
}
