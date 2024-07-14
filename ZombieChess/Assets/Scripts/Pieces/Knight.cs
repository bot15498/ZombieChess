using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Knight : MoveablePiece
{
    private static int[] signs = new int[] { -1, 1 };

    private int baseLongRange = 2;
    private int baseShortRange = 1;

    [SerializeField]
    private bool canChainKill = false;

    [SerializeField]
    private bool immuneOnKill = false;

    [SerializeField]
    private int maxLongRange = 2;

    [SerializeField]
    private int maxShortRange = 1;

    [SerializeField]
    private bool canPassbyKill = false;

    [SerializeField]
    private float waitAfterStop = 0.5f;

    [SerializeField]
    private float killInterWait = 0.1f;

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

            //
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

    public override bool Move(int newXPos, int newYPos)
    {
        bool yLong = (Mathf.Abs(newYPos - this.yPos) > Mathf.Abs(newXPos - this.xPos));
        int oldYPos = this.yPos;
        int oldXPos = this.xPos;
        int longSign;
        int shortSign;
        if (yLong)
        {
            longSign = newYPos - this.yPos > 0 ? 1 : -1;
            shortSign = newXPos - this.xPos > 0 ? 1 : -1;
        }
        else
        {
            longSign = newXPos - this.xPos > 0 ? 1 : -1;
            shortSign = newYPos - this.yPos > 0 ? 1 : -1;
        }
        //Debug.Log(yLong.ToString() + " " + longSign.ToString() + " " + shortSign.ToString() + " (" + oldYPos.ToString() + " " + oldXPos.ToString() + ") to (" + newYPos.ToString() + " " + newXPos.ToString() + ") ");


        if (base.Move(newXPos, newYPos))
        {
            if(this.canPassbyKill) // TODO: make knight zigzag between all the enemies
            {
                List<BoardTile> fullPathTiles = new List<BoardTile>();
                BoardTile longStepIntermediate;
                BoardTile shortStepIntermediate;

                if (yLong)
                {
                    
                    for (int stepsToLong = oldYPos + longSign; newYPos - stepsToLong != 0; stepsToLong += longSign)
                    {
                        board.theBoard.TryGetValue((oldXPos, stepsToLong), out longStepIntermediate);
                        fullPathTiles.Add(longStepIntermediate);
                    }
                    for (int stepsToShort = oldXPos + shortSign; newXPos - stepsToShort != 0; stepsToShort += shortSign)
                    {
                        board.theBoard.TryGetValue((stepsToShort, newYPos), out shortStepIntermediate);
                        fullPathTiles.Add(shortStepIntermediate);
                    }
                }
                else
                {
                    for (int stepsToLong = oldXPos + longSign; newXPos - stepsToLong != 0; stepsToLong += longSign)
                    {
                        board.theBoard.TryGetValue((stepsToLong, oldYPos), out longStepIntermediate);
                        fullPathTiles.Add(longStepIntermediate);
                    }
                    for (int stepsToShort = oldYPos + shortSign; newYPos - stepsToShort != 0; stepsToShort += shortSign)
                    {
                        board.theBoard.TryGetValue((newXPos, stepsToShort), out shortStepIntermediate);
                        fullPathTiles.Add(shortStepIntermediate);
                    }
                }

                HashSet<BoardTile> tilesToAttackPassby = new HashSet<BoardTile>();
                foreach (BoardTile pathTile in fullPathTiles)
                {
                    foreach (BoardTile neighborTile in this.GetTileNeighbors(pathTile))
                    {
                        tilesToAttackPassby.Add(neighborTile);
                    }
                }

                List<MoveablePiece> enemies = new List<MoveablePiece>();
                MoveablePiece enemy;
                foreach (BoardTile tileToAttack in tilesToAttackPassby)
                {
                    if (board.allPieces.TryGetValue((tileToAttack.xCoord, tileToAttack.yCoord), out enemy) && enemy.owner != owner)
                    {
                        enemies.Add(enemy);
                        // if (this.canChainKill)
                        // {
                        //    this.numActions = 2;
                        // }
                    }
                }
                if (enemies.Count > 0)
                {
                    BoardTile targetTile;
                    board.theBoard.TryGetValue((newXPos, newYPos), out targetTile);

                    List<BoardTile> placesToMove;
                    MovePaths.TryGetValue(targetTile, out placesToMove);
                    
                    float totalTime = 0;
                    foreach (BoardTile tile in placesToMove)
                    {
                        totalTime += (moveTileSpeed * (tile.transform.position - transform.position).magnitude);
                    }
                        
                    StartCoroutine(this.killPassbyEnemies(enemies, totalTime));
                }
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public override bool Attack(int targetXPos, int targetYPos)
    {
        if (base.Attack(targetXPos, targetYPos)) 
        {
            if (this.canChainKill)
            {
                this.numActions = 2;
            }
            return true;
        }
        else
        {
            return false;
        }
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
        // Start to build the move paths dictionary
        this.MovePaths.Clear();

        // This returns all valid move tiles, regardless if something is there or not
        List<BoardTile> result = new List<BoardTile>();
        BoardTile tile;
        int newXPos;
        int newYPos;
        
        List<BoardTile> pathTiles;
        BoardTile longStep;
        BoardTile shortStep;

        for (int longSteps = this.baseLongRange; longSteps <= this.maxLongRange; longSteps++)
        {
            for (int shortSteps = this.baseShortRange; shortSteps <= this.maxShortRange; shortSteps++)
            {
                foreach (int longSign in signs)
                {
                    foreach (int shortSign in signs)
                    {
                        // Long step goes North or South - Short step goes East or West
                        newXPos = this.xPos + shortSign * shortSteps;
                        newYPos = this.yPos + longSign * longSteps;
                        if (board.theBoard.TryGetValue((newXPos, newYPos), out tile) && tile.canBeOccupied) { 
                            result.Add(tile);

                            pathTiles = new List<BoardTile>();
                            board.theBoard.TryGetValue((this.xPos, newYPos), out longStep);
                            pathTiles.Add(longStep);
                            board.theBoard.TryGetValue((newXPos, newYPos), out shortStep);
                            pathTiles.Add(shortStep);
                            MovePaths[tile] = pathTiles;
                        }

                        // Long step goes East or West - Short step goes North or South
                        newXPos = this.xPos + longSign * longSteps;
                        newYPos = this.yPos + shortSign * shortSteps;
                        if (board.theBoard.TryGetValue((newXPos, newYPos), out tile) && tile.canBeOccupied) {
                            result.Add(tile);

                            pathTiles = new List<BoardTile>();
                            board.theBoard.TryGetValue((newXPos, this.yPos), out longStep);
                            pathTiles.Add(longStep);
                            board.theBoard.TryGetValue((newXPos, newYPos), out shortStep);
                            pathTiles.Add(shortStep);
                            MovePaths[tile] = pathTiles;
                        }
                    }
                }
            }
        }
        return result;
    }

    private List<BoardTile> GetTileNeighbors(BoardTile tile)
    {
        int[] steps = new int[] { -1, 0, 1 };
        List<BoardTile> result = new List<BoardTile>();
        BoardTile neighbor;

        foreach (int verticalStep in steps)
        {
            foreach (int horizontalStep in steps)
            {
                if (!(horizontalStep == 0 && verticalStep == 0) && board.theBoard.TryGetValue((tile.xCoord + horizontalStep, tile.yCoord + verticalStep), out neighbor) && neighbor.canBeOccupied) { result.Add(neighbor); }
            }
        }

        return result;
    }

    private IEnumerator killPassbyEnemies(List<MoveablePiece> enemies, float initialWait)
    {
        yield return new WaitForSeconds(initialWait + this.waitAfterStop);

        foreach (MoveablePiece enemy in enemies)
        {
            enemy.Die();
            board.boardAudioController.PlayOneShot(board.playerKnightPassbyKill, 1.0f);
            yield return new WaitForSeconds(this.killInterWait);
        }

        yield return null;
    }
}

