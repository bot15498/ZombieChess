using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Knight : MoveablePiece
{
    /*
     * 
     * Killing a piece allows it to jump again
After a kill immune to capture for a turn
More move range
Can have multiple upgrades
Extend vertical range
Extend horizontal range
Attacks all spaces next to itÅfs move path

     */

    private bool canChainKill = true;
    private bool immuneOnKill = true;

    private int baseLongRange = 2;
    private int baseShortRange = 1;
    private int maxLongRange = 2;
    private int maxShortRange = 1;
    
    private bool canPassbyKill = true;

    private static int[] signs = new int[] {-1, 1};

    void Start()
    {
        UpgradeManager.current.ActivateKnightUpgrade += KnightUpgrade;
    }

    void KnightUpgrade(int id)
    {
        switch (id)
        {
            // Can Chain kills
            case 0:
                this.canChainKill = true;
                break;

            // immune on kill
            case 1:
                this.immuneOnKill = true;
                break;

            // Extend vertical range
            case 2:
                this.maxLongRange++;
                break;

            // Extend horizontal range
            case 3:
                this.maxShortRange++;
                break;

            // Can kill when passing by
            case 4:
                this.canPassbyKill = true;
                break;

            default:
                break;
        }
    }

    public override bool Attack(int targetXPos, int targetYPos)
    {
        // Do damage
        MoveablePiece enemy;
        if (board.allPieces.TryGetValue((targetXPos, targetYPos), out enemy) && enemy.owner != owner)
        {
            enemy.health--;
            if (enemy.health <= 0)
            {
                if (canChainKill)
                {
                    this.numActions++;
                }
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

        for (int longSteps = this.baseLongRange; longSteps <= this.maxLongRange; longSteps++)
        {
            for (int shortSteps = this.baseShortRange; shortSteps <= this.maxShortRange; shortSteps++)
            {
                foreach (int longSign in signs)
                {
                    foreach (int shortSign in signs)
                    {
                        // Long step goes North or South - Short step goes East or West
                        if (board.theBoard.TryGetValue((xPos + shortSign * shortSteps, yPos + longSign * longSteps), out tile) && tile.canBeOccupied) { result.Add(tile); }
                        // Long step goes East or West - Short step goes North or South
                        if (board.theBoard.TryGetValue((xPos + longSign * longSteps, yPos + shortSign * shortSteps), out tile) && tile.canBeOccupied) { result.Add(tile); }
                    }
                }
            }
        }
        return result;
    }
}
