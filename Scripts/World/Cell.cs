using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;

namespace ClickerGame
{
    public partial class Cell : Node
    {
        public Map mainMap;
        public int dirtDamage = 0;
        public int oreDamage = 0;
        public bool IsReadyForProgression;
        public int oreType;
        public bool shoveled;
        public bool fullyMined;
        public Godot.Vector2I cellLocation;
        public Vector2I atlasPosition;
        public void OnClicked()
        {
            if (oreType != 0) {
                MineCurrentCell();
            }
            // Insert ui interactions?
        }
        private void MineCurrentCell()
        {
            AnimationController.MiningAnimation((cellLocation * 16) + new Vector2(8, 8));
            if (IsReadyForProgression && fullyMined) { return; }
            Count();
            TileStateCheck();
            //UpdateCell();
            Map.DrawMap();
            mainMap.CheckLevelCompletion();
        }
        private void Bomb()
        {
            AnimationController.BombAnimation((cellLocation * 16) + new Vector2(8, 8), DamageSurroundingCells);
            ProfileBase.playerHealth -= 10;
            mainMap.CheckLevelCompletion();
        }
        private void Count()
        {
            ProfileBase.clickCount++;
            if (!shoveled) {
                dirtDamage += ProfileBase.miningSpeed;
            }
            else {
                oreDamage += ProfileBase.miningSpeed;
            }
            if (oreDamage > 0 && Map.oreList[oreType] == "Sp_Bomb") {
                //Bomb();
            }
        }
        private void TileStateCheck()
        {
            if (!shoveled) {
                if (dirtDamage >= 2) {
                    shoveled = true;
                    dirtDamage = -1;
                    if (Map.oreList[oreType] == "Sp_Bomb") {
                        IsReadyForProgression = true;
                    }
                    //UpdateNBors();
                    Map.DrawMap();
                }
            }
            else {
                if (oreDamage > 0) {
                    fullyMined = true;
                    IsReadyForProgression = true;
                    oreDamage = -1;
                    GiveOreToPlayer();
                }
            }
        }
        private void GiveOreToPlayer()
        {
            if (oreType != 8) {
                ProfileBase.oreOwned[oreType - 1]++;
            }
        }
        private string GenerateNextTile()
        {
            if (fullyMined) {
                return "Dirt_Hole";
            }
            else if (shoveled) {
                return Map.oreList[oreType];
            }
            else {
                return (dirtDamage) + "_D_Dirt";
            }
        }
        public List<Cell> GetSurroundingCells()
        {
            List<Cell> surroundingCells = new();
            for (int i = 1; i <= 3; i++) {
                for (int j = 1; j <= 3; j++) {
                    Vector2I locOfCellInQuestion = new Vector2I(cellLocation.X - 2 + i, cellLocation.Y - 2 + j);
                    if (locOfCellInQuestion.X < 1 || locOfCellInQuestion.X > 8 || locOfCellInQuestion.Y < 1 || locOfCellInQuestion.Y > 8 || (i == 2 && j == 2)) {
                        continue;
                    }
                    surroundingCells.Add(Map.tileMapData[locOfCellInQuestion.X][locOfCellInQuestion.Y]);
                }
            }
            return surroundingCells;
        }
        private void DamageSurroundingCells()
        {
            List<Cell> surroundingCells = GetSurroundingCells();
            for (int i = 0; i < surroundingCells.Count; i++) {
                if (surroundingCells[i].oreType == 8 && surroundingCells[i].fullyMined == false) {
                    surroundingCells[i].Bomb();
                }
                surroundingCells[i].fullyMined = true;
                surroundingCells[i].IsReadyForProgression = true;
                surroundingCells[i].dirtDamage = -1;
                surroundingCells[i].oreDamage = -1;
                Map.tileMapData[surroundingCells[i].cellLocation.X][surroundingCells[i].cellLocation.Y] = surroundingCells[i];
            }
            //UpdateNBors();
            Map.DrawMap();
        }
        public void UpdateCell()
        {
            if (!shoveled) {
                Map.tileMap.SetCell(2, cellLocation, 0, Map.tileMapDictionary[dirtDamage + "_D_Dirt"]);
                return;
            }
            if (!fullyMined) {
                Map.tileMap.SetCell(1, cellLocation, 0, Map.tileMapDictionary[Map.oreList[oreType]]);
            }
            Map.tileMap.SetCell(1, cellLocation, -1);
            Map.tileMap.SetCell(0, cellLocation, 0, Map.tileMapDictionary["UnderGround"]);
            int IsNeighborNotShoveled(Vector2I nBor)
            {
                return Map.tileMapData[nBor.X][nBor.Y].shoveled ? 0 : 1;
            }
            int topLeft = IsNeighborNotShoveled(new Vector2I(cellLocation.X - 1, cellLocation.Y - 1));
            int top = IsNeighborNotShoveled(new Vector2I(cellLocation.X, cellLocation.Y - 1));
            int topRight = IsNeighborNotShoveled(new Vector2I(cellLocation.X + 1, cellLocation.Y - 1));
            int left = IsNeighborNotShoveled(new Vector2I(cellLocation.X - 1, cellLocation.Y));
            int right = IsNeighborNotShoveled(new Vector2I(cellLocation.X + 1, cellLocation.Y));
            int bottomLeft = IsNeighborNotShoveled(new Vector2I(cellLocation.X - 1, cellLocation.Y + 1));
            int bottom = IsNeighborNotShoveled(new Vector2I(cellLocation.X, cellLocation.Y + 1));
            int bottomRight = IsNeighborNotShoveled(new Vector2I(cellLocation.X + 1, cellLocation.Y + 1));
            //int orthogonal = ((top << 3) | (left << 2) | (right << 1) | bottom) + 7;
            int atlasY = ((top << 3) | (left << 2) | (right << 1) | bottom) + 7;
            //
            // 1 0 0
            // 0 0 0
            // 1 1 1
            //
            // Y = 1 + the 7
            // X = 1
            // 
            //               (              1        )   (               0         )   (                 0           )   (                  0            )
            //               (      1            1   )   (      1             0    )   (         0             1     )   (         0               1     )
            //               (   0      0        1   )   (   0      0         0    )   (     1       0         1     )   (    1        0           1     )
            //int diagonal = (~(top | left) & topLeft) + (~(top | right) & topRight) + (~(bottom | left) & bottomLeft) + (~(bottom | right) & bottomRight);
            int atlasX = (~(top | left) & topLeft) + (~(top | right) & topRight) + (~(bottom | left) & bottomLeft) + (~(bottom | right) & bottomRight);
            //GD.Print(cellLocation);
            //GD.Print(diagonal, ", " , orthogonal);
            Map.tileMap.SetCell(2, cellLocation, 0, new Vector2I(atlasX, atlasY));
            atlasPosition = new Vector2I(atlasX, atlasY);
        }
        private void UpdateNBors()
        {
            foreach (Cell nBor in GetSurroundingCells()) {
                nBor.UpdateCell();
            }
        }
        /* 
        * Depending on which orthogonal neighbor cells are shoveled, there are some diagonals that we don't care about.
        * By applying a bitmask to the diagonal bits, we can ensure that the bits we don't care about will be 0.
        * 
        * input:
        *  orthogonal bits
        *  format (4 bits): top, left, right, bottom
        * 
        * output:
        *  diagonal bitmask
        *  format (4 bits): top left, top right, bottom left, bottom right
        */
        //private static readonly int[] DiagonalBitMasks = [
        //    //  bitmask,    orthogonal Unshoveled neighbors
        //    0b1111, //  none
        //    0b1100, //  bottom
        //    0b1010, //  right
        //    0b1000, //  right, bottom
        //    0b0101, //  left
        //    0b0100, //  left, bottom
        //    0b0000, //  left, right
        //    0b0000, //  left, right, bottom
        //    0b0011, //  top
        //    0b0000, //  top, bottom
        //    0b0010, //  top, right
        //    0b0000, //  top, right, bottom
        //    0b0001, //  top, left
        //    0b0000, //  top, left, bottom
        //    0b0000, //  top, left, right
        //    0b0000  //  top, left, right, bottom
        //];
        //private static readonly new int[] {
        //    0b1111, // none
        //    0b1100, // bottom
        //    0b1010, // right
        //    0b1000, // right, bottom
        //    0b0101, // left
        //    0b0100, // left, bottom
        //    0b0000, // left, right
        //    0b0000, // left, right, bottom
        //    0b0011, // top
        //    0b0000, // top, bottom
        //    0b0010, // top, right
        //    0b0000, // top, right, bottom
        //    0b0001, // top, left
        //    0b0000, // top, left, bottom
        //    0b0000, // top, left, right
        //    0b0000  // top, left, right, bottom
        //}
        /* 
        * input:
        *  format (8 bits): top, left, right, bottom, top left, top right, bottom left, bottom right
        *  this assumes that the correct bitmask has been applied to the last 4 bits
        * 
        * output:
        *  Vector2I representing the atlas location
        */
        //private static readonly Dictionary<int, Vector2I> SpriteSheetLookup = new() {
        //    //      direction bits, atlas location, Unshoveled neighbors
        //    {   0b00000000,     new(0, 0) }, // none
        //    {   0b00001000,     new(0, 0) }, // top left
        //    {   0b00000100,     new(0, 0) }, // top right
        //    {   0b00001100,     new(0, 0) }, // top left, top right
        //    {   0b00000010,     new(0, 0) }, // bottom left
        //    {   0b00001010,     new(0, 0) }, // top left, bottom left
        //    {   0b00000110,     new(0, 0) }, // top right, bottom left
        //    {   0b00001110,     new(0, 0) }, // top left, top right, bottom left
        //    {   0b00000001,     new(0, 0) }, // bottom right
        //    {   0b00001001,     new(0, 0) }, // top left, bottom right
        //    {   0b00000101,     new(0, 0) }, // top right, bottom right
        //    {   0b00001101,     new(0, 0) }, // top left, top right, bottom right
        //    {   0b00000011,     new(0, 0) }, // bottom left, bottom right
        //    {   0b00001011,     new(0, 0) }, // top left, bottom left, bottom right
        //    {   0b00000111,     new(0, 0) }, // top right, bottom left, bottom right
        //    {   0b00001111,     new(0, 0) }, // top left, top right, bottom left, bottom right
        //    {   0b10000000,     new(0, 0) }, // top
        //    {   0b10000010,     new(0, 0) }, // top, bottom left
        //    {   0b10000001,     new(0, 0) }, // top, bottom right
        //    {   0b10000011,     new(0, 0) }, // top, bottom left, bottom right
        //    {   0b01000000,     new(0, 0) }, // left
        //    {   0b01000100,     new(0, 0) }, // left, top right
        //    {   0b01000001,     new(0, 0) }, // left, bottom right
        //    {   0b01000101,     new(0, 0) }, // left, top right, bottom right
        //    {   0b11000000,     new(0, 0) }, // top, left
        //    {   0b11000001,     new(0, 0) }, // top, left, bottom right
        //    {   0b00100000,     new(0, 0) }, // right
        //    {   0b00101000,     new(0, 0) }, // right, top left
        //    {   0b00100010,     new(0, 0) }, // right, bottom left
        //    {   0b00101010,     new(0, 0) }, // right, top left, bottom left
        //    {   0b10100000,     new(0, 0) }, // top, right
        //    {   0b10100010,     new(0, 0) }, // top, right, bottom left
        //    {   0b01100000,     new(0, 0) }, // left, right
        //    {   0b11100000,     new(0, 0) }, // top, left, right
        //    {   0b00010000,     new(0, 0) }, // bottom
        //    {   0b00011000,     new(0, 0) }, // bottom, top left
        //    {   0b00010100,     new(0, 0) }, // bottom, top right
        //    {   0b00011100,     new(0, 0) }, // bottom, top left, top right
        //    {   0b10010000,     new(0, 0) }, // top, bottom
        //    {   0b01010000,     new(0, 0) }, // left, bottom
        //    {   0b01010100,     new(0, 0) }, // left, bottom, top right
        //    {   0b11010000,     new(0, 0) }, // top, left, bottom
        //    {   0b00110000,     new(0, 0) }, // right, bottom
        //    {   0b00111000,     new(0, 0) }, // right, bottom, top left
        //    {   0b10110000,     new(0, 0) }, // top, right, bottom
        //    {   0b01110000,     new(0, 0) }, // left, right, bottom
        //    {   0b11110000,     new(0, 0) }, // top, left, right, bottom
        //};

        //private Vector2I GetAtlasLocation()
        //{

        //    // keep these the way they were before
        //    var top = 0;
        //    var topLeft = 0;
        //    var topRight = 0;
        //    var bottom = 0;
        //    var bottomLeft = 0;
        //    var bottomRight = 0;
        //    var left = 0;
        //    var right = 0;

        //    // orthogonal bits
        //    // format (4 bits): top, left, right, bottom
        //    var orthogonal = (top << 3) | (left << 2) | (right << 1) | bottom;

        //    // diagonal bits
        //    // format (4 bits): top left, top right, bottom left, bottom right
        //    var diagonal = (topLeft << 3) | (topRight << 2) | (bottomLeft << 1) | bottomRight;

        //    // push the orthogonal bits 4 bits to the front, apply the diagonal bitmask, and join them together
        //    // format (8 bits): top, left, right, bottom, top left, top right, bottom left, bottom right
        //    var lookupKey = (orthogonal << 4) | (diagonal & DiagonalBitMasks[orthogonal]);

        //    return SpriteSheetLookup[lookupKey];
        //}
    }
}
