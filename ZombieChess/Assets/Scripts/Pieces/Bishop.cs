using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Bishop : MoveablePiece
{
    [SerializeField]
    private bool ismartyr = false;
    [SerializeField]
    private bool hasRookMovement = false;
    [SerializeField]
    private bool piercing = false;
    [SerializeField]
    private int numRicochetCount = 0;

    void Start()
    {
        UpgradeManager.current.ActivateBishopUpgrade += BishopUpgrade;
    }

    void BishopUpgrade(int id)
    {
        switch (id)
        {
            case 0:
                //upgrade stuff goes here upgrade id 0
                Debug.Log("AAAAAAAA");
                break;

            case 1:
                // kill enemies it passes through
                piercing = true;
                break;
            case 2:
                // Can "ricochet" off walls for movement
                numRicochetCount++;
                break;
            case 3:
                // Can "ricochet" off walls for more movement
                numRicochetCount++;
                break;
            case 4:
                // Basically turns the bishop into a queen
                hasRookMovement = true;
                break;
            case 5:
                // martyrdom - when this unit dies, delete all enemies on same color tile
                ismartyr = true;
                break;
        }
    }

    public override bool Die()
    {
        if (ismartyr)
        {
            StartCoroutine(DieSlowly());
            return true;
        }
        else
        {
            return base.Die();
        }
    }

    private IEnumerator DieSlowly()
    {
        // A delayed death
        while (board.objectsMoving.Count > 0)
        {
            yield return new WaitForFixedUpdate();
        }
        // 0,0 is always a black tile.
        bool isOnWhiteTile = IsWhiteTile(xPos, yPos);
        MoveablePiece piece;
        for (int x = board.minXPos; x <= board.maxXPos; x++)
        {
            for (int y = board.minYPos; y <= board.maxYPos; y++)
            {
                if (isOnWhiteTile && IsWhiteTile(x, y) && board.allPieces.TryGetValue((x, y), out piece) && piece.owner != owner)
                {
                    piece.Die();
                }
                else if (!isOnWhiteTile && !IsWhiteTile(x, y) && board.allPieces.TryGetValue((x, y), out piece) && piece.owner != owner)
                {
                    piece.Die();
                }
            }
        }
        board.boardAudioController.PlayOneShot(board.playerBishopMartyr, 1.0f);

        // delete yourself from existence
        base.Die();
        yield return null;
    }

    private bool IsWhiteTile(int x, int y)
    {
        return (Mathf.Abs(x % 2) == 1 && Mathf.Abs(y % 2) == 0) || (Mathf.Abs(x % 2) == 0 && Mathf.Abs(y % 2) == 1);
    }

    public override List<BoardTile> PreviewMove()
    {
        // filter out the attacks
        return GetAllMovesAndAttacks().Where(x => !board.allPieces.ContainsKey((x.xCoord, x.yCoord))).ToList();
    }

    public override List<BoardTile> PreviewAttack()
    {
        // Give all the possible places that the bishop can attack to.
        List<BoardTile> result = new List<BoardTile>();
        MoveablePiece enemyPiece;
        foreach (BoardTile tile in GetAllMovesAndAttacks())
        {
            if (board.allPieces.TryGetValue((tile.xCoord, tile.yCoord), out enemyPiece) && enemyPiece.owner != owner)
            {
                result.Add(tile);
            }
        }
        return result;
    }

    private List<BoardTile> GetAllMovesAndAttacks()
    {
        // Give all the possible places that the bishop can move to.
        List<BoardTile> result = GetAllValidMoveTiles(xPos, yPos);
        if (numRicochetCount > 0)
        {
            // Start to build the move paths dictionary
            MovePaths.Clear();
            foreach (BoardTile tile in result)
            {
                MovePaths.Add(tile, new List<BoardTile> { tile });
            }

            // I wrote this at like 3am I have no idea if it works.
            List<BoardTile> tilesToLookAt = result;
            for (int i = 0; i < numRicochetCount; i++)
            {
                foreach (BoardTile tile in GetBorderTiles(tilesToLookAt))
                {
                    List<BoardTile> bounceTiles = GetAllValidMoveTiles(tile.xCoord, tile.yCoord);
                    foreach (BoardTile currBounceTile in bounceTiles)
                    {
                        if (!MovePaths.ContainsKey(currBounceTile))
                        {
                            List<BoardTile> toad = new List<BoardTile>();
                            toad.AddRange(MovePaths[tile]);
                            toad.Add(currBounceTile);
                            MovePaths.Add(currBounceTile, toad);
                        }
                    }
                    tilesToLookAt.AddRange(bounceTiles);
                }
            }
        }
        return result;
    }

    private List<BoardTile> GetBorderTiles(ICollection<BoardTile> intiles)
    {
        List<BoardTile> result = new List<BoardTile>();
        foreach(BoardTile tile in intiles)
        {
            // check if at the border or not
            if(tile.xCoord == board.maxXPos
                || tile.xCoord == board.minXPos
                || tile.yCoord == board.maxYPos
                || tile.yCoord == board.minYPos)
            {
                result.Add(tile);
            }
            // Check if we are going to hit a friendly piece if we go any further
            // TODO: this
        }

        return result;
    }

    public override bool Move(int newXPos, int newYPos)
    {
        if (piercing)
        {
            AttackTiles.Clear();
            // add all the tiles that the bishop is going to move on. 
            BoardTile lastTile;
            board.theBoard.TryGetValue((xPos, yPos), out lastTile);
            BoardTile endTile;
            board.theBoard.TryGetValue((newXPos, newYPos), out endTile);

            // Check for how the bishop is going to get to where it's going.
            List<BoardTile> currMovePaths;
            if (!MovePaths.ContainsKey(endTile))
            {
                currMovePaths = new List<BoardTile> { endTile };
            }
            else
            {
                currMovePaths = MovePaths[endTile];
            }

            // Do a very dumb way to figure out what tile are in the kill line.
            foreach (BoardTile nextTile in currMovePaths)
            {
                int xdir = nextTile.xCoord - lastTile.xCoord < 0 ? -1 : nextTile.xCoord - lastTile.xCoord > 0 ? 1 : 0;
                int ydir = nextTile.yCoord - lastTile.yCoord < 0 ? -1 : nextTile.yCoord - lastTile.yCoord > 0 ? 1 : 0;
                // dangerous!
                int currX = lastTile.xCoord;
                int currY = lastTile.yCoord;
                BoardTile tileToadd;
                while (currX != nextTile.xCoord || currY != nextTile.yCoord)
                {
                    currX += xdir;
                    currY += ydir;
                    if (board.theBoard.TryGetValue((currX, currY), out tileToadd))
                    {
                        AttackTiles.Add(tileToadd);
                    }
                }
                // Set the last tile reference
                lastTile = nextTile;
            }
        }
        return base.Move(newXPos, newYPos);
    }

    private List<BoardTile> GetAllValidMoveTiles(int currXPos, int currYPos)
    {
        // This returns all valid move tiles, regardless if something is there or not
        List<BoardTile> result = new List<BoardTile>();
        BoardTile tile;
        MoveablePiece piece;

        int northEastMaxCheck = Mathf.Min(board.maxXPos - currXPos, board.maxYPos - currYPos);
        int northWestMaxCheck = Mathf.Min(currXPos - board.minXPos, board.maxYPos - currYPos);
        int southEastMaxCheck = Mathf.Min(board.maxXPos - currXPos, currYPos - board.minYPos);
        int southWestMaxCheck = Mathf.Min(currXPos - board.minXPos, currYPos - board.minYPos);

        // Check going north east
        for (int i = 1; i <= northEastMaxCheck; i++)
        {
            if (board.theBoard.TryGetValue((currXPos + i, currYPos + i), out tile) && tile.canBeOccupied)
            {
                result.Add(tile);
            }
            if (board.allPieces.TryGetValue((currXPos + i, currYPos + i), out piece))
            {
                if (piece.owner == owner || !tile.canBeOccupied)
                {
                    // hit your own piece or an unusable tile, break out
                    break;
                }
                else if (!piercing)
                {
                    // You don't have piercing upgrade, so stop here
                    break;
                }
            }
        }
        // check going north weast
        for (int i = 1; i <= northWestMaxCheck; i++)
        {
            if (board.theBoard.TryGetValue((currXPos - i, currYPos + i), out tile) && tile.canBeOccupied)
            {
                result.Add(tile);
            }
            if (board.allPieces.TryGetValue((currXPos - i, currYPos + i), out piece))
            {
                if (piece.owner == owner || !tile.canBeOccupied)
                {
                    break;
                }
                else if (!piercing)
                {
                    break;
                }
            }
        }
        // check going south east
        for (int i = 1; i <= southEastMaxCheck; i++)
        {
            if (board.theBoard.TryGetValue((currXPos + i, currYPos - i), out tile) && tile.canBeOccupied)
            {
                result.Add(tile);
            }
            if (board.allPieces.TryGetValue((currXPos + i, currYPos - i), out piece))
            {
                if (piece.owner == owner || !tile.canBeOccupied)
                {
                    break;
                }
                else if (!piercing)
                {
                    break;
                }
            }
        }
        // check going south weast
        for (int i = 1; i <= southWestMaxCheck; i++)
        {
            if (board.theBoard.TryGetValue((currXPos - i, currYPos - i), out tile) && tile.canBeOccupied)
            {
                result.Add(tile);
            }
            if (board.allPieces.TryGetValue((currXPos - i, currYPos - i), out piece))
            {
                if (piece.owner == owner || !tile.canBeOccupied)
                {
                    break;
                }
                else if (!piercing)
                {
                    break;
                }
            }
        }

        if (hasRookMovement)
        {
            // Also add the rook behavior
            // north check
            for (int i = currYPos + 1; i <= board.maxYPos; i++)
            {
                if (board.theBoard.TryGetValue((currXPos, i), out tile) && tile.canBeOccupied) { result.Add(tile); }
                if ((board.allPieces.ContainsKey((currXPos, i)) && !piercing) || !tile.canBeOccupied)
                {
                    // Founda piece, so stop looking.
                    break;
                }
                if (board.allPieces.TryGetValue((currXPos, i), out piece))
                {
                    if (piece.owner == owner || !tile.canBeOccupied)
                    {
                        break;
                    }
                    else if (!piercing)
                    {
                        break;
                    }
                }
            }

            // south check
            for (int i = currYPos - 1; i >= board.minYPos; i--)
            {
                if (board.theBoard.TryGetValue((currXPos, i), out tile) && tile.canBeOccupied) { result.Add(tile); }
                if (board.allPieces.TryGetValue((currXPos, i), out piece))
                {
                    if (piece.owner == owner || !tile.canBeOccupied)
                    {
                        break;
                    }
                    else if (!piercing)
                    {
                        break;
                    }
                }
            }

            // weast check
            for (int i = currXPos - 1; i >= board.minXPos; i--)
            {
                if (board.theBoard.TryGetValue((i, currYPos), out tile) && tile.canBeOccupied) { result.Add(tile); }
                if (board.allPieces.TryGetValue((i, currYPos), out piece))
                {
                    if (piece.owner == owner || !tile.canBeOccupied)
                    {
                        break;
                    }
                    else if (!piercing)
                    {
                        break;
                    }
                }
            }

            // east check
            for (int i = currXPos + 1; i <= board.maxXPos; i++)
            {
                if (board.theBoard.TryGetValue((i, currYPos), out tile) && tile.canBeOccupied) { result.Add(tile); }
                if (board.allPieces.TryGetValue((i, currYPos), out piece))
                {
                    if (piece.owner == owner || !tile.canBeOccupied)
                    {
                        break;
                    }
                    else if (!piercing)
                    {
                        break;
                    }
                }
            }
        }
        return result;
    }
}
