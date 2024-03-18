using ClickerGame;
using Godot;
using System.Collections.Generic;

public partial class Map : Node2D
{
    Timer timer;
    public static int mapWidth = 18;
    public static int mapHeight = 10;
    public static List<List<Cell>> tileMapData;
    public static TileMap tileMap;
    public static Dictionary<string, Godot.Vector2I> tileMapDictionary = new Dictionary<string, Godot.Vector2I>();
    public static List<string> oreList = new() { "Ore_Stone", "Ore_Coal", "Ore_Iron", "Ore_Gold", "Ore_Lapis", "Ore_Emerald", "Ore_Diamond", "Ore_Avorion", "Sp_Bomb" };
    public override void _Ready()
    {
        timer = GetNode<Timer>("Timer");
        timer.Timeout += OnTimerExecute;
        // Dirt Damage States
        tileMapDictionary.Add("0_D_Dirt", new Godot.Vector2I(4, 2));
        tileMapDictionary.Add("1_D_Dirt", new Godot.Vector2I(0, 1));
        tileMapDictionary.Add("2_D_Dirt", new Godot.Vector2I(1, 1));
        tileMapDictionary.Add("3_D_Dirt", new Godot.Vector2I(2, 1));
        tileMapDictionary.Add("4_D_Dirt", new Godot.Vector2I(3, 1));
        tileMapDictionary.Add("UnderGround", new Godot.Vector2I(4, 5));
        // Dirt Edges
        tileMapDictionary.Add("D_Surrounded", new Godot.Vector2I(4, 3));
        tileMapDictionary.Add("D_TopLeft", new Godot.Vector2I(0, 2));
        tileMapDictionary.Add("D_TopMiddle", new Godot.Vector2I(1, 2));
        tileMapDictionary.Add("D_TopRight", new Godot.Vector2I(2, 2));
        tileMapDictionary.Add("D_MiddleLeft", new Godot.Vector2I(0, 3));
        tileMapDictionary.Add("D_MiddleEmpty", new Godot.Vector2I(1, 3));
        tileMapDictionary.Add("D_MiddleRight", new Godot.Vector2I(2, 3));
        tileMapDictionary.Add("D_BottomLeft", new Godot.Vector2I(0, 4));
        tileMapDictionary.Add("D_BottomMiddle", new Godot.Vector2I(1, 4));
        tileMapDictionary.Add("D_BottomRight", new Godot.Vector2I(2, 4));
        // Dirt Tunnels
        tileMapDictionary.Add("D_T_V_Top", new Godot.Vector2I(3, 2));
        tileMapDictionary.Add("D_T_V_Middle", new Godot.Vector2I(3, 3));
        tileMapDictionary.Add("D_T_V_Bottom", new Godot.Vector2I(3, 4));
        tileMapDictionary.Add("D_T_H_Left", new Godot.Vector2I(4, 4));
        tileMapDictionary.Add("D_T_H_Middle", new Godot.Vector2I(5, 4));
        tileMapDictionary.Add("D_T_H_Right", new Godot.Vector2I(6, 4));
        // Dirt Corners
        tileMapDictionary.Add("D_C_TopLeft", new Godot.Vector2I(7, 14));
        tileMapDictionary.Add("D_C_TopRight", new Godot.Vector2I(9, 14));
        tileMapDictionary.Add("D_C_BottomLeft", new Godot.Vector2I(7, 16));
        tileMapDictionary.Add("D_C_BottomRight", new Godot.Vector2I(9, 16));
        tileMapDictionary.Add("D_C_Right", new Godot.Vector2I(7, 2));
        tileMapDictionary.Add("D_C_Left", new Godot.Vector2I(8, 2));
        tileMapDictionary.Add("D_C_Top", new Godot.Vector2I(9, 3));
        tileMapDictionary.Add("D_C_Bottom", new Godot.Vector2I(9, 2));
        tileMapDictionary.Add("D_C_1", new Godot.Vector2I(7, 3));
        tileMapDictionary.Add("D_C_2", new Godot.Vector2I(8, 3));
        tileMapDictionary.Add("D_C_3", new Godot.Vector2I(7, 4));
        tileMapDictionary.Add("D_C_4", new Godot.Vector2I(8, 4));
        tileMapDictionary.Add("D_C_All", new Godot.Vector2I(9, 4));
        // Stone Boarder Innie
        tileMapDictionary.Add("T_L_Inni_Stone", new Godot.Vector2I(3, 0));
        tileMapDictionary.Add("T_M_Inni_Stone", new Godot.Vector2I(4, 0));
        tileMapDictionary.Add("T_R_Inni_Stone", new Godot.Vector2I(5, 0));
        tileMapDictionary.Add("M_L_Inni_Stone", new Godot.Vector2I(3, 1));
        tileMapDictionary.Add("M_R_Inni_Stone", new Godot.Vector2I(5, 1));
        tileMapDictionary.Add("B_L_Inni_Stone", new Godot.Vector2I(3, 2));
        tileMapDictionary.Add("B_M_Inni_Stone", new Godot.Vector2I(4, 2));
        tileMapDictionary.Add("B_R_Inni_Stone", new Godot.Vector2I(5, 2));
        // Solid Stone Tile
        tileMapDictionary.Add("M_M_Inni_Stone", new Godot.Vector2I(5, 2));
        // All Ores Including Stone
        tileMapDictionary.Add("Ore_Stone", new Godot.Vector2I(0, 0));
        tileMapDictionary.Add("Ore_Coal", new Godot.Vector2I(1, 0));
        tileMapDictionary.Add("Ore_Iron", new Godot.Vector2I(2, 0));
        tileMapDictionary.Add("Ore_Gold", new Godot.Vector2I(3, 0));
        tileMapDictionary.Add("Ore_Lapis", new Godot.Vector2I(4, 0));
        tileMapDictionary.Add("Ore_Emerald", new Godot.Vector2I(5, 0));
        tileMapDictionary.Add("Ore_Diamond", new Godot.Vector2I(6, 0));
        tileMapDictionary.Add("Ore_Avorion", new Godot.Vector2I(7, 0));
        // Special Tiles
        tileMapDictionary.Add("Sp_Bomb", new Godot.Vector2I(8, 0));
        tileMapDictionary.Add("Sp_Error", new Godot.Vector2I(4, 6));
        Init();
    }
    public void CheckLevelCompletion()
    {
        if (IsLevelMinedDry()) {
            StartDelayTimer();
        }
    }
    public bool IsLevelMinedDry()
    {
        for (int i = 1; i < 9; i++) {
            for (int j = 1; j < 9; j++) {
                if (!tileMapData[i][j].IsReadyForProgression) {
                    return false;
                }
            }
        }
        return true;
    }
    public void Init()
    {
        tileMap = GetNode<TileMap>("TileMap");
        tileMapData = new List<List<Cell>>();
        for (int i = 0; i < mapWidth; i++) {
            List<Cell> row = new List<Cell>();
            for (int j = 0; j < mapHeight; j++) {
                Cell cell = new Cell() {
                    cellLocation = new Vector2I(i, j),
                    mainMap = this
                };
                if ((i != 0) & (j != 0) & (i < 9) & (j < 9)) {
                    cell.oreType = GD.RandRange(1, 8);
                }
                else {
                    cell.oreType = 0;
                }
                AddChild(cell);
                row.Add(cell);
            }
            tileMapData.Add(row);
        }
        DrawMap();
    }
    public static void DrawMap()
    {
        for (int row = 0; row < mapWidth; row++) {
            for (int col = 0; col < mapHeight; col++) {
                Vector2I drawingLocation = new Vector2I(row, col);
                if ((row != 0) & (col != 0) & (row < 9) & (col < 9)) {
                    Cell tempCell = tileMapData[drawingLocation.X][drawingLocation.Y];
                    if (!tempCell.shoveled) {
                        tileMap.SetCell(2, drawingLocation, 0, tileMapDictionary[(tileMapData[row][col].dirtDamage) + "_D_Dirt"]);
                    }
                    else if (!tempCell.fullyMined) {
                        tileMap.SetCell(1, drawingLocation, 0, tileMapDictionary[oreList[(tileMapData[row][col].oreType)]]);
                    }
                    //if (tempCell.shoveled) {
                    //    List<Cell> surroundingCells = tempCell.GetSurroundingCells();
                    //    List<bool> boolList = new();
                    //    for (int k = 0; k < 9; k++) {
                    //        boolList.Add(true);
                    //    }
                    //    /*
                    //        0 1 2 3 4 5 6 7 8
                    //        3 4 5
                    //        6 7 8
                    //    */
                    //    for (int i = 0; i < surroundingCells.Count; i++) {
                    //        if (surroundingCells[i].cellLocation.X < drawingLocation.X && surroundingCells[i].cellLocation.Y < drawingLocation.Y) {
                    //            boolList[0] = !surroundingCells[i].shoveled;
                    //        }
                    //        if (surroundingCells[i].cellLocation.X == drawingLocation.X && surroundingCells[i].cellLocation.Y < drawingLocation.Y) {
                    //            boolList[1] = !surroundingCells[i].shoveled;
                    //        }
                    //        if (surroundingCells[i].cellLocation.X > drawingLocation.X && surroundingCells[i].cellLocation.Y < drawingLocation.Y) {
                    //            boolList[2] = !surroundingCells[i].shoveled;
                    //        }
                    //        if (surroundingCells[i].cellLocation.X < drawingLocation.X && surroundingCells[i].cellLocation.Y == drawingLocation.Y) {
                    //            boolList[3] = !surroundingCells[i].shoveled;
                    //        }
                    //        if (surroundingCells[i].cellLocation.X == drawingLocation.X && surroundingCells[i].cellLocation.Y == drawingLocation.Y) {
                    //            boolList[4] = !surroundingCells[i].shoveled;
                    //        }
                    //        if (surroundingCells[i].cellLocation.X > drawingLocation.X && surroundingCells[i].cellLocation.Y == drawingLocation.Y) {
                    //            boolList[5] = !surroundingCells[i].shoveled;
                    //        }
                    //        if (surroundingCells[i].cellLocation.X < drawingLocation.X && surroundingCells[i].cellLocation.Y > drawingLocation.Y) {
                    //            boolList[6] = !surroundingCells[i].shoveled;
                    //        }
                    //        if (surroundingCells[i].cellLocation.X == drawingLocation.X && surroundingCells[i].cellLocation.Y > drawingLocation.Y) {
                    //            boolList[7] = !surroundingCells[i].shoveled;
                    //        }
                    //        if (surroundingCells[i].cellLocation.X > drawingLocation.X && surroundingCells[i].cellLocation.Y > drawingLocation.Y) {
                    //            boolList[8] = !surroundingCells[i].shoveled;
                    //        }
                    //    }
                    //    if (tileMapData[drawingLocation.X][drawingLocation.Y].shoveled) {
                    //        if (boolList[1] && boolList[3] && !boolList[5] && !boolList[7] && !boolList[8]) {
                    //            tileMap.SetCell(2, drawingLocation, 0, tileMapDictionary["D_TopLeft"]);
                    //        }
                    //        if (boolList[1] && !boolList[3] && !boolList[5] && !boolList[7] && !boolList[6] && !boolList[8]) {
                    //            tileMap.SetCell(2, drawingLocation, 0, tileMapDictionary["D_TopMiddle"]);
                    //        }
                    //        if (boolList[1] && !boolList[3] && boolList[5] && !boolList[7] && boolList[0] && boolList[2] && boolList[6] && boolList[8]) {
                    //            tileMap.SetCell(2, drawingLocation, 0, tileMapDictionary["D_TopRight"]);
                    //        }
                    //        if (!boolList[1] && boolList[3] && !boolList[5] && !boolList[7] && boolList[0] && boolList[2] && boolList[6] && boolList[8]) {
                    //            tileMap.SetCell(2, drawingLocation, 0, tileMapDictionary["D_MiddleLeft"]);
                    //        }
                    //        if (!boolList[1] && !boolList[3] && !boolList[5] && !boolList[7] && boolList[0] && boolList[2] && boolList[6] && boolList[8]) {
                    //            tileMap.SetCell(2, drawingLocation, 0, tileMapDictionary["D_MiddleEmpty"]);
                    //        }
                    //        if (!boolList[1] && !boolList[3] && boolList[5] && !boolList[7] && boolList[0] && boolList[2] && boolList[6] && boolList[8]) {
                    //            tileMap.SetCell(2, drawingLocation, 0, tileMapDictionary["D_MiddleRight"]);
                    //        }
                    //        if (!boolList[1] && boolList[3] && !boolList[5] && boolList[7] && boolList[0] && boolList[2] && boolList[6] && boolList[8]) {
                    //            tileMap.SetCell(2, drawingLocation, 0, tileMapDictionary["D_BottomLeft"]);
                    //        }
                    //        if (!boolList[1] && !boolList[3] && !boolList[5] && boolList[7] && boolList[0] && boolList[2] && boolList[6] && boolList[8]) {
                    //            tileMap.SetCell(2, drawingLocation, 0, tileMapDictionary["D_BottomMiddle"]);
                    //        }
                    //        if (!boolList[1] && !boolList[3] && boolList[5] && boolList[7] && boolList[0] && boolList[2] && boolList[6] && boolList[8]) {
                    //            tileMap.SetCell(2, drawingLocation, 0, tileMapDictionary["D_BottomRight"]);
                    //        }
                    //        if (boolList[1] && boolList[3] && boolList[5] && !boolList[7] && boolList[0] && boolList[2] && boolList[6] && boolList[8]) {
                    //            tileMap.SetCell(2, drawingLocation, 0, tileMapDictionary["D_T_V_Top"]);
                    //        }
                    //        if (!boolList[1] && boolList[3] && boolList[5] && !boolList[7] && boolList[0] && boolList[2] && boolList[6] && boolList[8]) {
                    //            tileMap.SetCell(2, drawingLocation, 0, tileMapDictionary["D_T_V_Middle"]);
                    //        }
                    //        if (!boolList[1] && boolList[3] && boolList[5] && boolList[7] && boolList[0] && boolList[2] && boolList[6] && boolList[8]) {
                    //            tileMap.SetCell(2, drawingLocation, 0, tileMapDictionary["D_T_V_Bottom"]);
                    //        }
                    //        if (boolList[1] && boolList[3] && boolList[5] && boolList[7] && boolList[0] && boolList[2] && boolList[6] && boolList[8]) {
                    //            tileMap.SetCell(2, drawingLocation, 0, tileMapDictionary["D_Surrounded"]);
                    //        }
                    //        if (boolList[1] && boolList[3] && !boolList[5] && boolList[7] && boolList[0] && boolList[2] && boolList[6] && boolList[8]) {
                    //            tileMap.SetCell(2, drawingLocation, 0, tileMapDictionary["D_T_H_Left"]);
                    //        }
                    //        if (boolList[1] && !boolList[3] && !boolList[5] && boolList[7] && boolList[0] && boolList[2] && boolList[6] && boolList[8]) {
                    //            tileMap.SetCell(2, drawingLocation, 0, tileMapDictionary["D_T_H_Middle"]);
                    //        }
                    //        if (boolList[1] && !boolList[3] && boolList[5] && boolList[7] && boolList[0] && boolList[2] && boolList[6] && boolList[8]) {
                    //            tileMap.SetCell(2, drawingLocation, 0, tileMapDictionary["D_T_H_Right"]);
                    //        }
                    //        if (!boolList[1] && !boolList[3] && !boolList[5] && !boolList[7] && boolList[8]) {
                    //            tileMap.SetCell(2, drawingLocation, 0, tileMapDictionary["D_C_TopLeft"]);
                    //        }
                    //        if (!boolList[1] && !boolList[3] && !boolList[5] && !boolList[7] && boolList[6]) {
                    //            tileMap.SetCell(2, drawingLocation, 0, tileMapDictionary["D_C_TopRight"]);
                    //        }
                    //        if (!boolList[1] && !boolList[3] && !boolList[5] && !boolList[7] && boolList[2]) {
                    //            tileMap.SetCell(2, drawingLocation, 0, tileMapDictionary["D_C_BottomLeft"]);
                    //        }
                    //        if (!boolList[1] && !boolList[3] && !boolList[5] && !boolList[7] && boolList[0]) {
                    //            tileMap.SetCell(2, drawingLocation, 0, tileMapDictionary["D_C_BottomRight"]);
                    //        }
                    //        /*
                    //        0 1 2
                    //        3 4 5
                    //        6 7 8
                    //         */
                    //        if (!boolList[1] && !boolList[3] && !boolList[5] && !boolList[7] && !boolList[0] && !boolList[6] && boolList[2] && boolList[8]) {
                    //            tileMap.SetCell(2, drawingLocation, 0, tileMapDictionary["D_C_Right"]);
                    //        }
                    //        if (!boolList[1] && !boolList[3] && !boolList[5] && !boolList[7] && boolList[0] && boolList[6] && !boolList[2] && !boolList[8]) {
                    //            tileMap.SetCell(2, drawingLocation, 0, tileMapDictionary["D_C_Left"]);
                    //        }
                    //        if (!boolList[1] && !boolList[3] && !boolList[5] && !boolList[7] && boolList[0] && !boolList[6] && boolList[2] && !boolList[8]) {
                    //            tileMap.SetCell(2, drawingLocation, 0, tileMapDictionary["D_C_Top"]);
                    //        }
                    //        if (!boolList[1] && !boolList[3] && !boolList[5] && !boolList[7] && !boolList[0] && boolList[6] && !boolList[2] && boolList[8]) {
                    //            tileMap.SetCell(2, drawingLocation, 0, tileMapDictionary["D_C_Bottom"]);
                    //        }
                    //        if (!boolList[1] && !boolList[3] && !boolList[5] && !boolList[7] && boolList[0] && boolList[6] && boolList[2] && !boolList[8]) {
                    //            tileMap.SetCell(2, drawingLocation, 0, tileMapDictionary["D_C_1"]);
                    //        }
                    //        if (!boolList[1] && !boolList[3] && !boolList[5] && !boolList[7] && boolList[0] && !boolList[6] && boolList[2] && boolList[8]) {
                    //            tileMap.SetCell(2, drawingLocation, 0, tileMapDictionary["D_C_2"]);
                    //        }
                    //        if (!boolList[1] && !boolList[3] && !boolList[5] && !boolList[7] && boolList[0] && boolList[6] && !boolList[2] && boolList[8]) {
                    //            tileMap.SetCell(2, drawingLocation, 0, tileMapDictionary["D_C_3"]);
                    //        }
                    //        if (!boolList[1] && !boolList[3] && !boolList[5] && !boolList[7] && !boolList[0] && boolList[6] && boolList[2] && boolList[8]) {
                    //            tileMap.SetCell(2, drawingLocation, 0, tileMapDictionary["D_C_4"]);
                    //        }
                    //        if (!boolList[1] && !boolList[3] && !boolList[5] && !boolList[7] && boolList[0] && boolList[2] && boolList[6] && boolList[8]) {
                    //            tileMap.SetCell(2, drawingLocation, 0, tileMapDictionary["D_C_All"]);
                    //        }
                    //    }
                    //    if (tempCell.shoveled && tempCell.fullyMined) {
                    //        tileMap.SetCell(1, drawingLocation, -1);
                    //        tileMap.SetCell(0, drawingLocation, 0, tileMapDictionary["UnderGround"]);
                    //    }
                    //}
                }
            }
        }
    }
    public static void DrawPlayArea()
    {

    }
    public static bool IsInPlayerArea(Vector2I location)
    {
        return (location.X < 1 || location.X > 8 || location.Y < 1 || location.Y > 8);
    }
    private void StartDelayTimer()
    {
        timer.Start();
    }
    private void OnTimerExecute()
    {
        ProfileBase.depthLevel++;
        Init();
    }
}
