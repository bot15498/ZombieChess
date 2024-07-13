using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using static UnityEngine.UI.GridLayoutGroup;

public class Rook : MoveablePiece
{
    public bool isFirstMove = true;

    public bool isImmunteToShambler = false;

    [SerializeField]
    private bool hasAoeOnAttack = false;
    [SerializeField]
    private int aoeAttackSize = 1;

    [SerializeField]
    private bool hasColumnClear = false;
    [SerializeField]
    private int columnClearAdditionalWidth = 0;


    void Start()
    {
        UpgradeManager.current.ActivateRookUpgrade += RookUpgrade;
    }

    void RookUpgrade(int id)
    {
        switch (id)
        {
            case 0:
                // Makes it so the rook has AOE when taking a piece.
                hasAoeOnAttack = true;
                break;
            case 1:
                // Makes it so the rook can't move, but can clear out all pieces in a column
                hasColumnClear = true;
                break;
            case 2:
                // Make it 3 columns instead of 1
                hasColumnClear = true;
                columnClearAdditionalWidth = 1;
                break;
            case 3:
                isImmunteToShambler = true;
                break;
        }
    }

    public override bool Move(int newXPos, int newYPos)
    {
        isFirstMove = false;
        bool returnval = base.Move(newXPos, newYPos);
        if (hasAoeOnAttack)
        {
            StartCoroutine(AoeKill());
        }
        return returnval;
    }

    public override bool Attack(int targetXPos, int targetYPos)
    {
        if (hasColumnClear)
        {
            // Rook can't move anymore so don't move it.
            // Instead, kill everything that exists in an attack tile.
            // TODO maybe play an animation here of how the rook attacks?
            MoveablePiece enemy;
            foreach (BoardTile tile in AttackTiles)
            {
                if (board.allPieces.TryGetValue((tile.xCoord, tile.yCoord), out enemy) && enemy.owner != owner)
                {
                    enemy.Die();
                }
            }

            return true;
        }
        else
        {
            return base.Attack(targetXPos, targetYPos);
        }
    }

    public override List<BoardTile> PreviewMove()
    {
        // Give all the possible places that the rook can move to.
        if (hasColumnClear)
        {
            // Can't move anymore
            return new List<BoardTile>();
        }
        else
        {
            List<BoardTile> result = GetAllValidMoveTiles();
            return result.Where(x => !board.allPieces.ContainsKey((x.xCoord, x.yCoord))).ToList();
        }
    }

    public override List<BoardTile> PreviewAttack()
    {
        // Give all the possible places that the rook can attack.
        if (hasColumnClear)
        {
            // Can't move anymore, but highlight every tile in column as if you can attack there
            AttackTiles.Clear();
            BoardTile tile;
            for (int i = -columnClearAdditionalWidth; i <= columnClearAdditionalWidth; i++)
            {
                for (int y = board.minYPos; y <= board.maxYPos; y++)
                {
                    if (board.theBoard.TryGetValue((xPos + i, y), out tile))
                    {
                        AttackTiles.Add(tile);
                    }
                }

            }
            return AttackTiles;
        }
        else
        {
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
    }

    private List<BoardTile> GetAllValidMoveTiles()
    {
        // This returns all valid move tiles, regardless if something is there or not
        List<BoardTile> result = new List<BoardTile>();
        BoardTile tile;

        // north check
        for (int i = yPos + 1; i <= board.maxYPos; i++)
        {
            if (board.theBoard.TryGetValue((xPos, i), out tile) && tile.canBeOccupied) { result.Add(tile); }
            if (board.allPieces.ContainsKey((xPos, i)) || !tile.canBeOccupied)
            {
                // Founda piece, so stop looking.
                break;
            }
        }

        // south check
        for (int i = yPos - 1; i >= board.minYPos; i--)
        {
            if (board.theBoard.TryGetValue((xPos, i), out tile) && tile.canBeOccupied) { result.Add(tile); }
            if (board.allPieces.ContainsKey((xPos, i)) || !tile.canBeOccupied)
            {
                break;
            }
        }

        // weast check
        for (int i = xPos - 1; i >= board.minXPos; i--)
        {
            if (board.theBoard.TryGetValue((i, yPos), out tile) && tile.canBeOccupied) { result.Add(tile); }
            if (board.allPieces.ContainsKey((i, yPos)) || !tile.canBeOccupied)
            {
                break;
            }
        }

        // east check
        for (int i = xPos + 1; i <= board.maxXPos; i++)
        {
            if (board.theBoard.TryGetValue((i, yPos), out tile) && tile.canBeOccupied) { result.Add(tile); }
            if (board.allPieces.ContainsKey((i, yPos)) || !tile.canBeOccupied)
            {
                break;
            }
        }

        return result;
    }

    private IEnumerator AoeKill()
    {
        yield return new WaitForSeconds(0.1f);
        while (board.objectsMoving.Contains(this))
        {
            // wait for movement to finish
            yield return new WaitForSeconds(0.1f);
        }

        // Now kill everything in a 1 tile aoe
        MoveablePiece enemy;
        for (int xdiff = -aoeAttackSize; xdiff <= aoeAttackSize; xdiff++)
        {
            for (int ydiff = -aoeAttackSize; ydiff <= aoeAttackSize; ydiff++)
            {
                if (board.allPieces.TryGetValue((xPos + xdiff, yPos + ydiff), out enemy) && enemy.owner != owner)
                {
                    enemy.Die();
                }
            }
        }
        yield return null;
    }
}
