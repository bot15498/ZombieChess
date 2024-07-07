using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Bishop : MoveablePiece
{
    void Start()
    {
        UpgradeManager.current.ActivateBishopUpgrade += BishopUpgrade;
    }

    void Update()
    {

    }
    void BishopUpgrade(int id)
    {
        switch (id)
        {
            case 0:
                //upgrade stuff goes here upgrade id 0
                Debug.Log("AAAAAAAA");
                break;

            case 1:
                //upgrade stuff goes here upgrade id 1 
                break;

            default:

                break;
        }
    }

    public override List<BoardTile> PreviewMove()
    {
        // Give all the possible places that the bishop can move to.
        List<BoardTile> result = GetAllValidMoveTiles();
        return result.Where(x => !board.allPieces.ContainsKey((x.xCoord, x.yCoord))).ToList();
    }

    public override List<BoardTile> PreviewAttack()
    {
        // Give all the possible places that the bishop can attack to.
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

        int northEastMaxCheck = Mathf.Min(board.maxXPos - xPos, board.maxYPos - yPos);
        int northWestMaxCheck = Mathf.Min(xPos - board.minXPos, board.maxYPos - yPos);
        int southEastMaxCheck = Mathf.Min(board.maxXPos - xPos, yPos - board.minYPos);
        int southWestMaxCheck = Mathf.Min(xPos - board.minXPos, yPos - board.minYPos);

        // Check going north east
        for (int i = 1; i <= northEastMaxCheck; i++)
        {
            if (board.theBoard.TryGetValue((xPos + i, yPos + i), out tile) && tile.canBeOccupied)
            {
                result.Add(tile);
            }
            if (board.allPieces.ContainsKey((xPos + i, yPos + i)) || !tile.canBeOccupied)
            {
                // Hit a piece, break out
                break;
            }
        }
        // check going north weast
        for (int i = 1; i <= northWestMaxCheck; i++)
        {
            if (board.theBoard.TryGetValue((xPos - i, yPos + i), out tile) && tile.canBeOccupied)
            {
                result.Add(tile);
            }
            if (board.allPieces.ContainsKey((xPos - i, yPos + i)) || !tile.canBeOccupied)
            {
                break;
            }
        }
        // check going south east
        for (int i = 1; i <= southEastMaxCheck; i++)
        {
            if (board.theBoard.TryGetValue((xPos + i, yPos - i), out tile) && tile.canBeOccupied)
            {
                result.Add(tile);
            }
            if (board.allPieces.ContainsKey((xPos + i, yPos - i)) || !tile.canBeOccupied)
            {
                break;
            }
        }
        // check going south weast
        for (int i = 1; i <= southWestMaxCheck; i++)
        {
            if (board.theBoard.TryGetValue((xPos - i, yPos - i), out tile) && tile.canBeOccupied)
            {
                result.Add(tile);
            }
            if (board.allPieces.ContainsKey((xPos - i, yPos - i)) || !tile.canBeOccupied)
            {
                break;
            }
        }
        return result;
    }
}
