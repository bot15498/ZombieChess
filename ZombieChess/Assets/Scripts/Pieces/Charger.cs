using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Charger : MoveablePiece, IZombiePiece
{
    [SerializeField]
    private int waitTimer = 3;

    private bool grappling = false;
    private MoveablePiece grappleTarget;

    public override List<BoardTile> PreviewMove()
    {
        throw new System.NotImplementedException();
    }

    public override List<BoardTile> PreviewAttack()
    {
        throw new System.NotImplementedException();
    }

    private BoardTile GetValidMoveTile()
    {
        // Similar to rook, but the only valid move tile is the one furthest to the south
        List<BoardTile> result = new List<BoardTile>();
        BoardTile tile;
        MoveablePiece enemyPiece;

        for (int i = yPos - 1; i >= board.minYPos; i--)
        {
            if (board.theBoard.TryGetValue((xPos, i), out tile) && tile.canBeOccupied) { result.Add(tile); }
            if (board.allPieces.TryGetValue((tile.xCoord, tile.yCoord), out enemyPiece) && enemyPiece.owner != owner)
            {
                // Enemy piece found
                break;
            }
        }

        // return only southmost tile
        result.RemoveRange(0, result.Count - 1);

        return result[0];
    }

    public void ZombieAiAction()
    {
        if (waitTimer > 0)
        {
            waitTimer -= 1;
        }
        else
        {
            if (!grappling)
            {
                BoardTile validMove = this.GetValidMoveTile();
                MoveablePiece enemyPiece;
                if (board.allPieces.TryGetValue((validMove.xCoord, validMove.yCoord), out enemyPiece) && enemyPiece.owner != owner)
                {
                    // Enemy sighted

                    // TODO: kill each zombie on the way

                    // Move to in front of target
                    this.Move(this.xPos, validMove.yCoord + 1);

                    // Grapple the target
                    grappling = true;
                    grappleTarget = enemyPiece;
                    // TODO: set enemy piece action points to 0
                }
                else
                {
                    // No Attacks, so move to the bottom of the board
                    this.Move(this.xPos, validMove.yCoord);
                }
            }
            else
            {
                // time to attack
                this.Attack(grappleTarget.xPos, grappleTarget.yPos);
            }
            
        }

    }

    void Start()
    {
        
    }
}
