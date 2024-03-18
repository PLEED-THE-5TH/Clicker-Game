using ClickerGame;
using Godot;

public partial class MouseController : Control
{
    World thisWorld;
    public override void _Ready()
    {
        thisWorld = GetParent<World>();
    }
    public override void _Process(double delta)
    {
        
    }
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton eventMouseButton) {
            if (@event.IsPressed()) {
                ProfileBase.localMousePos = thisWorld.GetLocalMousePosition();
                OnClick(ProfileBase.localMousePos);
            }
        }
        if (@event is InputEventMouse eventMouseMotion) {
            ProfileBase.mousePos = eventMouseMotion.Position;
            ProfileBase.localMousePos = thisWorld.GetLocalMousePosition();
        }
    }
    void OnClick(Vector2 location)
    {
        float locWidth = location.X / 16;
        float locHeight = location.Y / 16;
        if (locWidth >= 0 && locWidth < Map.mapWidth && locHeight >= 0 && locHeight < Map.mapHeight) {
            Cell clickedCell = Map.tileMapData[(int)locWidth][(int)locHeight];
            ProfileBase.tileClicked = clickedCell;
            clickedCell.OnClicked();
        }
    }
}
