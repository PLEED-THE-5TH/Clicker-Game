namespace ClickerGame
{
    public static class ProfileBase
    {
        public static int depthLevel = 1;
        public static int playerHealth = 100;
        public static int clickCount = 0;
        public static Godot.Vector2 mousePos;
        public static Godot.Vector2 localMousePos;
        public static Cell tileClicked;
        public static int miningSpeed = 1;
    }
}
