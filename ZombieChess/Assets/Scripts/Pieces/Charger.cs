using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Charger : MoveablePiece, IZombiePiece
{
    [SerializeField]
    private int initialWaitTimer = 3;
    private int waitTimer;

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

    private List<BoardTile> GetValidMovePath()
    {
        // Returns line of move tiles

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

        return result;
    }

    private BoardTile GetValidMoveTile()
    {
        // Similar to rook, but the only valid move tile is the one furthest to the south
        List<BoardTile> result = this.GetValidMovePath();
        if(result.Count == 0)
        {
            return null;
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
            BoardTile validMove = this.GetValidMoveTile();
            if (!grappling && validMove != null)
            {
                MoveablePiece enemyPiece;

                // Kill all zombies in the way
                foreach (BoardTile tile in this.GetValidMovePath())
                {
                    if (board.allPieces.TryGetValue((tile.xCoord, tile.yCoord), out enemyPiece) && enemyPiece.owner == owner)
                    {
                        // If it's a zombie, kill it
                        enemyPiece.Die();
                    }
                }

                if (board.allPieces.TryGetValue((validMove.xCoord, validMove.yCoord), out enemyPiece) && enemyPiece.owner != owner)
                {
                    // Enemy sighted

                    // Move to in front of target
                    this.Move(this.xPos, validMove.yCoord + 1);

                    // Grapple the target
                    this.grappling = true;
                    this.grappleTarget = enemyPiece;
                    this.grappleTarget.numActions = 0;
                    this.grappleTarget.maxNumActions = 0;
                }
                else
                {
                    // No Attacks, so move to the bottom of the board
                    this.Move(this.xPos, validMove.yCoord);
                }
            }
            else if(grappling)
            {
                // time to attack
                this.Attack(grappleTarget.xPos, grappleTarget.yPos);
            }
            
        }

    }

    public new bool Attack(int targetXPos, int targetYPos)
    {
        // Do damage
        MoveablePiece enemy;
        if (board.allPieces.TryGetValue((targetXPos, targetYPos), out enemy) && enemy.owner != owner)
        {
            enemy.health--;
            if (enemy.health <= 0)
            {
                enemy.Die();
            }
        }

        // If we kill, reset timer to charge
        if (!board.allPieces.ContainsKey((targetXPos, targetYPos)))
        {
            this.grappling = false;
            this.grappleTarget = null;
            this.waitTimer = this.initialWaitTimer;
        }

        return true;
    }

    public override bool Die()
    {
        if (grappling)
        {
            grappleTarget.maxNumActions = 1; // TODO: set to actual max num (from upgrades probably)
        }

        // delete yourself from the board
        board.allPieces.Remove((xPos, yPos));
        // delete yourself from existence
        Destroy(gameObject);
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.waitTimer = this.initialWaitTimer;
    }
}
