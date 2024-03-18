using ClickerGame;
using Godot;

public partial class DebugScreen : Node2D
{
	Label depthLevel;
    Label playerHealth;
    Label clickCount;
    Label mousePos;
	Label localMousePos;
	Label tileClicked;
    public override void _Ready()
	{
        playerHealth = GetNode<Label>("Control/Panel/VBoxContainer/Player Health");
        depthLevel = GetNode<Label>("Control/Panel/VBoxContainer/Depth Level");
        mousePos = GetNode<Label>("Control/Panel/VBoxContainer/Mouse Position");
		clickCount = GetNode<Label>("Control/Panel/VBoxContainer/Click Count");
		localMousePos = GetNode<Label>("Control/Panel/VBoxContainer/Local Mouse Position");
        tileClicked = GetNode<Label>("Control/Panel/VBoxContainer/Tile Clicked");
    }
	public override void _Process(double delta)
	{
        depthLevel.Text = $"Depth Level: {ProfileBase.depthLevel}";
        playerHealth.Text = $"Player Health: {ProfileBase.playerHealth}";
        clickCount.Text = $"Click Count: {ProfileBase.clickCount}";
        mousePos.Text = $"Mouse Position: {ProfileBase.mousePos}";
        clickCount.Text = $"Click Count: {ProfileBase.clickCount}";
		localMousePos.Text = $"Local Mouse Pos: {ProfileBase.localMousePos}";
        if (ProfileBase.tileClicked != null) {
            tileClicked.Text = $"Tile Clicked:\n    " +
                $"Tile Location: {ProfileBase.tileClicked.cellLocation}\n    " +
                $"Dirt Damage: {ProfileBase.tileClicked.dirtDamage}\n    " +
                $"Ore Damage: {ProfileBase.tileClicked.oreDamage}\n    " +
                $"oreType: {Map.oreList[ProfileBase.tileClicked.oreType]}\n    " +
                $"Shoveled: {ProfileBase.tileClicked.shoveled}\n    " +
                $"Fully Mined: {ProfileBase.tileClicked.fullyMined}\n    " +
                $"IsReadyForProgression: {ProfileBase.tileClicked.IsReadyForProgression}";
        }
    }
}
