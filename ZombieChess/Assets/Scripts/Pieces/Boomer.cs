using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomer : MoveablePiece, IZombiePiece
{
    [SerializeField]
    private int alertRadius = 5;
    [SerializeField]
    private int turnsUntilExplode = 1;
    [SerializeField]
    private int explodeRadius = 1;
    [SerializeField]
    private bool isArmed = false;

    public override List<BoardTile> PreviewAttack()
    {
        // Boomers arm themselves, so the next turn they "explode" and then 
        List<BoardTile> result = new List<BoardTile>();
        BoardTile tile;
        if (board.theBoard.TryGetValue((xPos, yPos), out tile))
        {
            result.Add(tile);
        }
        return result;
    }

    public override List<BoardTile> PreviewMove()
    {
        // Boomers can only in 1 square radius
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
        return result;
    }

    public void ZombieAiAction()
    {
        if (isArmed)
        {
            turnsUntilExplode--;
            if (turnsUntilExplode < 0)
            {
                // time to explode
                int numShamblersToSpawn = Random.Range(2, 5);
                List<BoardTile> freespaces = FreeSpaces(explodeRadius);
                numShamblersToSpawn = Mathf.Min(numShamblersToSpawn, freespaces.Count);
                for (int i = 0; i < numShamblersToSpawn; i++)
                {
                    int spawnLocIdx = Random.Range(0, freespaces.Count);
                    board.PlacePiece(freespaces[spawnLocIdx].xCoord, freespaces[spawnLocIdx].yCoord, CurrentTurn.Zombie, BoardStateManager.current.shamblerPrefab);
                }
                Die();
            }
        }
        else
        {
            // See if there is an enemy within a x tile radius
            MoveablePiece closestEnemy = null;
            foreach (MoveablePiece piece in board.allPieces.Values)
            {
                if (piece.owner != owner && (closestEnemy == null || piece.ManDistance(this) < closestEnemy.ManDistance(this)))
                {
                    closestEnemy = piece;
                }
            }
            Debug.Log(closestEnemy);
            if (closestEnemy.ManDistance(this) <= alertRadius)
            {
                // arm myself
                isArmed = true;
            }
            else if (yPos <= (board.maxYPos - board.minYPos) / 2 + board.minYPos)
            {
                //If you get over half the board, 50/50 chance every time to move forward again or to arm yourself
                float roll = Random.Range(0, 1);
                if (roll >= 0.5)
                {
                    isArmed = true;
                }
            }
            else
            {
                // just move forward towards closest piece
                BoardTile target = null;
                foreach (BoardTile tile in PreviewMove())
                {
                    if (target == null || closestEnemy.ManDistance(tile) < closestEnemy.ManDistance(target))
                    {
                        target = tile;
                    }
                }
                Move(target.xCoord, target.yCoord);
            }
        }
    }

    private List<BoardTile> FreeSpaces(int radius)
    {
        // Figure out how many free spaces are around the current unit (not including self). So min is 1
        List<BoardTile> result = new List<BoardTile>();
        BoardTile tile;
        for (int x = xPos - radius; x <= xPos + radius; x++)
        {
            for (int y = yPos - radius; y <= yPos + radius; y++)
            {
                if(!board.allPieces.ContainsKey((x, y)) && board.theBoard.TryGetValue((x,y), out tile))
                {
                    result.Add(tile);
                }
            }
        }
        board.theBoard.TryGetValue((xPos, yPos), out tile);
        result.Add(tile);
        return result;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
