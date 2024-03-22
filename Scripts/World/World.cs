using ClickerGame;
using Godot;
using System.Collections.Generic;

public partial class World : Node2D
{
    private static Node debugScreen;
    private static Node pauseMenu;
    private static Node settingsMenu;
    private Node postProcess;
    public static World world;
    public override void _Ready()
    {
        world = this;
        postProcess = GetNode<CanvasLayer>("Post Effects");
        LoadSettings();
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
    private void LoadSettings()
    {
        if (SettingsMenu.settingDebugScreen) {
            OpenDebugScreen();
        }
        else if (debugScreen != null) {
            CloseDebugScreen();
        }
        if (SettingsMenu.settingCRTEffect) {

        }
        else {
            CloseDebugScreen();
        }
    }
    private void OpenDebugScreen()
    {
        PackedScene debugScreenScene = GD.Load<PackedScene>("res://Scenes/World/DebugScreen.tscn");
        debugScreen = debugScreenScene.Instantiate();
        world.AddChild(debugScreen);
    }
    private void CloseDebugScreen()
    {
        debugScreen.QueueFree();
        debugScreen = null;
    }
    public static void OpenPauseMenu()
    {
        PackedScene menuScene = GD.Load<PackedScene>("res://Scenes/World/PauseMenu.tscn");
        pauseMenu = menuScene.Instantiate();
        world.AddChild(pauseMenu);
    }
    public static void ClosePauseMenu()
    {
        pauseMenu.QueueFree();
        pauseMenu = null;
        world.LoadSettings();
    }
    public static void OpenSettingsMenu()
    {
        PackedScene settingsScene = GD.Load<PackedScene>("res://Scenes/World/SettingsMenu.tscn");
        settingsMenu = settingsScene.Instantiate();
        world.AddChild(settingsMenu);
    }
    public static void CloseSettingsMenu()
    {
        settingsMenu.QueueFree();
        settingsMenu = null;
    }

    // help stone tasksksksks
    //List<List<char>> someList = new();
    //private List<List<char>> SortByID(List<List<char>> unsortedList, char sortBy)
    //{
    //    List<char> connectedGroups = new();
    //    List<List<char>> returnList = new();
    //    foreach (List<char> id in unsortedList) {
    //        if (id == sortBy) {
    //            returnList.Add(id);
    //            connectedGroups.Add(id.connectedGroupsList);
    //        }
    //    }
    //    foreach (char groupedIDs in connectedGroups) {
    //        SortByID(unsortedList, groupedIDs);
    //    }
    //    return returnList;
    //}
    //private void GetGroupedIDs(List<List<char>> unsortedList, char searchedID)
    //{
    //    List<List<char>> groupedList = new();
    //    SortByID(unsortedList, searchedID);
    //}
    /*
     Sort the list by the ID youre searching and add to MAINLIST
        as you search, keep track of all connected IDs
     Sort through the connected IDs and add to MAINLIST
     
     If you want to get a the list off all connected
     Sort by every ID consecutively BUT, ignore anything and everything youve already added as you go
    */




    public static class Program
    {

        // these could be numbers, or words, or whatever
        public enum ID
        {
            A,
            B,
            C,
            D,
            E,
            F,
            G,
            H,
            I,
            J,
            K,
            L,
            M,
            N,
            O,
            P
        }

        public class Data { }

        public class Entry
        {
            public Entry(ID id, Data data) { ID = id; Data = data; }
            public ID ID;
            public Data Data;
        }

        public static void Main()
        {
            var connections = new List<List<ID>> {
            new() { ID.A, ID.B },
            new() { ID.B, ID.C },
            new() { ID.B, ID.D },
            new() { ID.C, ID.D },
            new() { ID.D, ID.E },
            new() { ID.E, ID.H },
            new() { ID.E, ID.F },
            new() { ID.H, ID.G },
            new() { ID.L, ID.M },
            new() { ID.M, ID.N },
            new() { ID.N, ID.O },
            new() { ID.O, ID.P },
            new() { ID.I, ID.J }
        };

            var objectsToSort = new List<Entry> {
            new(ID.A, new()),
            new(ID.B, new()),
            new(ID.B, new()),
            new(ID.C, new()),
            new(ID.D, new()),
            new(ID.E, new()),
            new(ID.F, new()),
            new(ID.G, new()),
            new(ID.H, new()),
            new(ID.A, new()),
            new(ID.D, new()),
            new(ID.H, new()),
            new(ID.J, new()),
            new(ID.J, new()),
            new(ID.I, new()),
            new(ID.K, new()),
            new(ID.L, new()),
            new(ID.M, new()),
            new(ID.M, new()),
            new(ID.M, new()),
            new(ID.P, new())
        };

            var containers = GroupObjects(objectsToSort);
        }
        private static List<List<Entry>> GroupObjects(List<Entry> objectsToSort)
        {
            List<List<Entry>> groupedEntries = new();
            List<ID> allIDs = GetAllIDs(objectsToSort);
            for (int i = 0; i < allIDs.Count; i++) {
                groupedEntries.Add(new());
            }
            for (int i = 0; i < objectsToSort.Count; i++) {
                for (int j = 0; j < allIDs.Count; j++) {
                    if (objectsToSort[i].ID == allIDs[j]) {
                        groupedEntries[j].Add(objectsToSort[i]);
                    }
                }
            }
            return groupedEntries;
        }
        private static List<List<Entry>> GetConnectedObjects(List<List<ID>> connections, List<List<Entry>> groupedProject)
        {
            List<List<Entry>> connectedGroups = new();
            for (int i = 0; i < groupedProject.Count; i++) {
                if () {

                }
            }
            return connectedGroups;
        }
        private static List<ID> GetAllIDs(List<Entry> objectsToSort)
        {
            List<ID> ids = new();
            for (int i = 0; i < objectsToSort.Count; i++) {
                ids.Add(objectsToSort[i].ID);
            }
            return ids;
        }
    }
}
