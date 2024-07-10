using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UI.GridLayoutGroup;

public class Pawn : MoveablePiece
{
    [SerializeField]
    private bool isFirstMove = true;

    private bool canBackwardsMove = false;
    private bool canAttackForwards = false;
    private bool canChainKill = false;
    private bool explodeBoardOnPromotion = false;
    private bool canEnPassant = false;

    void Start()
    {
        UpgradeManager.current.ActivatePawnUpgrade += pawnUpgrade;
    }

    // Move backwards
    // attack forwards
    // Checkers chain hopping killing
    // Getting to the other side of the board clears the board kills the unit
    // EN PASSANT - can kill things by moving behind it
    // Pawns no longer controllable but autopilot like shambler

    void pawnUpgrade(int id)
    {
        switch (id)
        {
            // Backwards move
            case 0:
                this.canBackwardsMove = true;
                break;
            
            // Can Attack Forwards
            case 1:
                this.canAttackForwards = true;
                break;

            // Checkers chain kill
            case 2:
                this.canChainKill = true;
                break;

            // Promotion clears board
            case 3:
                this.explodeBoardOnPromotion = true;
                break;
            
            // En Passant at any time
            case 4:
                this.canEnPassant = true;
                break;

            // Is now a shambler
            case 5:
                break;

            default:
                break;
        }
    }

    public override bool Move(int newXPos, int newYPos)
    {
        isFirstMove = false;
        bool pieceMoved = base.Move(newXPos, newYPos);
        if (newYPos == board.maxYPos && explodeBoardOnPromotion)
        {
            this.Die();
            List<MoveablePiece> zomPieces = board.allPieces.Values.Where(x => x.owner != this.owner).ToList();
            foreach (MoveablePiece zom in zomPieces)
            {
                zom.Die();
            }
            return pieceMoved;
        }
        else
        {
            return pieceMoved;
        }
    }

    public override bool Attack(int targetXPos, int targetYPos)
    {
        // Do damage
        MoveablePiece enemy;
        BoardTile tile;
        if (board.allPieces.TryGetValue((targetXPos, targetYPos), out enemy) && enemy.owner != owner)
        {
            enemy.health--;
            if (enemy.health <= 0)
            {
                enemy.Die();
                if (this.canChainKill)
                {
                    this.numActions += 1;
                }
                
            }
        }
        else if (this.canEnPassant && board.theBoard.TryGetValue((targetXPos, targetYPos), out tile) && tile.canBeOccupied
                 && board.allPieces.TryGetValue((targetXPos, targetYPos - 1), out enemy) && enemy.owner != owner)
        {
            enemy.health--;
            if (enemy.health <= 0)
            {
                enemy.Die();
                if (this.canChainKill)
                {
                    this.numActions++;
                }
            }
        }
        // If we are normal attacking, and we defeat the enemy, then also do a move.
        if (!board.allPieces.ContainsKey((targetXPos, targetYPos)))
        {
            Move(targetXPos, targetYPos);
        }

        return true;
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
        if (board.theBoard.TryGetValue((xPos, yPos - 1), out tile) && tile.canBeOccupied && !board.allPieces.ContainsKey((xPos, yPos + 1)) && this.canBackwardsMove)
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
        if (this.canAttackForwards && board.allPieces.TryGetValue((xPos, yPos + 1), out enemyPiece) && enemyPiece.owner != owner)
        {
            board.theBoard.TryGetValue((xPos, yPos + 1), out tile);
            result.Add(tile);
        }
        if (this.canEnPassant)
        {
            if (!board.allPieces.TryGetValue((xPos - 1, yPos + 1), out enemyPiece) && 
                board.allPieces.TryGetValue((xPos - 1, yPos), out enemyPiece) && enemyPiece.owner != owner)
            {
                board.theBoard.TryGetValue((xPos - 1, yPos + 1), out tile);
                result.Add(tile);
            }
            if (!board.allPieces.TryGetValue((xPos + 1, yPos + 1), out enemyPiece) &&
                board.allPieces.TryGetValue((xPos + 1, yPos), out enemyPiece) && enemyPiece.owner != owner)
            {
                board.theBoard.TryGetValue((xPos + 1, yPos + 1), out tile);
                result.Add(tile);
            }
        }
        return result;
    }
}
