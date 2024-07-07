using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using static UnityEngine.UI.GridLayoutGroup;

public class Rook : MoveablePiece
{
    void Start()
    {
        UpgradeManager.current.ActivateRookUpgrade += RookUpgrade;
    }

    void RookUpgrade(int id)
    {
        switch (id)
        {
            case 0:
                //upgrade stuff goes here
                Debug.Log("AAAAAAAA");
                break;

            case 1:

                break;

            default:

                break;
        }
        //Do upgrade stuff
    }

    public override List<BoardTile> PreviewMove()
    {
        // Give all the possible places that the rook can move to.
        List<BoardTile> result = GetAllValidMoveTiles();
        return result.Where(x => !board.allPieces.ContainsKey((x.xCoord, x.yCoord))).ToList();
    }

    public override List<BoardTile> PreviewAttack()
    {
        // Give all the possible places that the rook can attack.
        List<BoardTile> result = new List<BoardTile>();
        MoveablePiece enemyPiece;
        foreach (BoardTile tile in GetAllValidMoveTiles())
        {
            if (board.allPieces.TryGetValue((tile.xCoord, tile.yCoord), out enemyPiece) && enemyPiece.owner != owner)
            {
                result.Add(tile);
            }
        }
        return result;
    }

    private List<BoardTile> GetAllValidMoveTiles()
    {
        // This returns all valid move tiles, regardless if something is there or not
        List<BoardTile> result = new List<BoardTile>();
        BoardTile tile;

        // north check
        for (int i = yPos + 1; i <= board.maxYPos; i++)
        {
            if (board.theBoard.TryGetValue((xPos, i), out tile) && tile.canBeOccupied) { result.Add(tile); }
            if (board.allPieces.ContainsKey((xPos, i)) || !tile.canBeOccupied)
            {
                // Founda piece, so stop looking.
                break;
            }
        }

        // south check
        for (int i = yPos - 1; i >= board.minYPos; i--)
        {
            if (board.theBoard.TryGetValue((xPos, i), out tile) && tile.canBeOccupied) { result.Add(tile); }
            if (board.allPieces.ContainsKey((xPos, i)) || !tile.canBeOccupied)
            {
                break;
            }
        }

        // weast check
        for (int i = xPos - 1; i >= board.minXPos; i--)
        {
            if (board.theBoard.TryGetValue((i, yPos), out tile) && tile.canBeOccupied) { result.Add(tile); }
            if (board.allPieces.ContainsKey((i, yPos)) || !tile.canBeOccupied)
            {
                break;
            }
        }

        // east check
        for (int i = xPos + 1; i <= board.maxXPos; i++)
        {
            if (board.theBoard.TryGetValue((i, yPos), out tile) && tile.canBeOccupied) { result.Add(tile); }
            if (board.allPieces.ContainsKey((i, yPos)) || !tile.canBeOccupied)
            {
                break;
            }
        }

        return result;
    }
}
