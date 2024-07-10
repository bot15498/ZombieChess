using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Queen : MoveablePiece
{
    [SerializeField]
    private bool canKnightMove = false;

    [SerializeField]
    private bool canMoveAfterAttack = false;
    [SerializeField]
    private bool didAttack = false;

    [SerializeField]
    private bool canExtraMovePoint = false;
    [SerializeField]
    private int noMoveChargeCount = 0;
    [SerializeField]
    private bool didMoveThisTurn = false;


    void Start()
    {
        UpgradeManager.current.ActivateQueenUpgrade += QueenUpgrade;
        BoardStateManager.current.TurnStartAction += OnTurnStart;
        BoardStateManager.current.TurnEndAction += OnTurnEnd;
    }

    void QueenUpgrade(int id)
    {
        switch (id)
        {
            case 0:
                // Can move like a knight
                canKnightMove = true;
                break;
            case 4:
                // After you attack a piece, can move one more time, but can't take another piece.
                canMoveAfterAttack = true;
                break;
            case 6:
                // Every turn you sit there, you gain an extra action point
                canExtraMovePoint = true;
                break;
        }
    }

    private void OnTurnStart(int turnCount)
    {
        didMoveThisTurn = false;
        didAttack = false;
        if (canExtraMovePoint)
        {
            numActions = maxNumActions + noMoveChargeCount;
        }
    }

    private void OnTurnEnd(int turnCount)
    {
        if (canExtraMovePoint && !didMoveThisTurn)
        {
            noMoveChargeCount++;
        }
    }

    public override bool Move(int newXPos, int newYPos)
    {
        // Reset the move counter 
        noMoveChargeCount = 0;
        didMoveThisTurn = true;
        return base.Move(newXPos, newYPos);
    }

    public override bool Attack(int targetXPos, int targetYPos)
    {
        bool returnval = base.Attack(targetXPos, targetYPos);
        if(canMoveAfterAttack && numActions == 1)
        {
            didAttack = true;
            numActions++;
        }
        return returnval;
    }

    public override List<BoardTile> PreviewMove()
    {
        // Give all the possible places that the queen can move to.
        List<BoardTile> result = GetAllValidMoveTiles();
        return result.Where(x => !board.allPieces.ContainsKey((x.xCoord, x.yCoord))).ToList();
    }

    public override List<BoardTile> PreviewAttack()
    {
        // Give all the possible places that the queen can attack.
        List<BoardTile> result = new List<BoardTile>();
        if(canMoveAfterAttack && didAttack && numActions == 1)
        {
            // when you last did an attack, don't allow another attack. Only moveemnt
            return result;  
        }

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
        // This is the same as the rook and the bishop code.
        // This returns all valid move tiles, regardless if something is there or not
        List<BoardTile> result = new List<BoardTile>();
        BoardTile tile;

        int northEastMaxCheck = Mathf.Min(board.maxXPos - xPos, board.maxYPos - yPos);
        int northWestMaxCheck = Mathf.Min(xPos - board.minXPos, board.maxYPos - yPos);
        int southEastMaxCheck = Mathf.Min(board.maxXPos - xPos, yPos - board.minYPos);
        int southWestMaxCheck = Mathf.Min(xPos - board.minXPos, yPos - board.minYPos);

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

        // northj east check
        for (int i = 1; i <= northEastMaxCheck; i++)
        {
            if (board.theBoard.TryGetValue((xPos + i, yPos + i), out tile) && tile.canBeOccupied)
            {
                result.Add(tile);
            }
            if (board.allPieces.ContainsKey((xPos + i, yPos + i)) || !tile.canBeOccupied)
            {
                // Hit a piece, break out
                break;
            }
        }
        //north west checl
        for (int i = 1; i <= northWestMaxCheck; i++)
        {
            if (board.theBoard.TryGetValue((xPos - i, yPos + i), out tile) && tile.canBeOccupied)
            {
                result.Add(tile);
            }
            if (board.allPieces.ContainsKey((xPos - i, yPos + i)) || !tile.canBeOccupied)
            {
                break;
            }
        }
        // south easht che
        for (int i = 1; i <= southEastMaxCheck; i++)
        {
            if (board.theBoard.TryGetValue((xPos + i, yPos - i), out tile) && tile.canBeOccupied)
            {
                result.Add(tile);
            }
            if (board.allPieces.ContainsKey((xPos + i, yPos - i)) || !tile.canBeOccupied)
            {
                break;
            }
        }
        // south west check
        for (int i = 1; i <= southWestMaxCheck; i++)
        {
            if (board.theBoard.TryGetValue((xPos - i, yPos - i), out tile) && tile.canBeOccupied)
            {
                result.Add(tile);
            }
            if (board.allPieces.ContainsKey((xPos - i, yPos - i)) || !tile.canBeOccupied)
            {
                break;
            }
        }

        if (canKnightMove)
        {
            int[] signs = new int[] { -1, 1 };
            int baseLongRange = 2;
            int baseShortRange = 1;
            foreach (int longSign in signs)
            {
                foreach (int shortSign in signs)
                {
                    // Long step goes North or South - Short step goes East or West
                    if (board.theBoard.TryGetValue((xPos + shortSign * baseShortRange, yPos + longSign * baseLongRange), out tile) && tile.canBeOccupied) { result.Add(tile); }
                    // Long step goes East or West - Short step goes North or South
                    if (board.theBoard.TryGetValue((xPos + longSign * baseLongRange, yPos + shortSign * baseShortRange), out tile) && tile.canBeOccupied) { result.Add(tile); }
                }
            }
        }

        return result;
    }
}
