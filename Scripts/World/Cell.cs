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
            UpdateCell();
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
                Bomb();
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
                    UpdateNBors();
                }
            }
            else {
                if (oreDamage > 0) {
                    fullyMined = true;
                    IsReadyForProgression = true;
                    oreDamage = -1;
                }
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
            UpdateNBors();
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
            int atlasX =     (~(top | left) & topLeft) + (~(top | right) & topRight) + (~(bottom | left) & bottomLeft) + (~(bottom | right) & bottomRight);
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
    }
}
