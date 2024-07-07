using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using static UnityEngine.UI.GridLayoutGroup;

public class King : MoveablePiece
{
    public bool isFirstMove = true;

    void Start()
    {
        UpgradeManager.current.ActivateKingUpgrade += KingUpgrade;
    }

    public override bool Move(int newXPos, int newYPos)
    {
        if(isFirstMove)
        {
            List<Rook> rooks = board.allPieces.Values.Where(x => x.GetComponent<Rook>() != null).Cast<Rook>().ToList();
            foreach (Rook rook in rooks)
            {
                if (rook.isFirstMove && rook.xPos < xPos && !board.allPieces.ContainsKey((xPos - 2, yPos)))
                {
                    // queenside castle
                    rook.Move(newXPos + 1, newYPos);
                }
                else if (rook.isFirstMove && rook.xPos > xPos && !board.allPieces.ContainsKey((xPos + 2, yPos)))
                {
                    // kingside castle
                    rook.Move(newXPos - 1, newYPos);
                }
            }
        }
        isFirstMove = false;
        return base.Move(newXPos, newYPos);
    }

    void KingUpgrade(int id)
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
        // Give all the possible places that the king can move to.
        List<BoardTile> result = new List<BoardTile>();
        BoardTile tile;
        for (int xCounter = -1; xCounter <= 1; xCounter++)
        {
            for (int yCounter = -1; yCounter <= 1; yCounter++)
            {
                if (xCounter == 0 && yCounter == 0)
                {
                    continue;
                }

                // If you are in the 1 square radius around the king.
                if (board.theBoard.TryGetValue((xPos + xCounter, yPos + yCounter), out tile) && !board.allPieces.ContainsKey((xPos + xCounter, yPos + yCounter)))
                {
                    result.Add(tile);
                }
            }
        }
        // The check for castling
        if (isFirstMove)
        {
            List<Rook> rooks = board.allPieces.Values.Where(x => x.GetComponent<Rook>() != null).Cast<Rook>().ToList();
            foreach (Rook rook in rooks)
            {
                if (rook.isFirstMove && rook.xPos < xPos && !board.allPieces.ContainsKey((xPos - 2, yPos)))
                {
                    // queenside castle
                    board.theBoard.TryGetValue((xPos - 2, yPos), out tile);
                    result.Add(tile);
                }
                else if (rook.isFirstMove && rook.xPos > xPos && !board.allPieces.ContainsKey((xPos + 2, yPos)))
                {
                    // kingside castle
                    board.theBoard.TryGetValue((xPos + 2, yPos), out tile);
                    result.Add(tile);
                }
            }
        }
        return result;
    }

    public override List<BoardTile> PreviewAttack()
    {
        // Give all the possible places that the king can attack to.
        List<BoardTile> result = new List<BoardTile>();
        BoardTile tile;
        MoveablePiece enemyPiece;
        for (int xCounter = -1; xCounter <= 1; xCounter++)
        {
            for (int yCounter = -1; yCounter <= 1; yCounter++)
            {
                if (xCounter == 0 && yCounter == 0)
                {
                    continue;
                }

                // If you are in the 1 square radius around the king.
                if (board.allPieces.TryGetValue((xPos + xCounter, yPos + yCounter), out enemyPiece) && enemyPiece.owner != owner)
                {
                    board.theBoard.TryGetValue((xPos + xCounter, yPos + yCounter), out tile);
                    result.Add(tile);
                }
            }
        }
        return result;
    }
}
