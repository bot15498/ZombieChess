using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveablePiece: MonoBehaviour
{
    public int health { get; set; }
    public int maxHealth { get; set; }
    public int numActions { get; set; }
    public int maxNumActions { get; set; }
    public int xPos { get; set; }
    public int yPos { get; set; }
    public bool canAct { get; set; }
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
        board.MovePiece(gameObject, newXPos, newYPos);

        return true;
    }
    public virtual bool Spawn(Board board, int xPos, int yPos, CurrentTurn owner)
    {
        this.board = board;
        this.xPos = xPos;
        this.yPos = yPos;
        this.owner = owner;
        return true;
    }
    public bool Attack(int targetXPos, int targetYPos)
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
}
