using ClickerGame;
using Godot;
using Microsoft.VisualBasic;
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
        SettingsController settingsController = new SettingsController();
        AddChild(settingsController);
        ProfileBase.Init();
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
    public static void ToggleDebugScreen()
    {
        if (!SettingsController.settings.DebugScreen) {
            PackedScene debugScreenScene = GD.Load<PackedScene>("res://Scenes/World/DebugScreen.tscn");
            debugScreen = debugScreenScene.Instantiate();
            world.AddChild(debugScreen);
        }
        else {
            debugScreen.QueueFree();
            debugScreen = null;
        }
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

            var containers = Solution(connections, objectsToSort);
        }

        private static List<List<Entry>> Solution(List<List<ID>> connections, List<Entry> objectsToSort)
        {
            var numGroups = 0;
            var lookup = new Dictionary<ID, int>();
            foreach (var connection in connections) {
                // some voodoo magic
                foreach (var id in connection) {
                    if (!lookup.ContainsKey(id)) {
                        lookup[id] = numGroups;
                        numGroups++;
                    }
                }
            }
            var groupedObjects = new List<List<Entry>>();
            for (var i = 0; i < numGroups; i++) {
                groupedObjects.Add(new());
            }
            foreach (var Entry in objectsToSort) {
                groupedObjects[lookup[Entry.ID]].Add(Entry);
            }

            return groupedObjects;
        }


        //private static List<List<Entry>> GroupObjects(List<Entry> objectsToSort)
        //{
        //    List<List<Entry>> groupedEntries = new();
        //    List<ID> allIDs = GetAllIDs(objectsToSort);
        //    for (int i = 0; i < allIDs.Count; i++) {
        //        groupedEntries.Add(new());
        //    }
        //    for (int i = 0; i < objectsToSort.Count; i++) {
        //        for (int j = 0; j < allIDs.Count; j++) {
        //            if (objectsToSort[i].ID == allIDs[j]) {
        //                groupedEntries[j].Add(objectsToSort[i]);
        //            }
        //        }
        //    }
        //    return groupedEntries;
        //}
        //private static List<List<Entry>> GetConnectedObjects(List<List<ID>> connections, List<List<Entry>> groupedProject)
        //{
        //    List<List<Entry>> connectedGroups = new List<List<Entry>>();
        //    for (int i = 0; i < connections.Count; i++) {                    // (var connection in connections) or? (int i = 0; i < connections.Count; i++)
        //        List<Entry> connectedGroup = new List<Entry>();
        //        for (int j = 0; j < connections[i].Count; j++) {             // (var id in connection)          or? (int j = 0; j < connections[i].Count; j++)
        //            for (int k = 0; k < groupedProject.Count; k++) {         // (var group in groupedProject)   or? (int k = 0; k < groupedProject.Count; k++)
        //                for (int l = 0; l < groupedProject[i].Count; l++) {  // (var entry in group)            or? (int l = 0; l < groupedProject[i].Count; l++)
        //                    if (groupedProject[k][j].ID == connections[i]) { // (entry.ID == id)                or? (groupedProject[k][j].ID == connections[i])
        //                        connectedGroup.AddRange(groupedProject[k]);  // (group)                         or? (groupedProject[k])
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //        if (connectedGroup.Count > 0) {
        //            connectedGroups.Add(connectedGroup);
        //        }
        //    }
        //    return connectedGroups;
        //}
        //private static List<ID> GetAllIDs(List<Entry> objectsToSort)
        //{
        //    List<ID> ids = new();
        //    for (int i = 0; i < objectsToSort.Count; i++) {
        //        ids.Add(objectsToSort[i].ID);
        //    }
        //    return ids;
        //}
    }
}
