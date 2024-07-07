using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hunter : MoveablePiece, IZombiePiece
{
    public int pounceCharge = 1;
    public int jumpRadius = 3;
    private int currPounceCharge = 0;
    private BoardTile target = null;
    public override List<BoardTile> PreviewAttack()
    {
        List<BoardTile> validMoves = GetAllValidMoveTiles();
        List<BoardTile> reuslt = new List<BoardTile>();
        MoveablePiece enemy;
        foreach (BoardTile tile in validMoves)
        {
            if(board.allPieces.TryGetValue((tile.xCoord, tile.yCoord), out enemy) && enemy.owner != owner)
            {
                reuslt.Add(tile);
            }
        }

        return reuslt;
    }

    public override List<BoardTile> PreviewMove()
    {
        List<BoardTile> result = GetAllValidMoveTiles();
        return result.Where(x => !board.allPieces.ContainsKey((x.xCoord, x.yCoord))).ToList();
    }

    public void ZombieAiAction()
    {
        // If you see something that can be attacked, start to charge. If it's still there after charge, then attack it.
        List<BoardTile> attacks = PreviewAttack();
        List<BoardTile> moves = PreviewMove();

        if (target != null && !attacks.Contains(target))
        {
            // No longer see target. Going to clear out and do something else this turn
            target = null;
        }

        if (target == null && attacks.Count > 0)
        {
            // Somethig is in range, and we don't have a target. Set it as the target and start the coutdown
            int idx = Random.Range(0, attacks.Count);
            target = attacks[idx];
            currPounceCharge = 0;
        }
        else if (target == null && moves.Count > 0)
        {
            // Nothing is in range, and we don't have a t arget. But we can move. So pick a place and move. 
            // Lets move randomly for now for increased chaos
            int idx = Random.Range(0, moves.Count);
            BoardTile targetTile = moves[idx];
            Move(targetTile.xCoord, targetTile.yCoord);
        }
        else if(target != null && attacks.Contains(target) && currPounceCharge < pounceCharge)
        {
            // Still charging
            currPounceCharge++;
        }
        else if (target != null && attacks.Contains(target))
        {
            // time to kill
            Attack(target.xCoord, target.yCoord);
        }
        // Other wise, you have no moves you can do
    }

    private List<BoardTile> GetAllValidMoveTiles()
    {
        // Hunters can only move in a 3 square radius of itself.
        List<BoardTile> result = new List<BoardTile>();

        BoardTile tile;
        for (int xCoord = -jumpRadius; xCoord < jumpRadius; xCoord++)
        {
            int yCoord = jumpRadius - Mathf.Abs(xCoord);
            if (board.theBoard.TryGetValue((xCoord + xPos, yPos + yCoord), out tile))
            {
                result.Add(tile);
            }
            if (board.theBoard.TryGetValue((xCoord + xPos, yPos - yCoord), out tile))
            {
                result.Add(tile);
            }
        }
        return result;
    }

    // Start is called before the first frame update
    void Start()
    {

    }
}
