using System.Collections.Generic;

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
        public static int coalOwned = 0;
        public static int ironOwned = 0;
        public static int goldOwned = 0;
        public static int lapisOwned = 0;
        public static int emeraldOwned = 0;
        public static int diamondOwned = 0;
        public static int avorionOwned = 0;
        public static List<int> oreOwned;
        public static void Init()
        {
            oreOwned = new List<int>(7);
            for (int i = 0; i < 7; i++) {
                oreOwned.Add(0);
            }
        }
    }
}
