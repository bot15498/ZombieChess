using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UI.GridLayoutGroup;

public class Pawn : MoveablePiece
{
    [SerializeField]
    private bool isFirstMove = true;

    void Start()
    {
        UpgradeManager.current.ActivatePawnUpgrade += pawnUpgrade;
    }

    void Update()
    {

    }

    void pawnUpgrade(int id)
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
        // Give all the possible places that the pawn can move to.
        List<BoardTile> result = new List<BoardTile>();
        BoardTile tile;
        if (board.theBoard.TryGetValue((xPos, yPos + 1), out tile) && tile.canBeOccupied && !board.allPieces.ContainsKey((xPos, yPos + 1)))
        {
            result.Add(tile);
        }
        if (isFirstMove && board.theBoard.TryGetValue((xPos, yPos + 2), out tile) 
            && tile.canBeOccupied && !board.allPieces.ContainsKey((xPos, yPos + 2))
            && !board.allPieces.ContainsKey((xPos, yPos + 1)))
        {
            result.Add(tile);
        }
        return result;
    }

    public override List<BoardTile> PreviewAttack()
    {
        // Give all the possible places that the pawn can move to.
        List<BoardTile> result = new List<BoardTile>();
        BoardTile tile;
        MoveablePiece enemyPiece;
        if (board.allPieces.TryGetValue((xPos - 1, yPos + 1), out enemyPiece) && enemyPiece.owner != owner)
        {
            board.theBoard.TryGetValue((xPos - 1, yPos + 1), out tile);
            result.Add(tile);
        }
        if (board.allPieces.TryGetValue((xPos + 1, yPos + 1), out enemyPiece) && enemyPiece.owner != owner)
        {
            board.theBoard.TryGetValue((xPos + 1, yPos + 1), out tile);
            result.Add(tile);
        }
        return result;
    }
}
