using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Knight : MoveablePiece
{

    void Start()
    {
        UpgradeManager.current.ActivateKnightUpgrade += KnightUpgrade;
    }

    void Update()
    {

    }
    void KnightUpgrade(int id)
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
        // Give all the possible places that the sam knight can move to.
        List<BoardTile> result = GetAllValidMoveTiles();
        return result.Where(x => !board.allPieces.ContainsKey((x.xCoord, x.yCoord))).ToList();
    }

    public override List<BoardTile> PreviewAttack()
    {
        // Give all the possible places that the knighjt can move to.
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
        if (board.theBoard.TryGetValue((xPos - 2, yPos + 1), out tile) && tile.canBeOccupied) { result.Add(tile); }
        if (board.theBoard.TryGetValue((xPos - 1, yPos + 2), out tile) && tile.canBeOccupied) { result.Add(tile); }
        if (board.theBoard.TryGetValue((xPos + 1, yPos + 2), out tile) && tile.canBeOccupied) { result.Add(tile); }
        if (board.theBoard.TryGetValue((xPos + 2, yPos + 1), out tile) && tile.canBeOccupied) { result.Add(tile); }
        if (board.theBoard.TryGetValue((xPos + 2, yPos - 1), out tile) && tile.canBeOccupied) { result.Add(tile); }
        if (board.theBoard.TryGetValue((xPos + 1, yPos - 2), out tile) && tile.canBeOccupied) { result.Add(tile); }
        if (board.theBoard.TryGetValue((xPos - 1, yPos - 2), out tile) && tile.canBeOccupied) { result.Add(tile); }
        if (board.theBoard.TryGetValue((xPos - 2, yPos - 1), out tile) && tile.canBeOccupied) { result.Add(tile); }

        return result;
    }
}
