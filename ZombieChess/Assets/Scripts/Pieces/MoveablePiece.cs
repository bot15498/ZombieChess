using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveablePiece: MonoBehaviour
{
    public int health { get; set; } = 1;
    public int maxHealth { get; set; } = 1;
    public int numActions { get; set; } = 1;
    public int maxNumActions { get; set; } = 1;
    public int xPos { get; set; } = 0;
    public int yPos { get; set; } = 0;
    public bool canAct { get; set; } = true;
    public int size { get; set; } = 1;
    protected bool justKilledKing = false;
    public CurrentTurn owner { get; set; }
    protected Board board { get; set; }
    public abstract List<BoardTile> PreviewMove();
    public abstract List<BoardTile> PreviewAttack();

    public virtual bool Move(int newXPos, int newYPos)
    {
        // Update the board with the new place we are at 
        board.allPieces.Remove((xPos, yPos));
        xPos = newXPos;
        yPos = newYPos;
        board.allPieces.Add((xPos, yPos), this);

        // move the actual thing
        return board.MovePiece(gameObject, newXPos, newYPos);
    }

    public virtual bool Spawn(Board board, int xPos, int yPos, CurrentTurn owner)
    {
        this.board = board;
        this.xPos = xPos;
        this.yPos = yPos;
        this.owner = owner;
        return true;
    }
    public virtual bool Attack(int targetXPos, int targetYPos)
    {
        // Do damage
        MoveablePiece enemy;
        if (board.allPieces.TryGetValue((targetXPos, targetYPos), out enemy) && enemy.owner != owner)
        {
            enemy.health--;
            if (enemy.health <= 0)
            {
                justKilledKing = enemy.GetComponent<King>() != null;
                enemy.Die();
            }
        }

        // If we are normal attacking, and we defeat the enemy, then also do a move.
        if (!board.allPieces.ContainsKey((targetXPos, targetYPos)))
        {
            Move(targetXPos, targetYPos);
        }

        return true;
    }
    public virtual bool Die()
    {
        // delete yourself from the board
        board.allPieces.Remove((xPos, yPos));
        // delete yourself from existence
        Destroy(gameObject);
        return true;
    }
    public int ManDistance(MoveablePiece other)
    {
        // Returns the manhattan distance between this piece and another
        return Mathf.Abs(other.xPos - xPos) + Mathf.Abs(other.yPos - yPos);
    }
    public int ManDistance(BoardTile tile)
    {
        // Returns the manhattan distance between this piece and another tile
        return Mathf.Abs(tile.xCoord - xPos) + Mathf.Abs(tile.yCoord - yPos);
    }
    public bool LoseCheck()
    {
        // Checks to see if the player has lost the game
        //If you are a zombie and are at minYPos, then you lose
        if(owner == CurrentTurn.Zombie && yPos == board.minYPos)
        {
            return true;
        }
        if(owner == CurrentTurn.Zombie && justKilledKing)
        {
            return true;
        }

        return false;
    }
}
