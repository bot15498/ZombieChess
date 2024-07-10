using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using static UnityEngine.UI.GridLayoutGroup;

public class King : MoveablePiece
{
    public bool isFirstMove = true;

    [SerializeField]
    private bool queenSynergy = false;
    [SerializeField]
    private float queenTeleportJumpHeight = 100f;
    [SerializeField]
    private List<BoardTile> queenSynergyTiles = new List<BoardTile>();

    void Start()
    {
        UpgradeManager.current.ActivateKingUpgrade += KingUpgrade;
        BoardStateManager.current.TurnStartAction += OnturnStart;
    }

    void KingUpgrade(int id)
    {
        switch (id)
        {
            case 0:
                //upgrade stuff goes here upgrade id 0
                Debug.Log("AAAAAAAA");
                break;
            case 4:
                queenSynergy = true;
                break;
        }
    }

    private void OnturnStart(int turnCount)
    {
        
    }

    public override bool Move(int newXPos, int newYPos)
    {
        if (isFirstMove)
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

        // Teleporting to the queen
        BoardTile targetTile;
        if(queenSynergy && board.theBoard.TryGetValue((newXPos, newYPos), out targetTile) && queenSynergyTiles.Contains(targetTile))
        {
            // do a jump instead of a normal move
            List<BoardTile> placesToMove;
            if (!MovePaths.TryGetValue(targetTile, out placesToMove))
            {
                placesToMove = new List<BoardTile> { targetTile };
            }
            StartCoroutine(DoPieceJumpMovement(placesToMove, queenTeleportJumpHeight, 0f, 0f));
            return true;
        }
        else
        {
            return base.Move(newXPos, newYPos);
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

        if (queenSynergy)
        {
            queenSynergyTiles.Clear();
            // add all possible spaces to move around the queen.
            List<MoveablePiece> queens = board.allPieces.Values.Where(x => x.GetComponent<Queen>() != null).ToList();
            foreach (MoveablePiece queen in queens)
            {
                for (int xdiff = -1; xdiff <= 1; xdiff++)
                {
                    for (int ydiff = -1; ydiff <= 1; ydiff++)
                    {
                        if(!board.allPieces.ContainsKey((queen.xPos + xdiff, queen.yPos + ydiff)) && board.theBoard.TryGetValue((queen.xPos + xdiff, queen.yPos + ydiff), out tile))
                        {
                            // There is not a piece on this tile and the tile exists
                            result.Add(tile);
                            // You better believe I'm doing this a stupid way
                            queenSynergyTiles.Add(tile);
                        }
                    }
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

        if (queenSynergy)
        {
            // add all possible spaces to move around the queen.
            List<MoveablePiece> queens = board.allPieces.Values.Where(x => x.GetComponent<Queen>() != null).ToList();
            foreach (MoveablePiece queen in queens)
            {
                for (int xdiff = -1; xdiff <= 1; xdiff++)
                {
                    for (int ydiff = -1; ydiff <= 1; ydiff++)
                    {
                        if (board.allPieces.TryGetValue((queen.xPos + xdiff, queen.yPos + ydiff), out enemyPiece) 
                            && enemyPiece.owner != owner
                            && board.theBoard.TryGetValue((queen.xPos + xdiff, queen.yPos + ydiff), out tile))
                        {
                            // There is an enemy piece on this tile and the tile exists
                            result.Add(tile);
                            // You better believe I'm doing this a stupid way
                            queenSynergyTiles.Add(tile);
                        }
                    }
                }
            }
        }
        return result;
    }

    public override bool Die()
    {
        // the king just died!!!!
        BoardStateManager.current.Lose();
        return base.Die();
    }
}
