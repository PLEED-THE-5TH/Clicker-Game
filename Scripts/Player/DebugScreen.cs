using ClickerGame;
using Godot;

public partial class DebugScreen : CanvasLayer
{
    Label playerData;
    Label tileData;
    public override void _Ready()
    {
        playerData = GetNode<Label>("Control/Panel/VBoxContainer/Player Data");
        tileData = GetNode<Label>("Control/Panel/VBoxContainer/Tile Data");
    }
    public override void _Process(double delta)
    {
        playerData.Text = $"Player Data:\n    " +
            $"Depth Level: {ProfileBase.depthLevel}\n    " +
            $"Player Health: {ProfileBase.playerHealth}\n    " +
            $"Click Count: {ProfileBase.clickCount}\n    " +
            $"Mouse Pos: {ProfileBase.mousePos}\n    " +
            $"Mouse Pos Local: {ProfileBase.localMousePos}";
        if (ProfileBase.tileClicked != null) {
            tileData.Text = $"Tile Clicked:\n    " +
                $"Tile Location: {ProfileBase.tileClicked.cellLocation}\n    " +
                $"Atlas Position: {ProfileBase.tileClicked.atlasPosition}\n    " +
                $"Dirt Damage: {ProfileBase.tileClicked.dirtDamage}\n    " +
                $"Ore Damage: {ProfileBase.tileClicked.oreDamage}\n    " +
                $"oreType: {Map.oreList[ProfileBase.tileClicked.oreType]}\n    " +
                $"Shoveled: {ProfileBase.tileClicked.shoveled}\n    " +
                $"Fully Mined: {ProfileBase.tileClicked.fullyMined}\n    " +
                $"IsReadyForProgression: {ProfileBase.tileClicked.IsReadyForProgression}";
        }
    }
}
