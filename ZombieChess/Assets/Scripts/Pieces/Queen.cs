using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Queen : MonoBehaviour, IMoveablePiece
{
    [SerializeField]
    private int _xPos = -1;
    [SerializeField]
    private int _yPos = -1;
    private CurrentTurn _owner = CurrentTurn.Player;
    private bool _canAct = true;
    private int _numActions = 1;
    private int _maxNumActions = 1;
    private int _health = 1;
    private int _maxHealth = 1;
    [SerializeField]
    private Board board;

    public int xPos { get => _xPos; set => _xPos = value; }
    public int yPos { get => _yPos; set => _yPos = value; }
    public int health { get => _health; set => _health = value; }
    public int maxHealth { get => _maxHealth; set => _maxHealth = value; }
    public int numActions { get => _numActions; set => _numActions = value; }
    public int maxNumActions { get => _maxNumActions; set => _maxNumActions = value; }
    public bool canAct { get => _canAct; set => _canAct = value; }
    public CurrentTurn owner { get => _owner; set => _owner = value; }

    void Start()
    {
        UpgradeManager.current.ActivateQueenUpgrade += QueenUpgrade;
    }

    void Update()
    {

    }

    void QueenUpgrade(int id)
    {
        switch (id)
        {
            case 0:
                //upgrade stuff goes here upgrade id 0
                Debug.Log("AAAAAAAA");
                break;

            case 1:
                //upgrade stuff goes here upgrade id 1 
                break;

            default:

                break;
        }
    }

    public bool Move(int newXPos, int newYPos)
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

    public bool Spawn(Board board, int xPos, int yPos, CurrentTurn owner)
    {
        this.board = board;
        this.xPos = xPos;
        this.yPos = yPos;
        this.owner = owner;
        return true;
    }

    public List<BoardTile> PreviewMove()
    {
        // Give all the possible places that the queen can move to.
        List<BoardTile> result = GetAllValidMoveTiles();
        return result.Where(x => !board.allPieces.ContainsKey((x.xCoord, x.yCoord))).ToList();
    }

    public List<BoardTile> PreviewAttack()
    {
        // Give all the possible places that the queen can attack.
        List<BoardTile> result = new List<BoardTile>();
        IMoveablePiece enemyPiece;
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
            board.theBoard.TryGetValue((xPos, i), out tile);
            result.Add(tile);
            if (board.allPieces.ContainsKey((xPos, i)))
            {
                // Founda piece, so stop looking.
                break;
            }
        }

        // south check
        for (int i = yPos - 1; i >= board.minYPos; i--)
        {
            board.theBoard.TryGetValue((xPos, i), out tile);
            result.Add(tile);
            if (board.allPieces.ContainsKey((xPos, i)))
            {
                break;
            }
        }

        // weast check
        for (int i = xPos - 1; i >= board.minXPos; i--)
        {
            board.theBoard.TryGetValue((i, yPos), out tile);
            result.Add(tile);
            if (board.allPieces.ContainsKey((i, yPos)))
            {
                break;
            }
        }

        // east check
        for (int i = xPos + 1; i <= board.maxXPos; i++)
        {
            board.theBoard.TryGetValue((i, yPos), out tile);
            result.Add(tile);
            if (board.allPieces.ContainsKey((i, yPos)))
            {
                break;
            }
        }

        // northj east check
        for (int i = 1; i <= northEastMaxCheck; i++)
        {
            if (board.theBoard.TryGetValue((xPos + i, yPos + i), out tile))
            {
                result.Add(tile);
            }
            if (board.allPieces.ContainsKey((xPos + i, yPos + i)))
            {
                // Hit a piece, break out
                break;
            }
        }
        //north west checl
        for (int i = 1; i <= northWestMaxCheck; i++)
        {
            if (board.theBoard.TryGetValue((xPos - i, yPos + i), out tile))
            {
                result.Add(tile);
            }
            if (board.allPieces.ContainsKey((xPos - i, yPos + i)))
            {
                break;
            }
        }
        // south easht che
        for (int i = 1; i <= southEastMaxCheck; i++)
        {
            if (board.theBoard.TryGetValue((xPos + i, yPos - i), out tile))
            {
                result.Add(tile);
            }
            if (board.allPieces.ContainsKey((xPos + i, yPos - i)))
            {
                break;
            }
        }
        // south west check
        for (int i = 1; i <= southWestMaxCheck; i++)
        {
            if (board.theBoard.TryGetValue((xPos - i, yPos - i), out tile))
            {
                result.Add(tile);
            }
            if (board.allPieces.ContainsKey((xPos - i, yPos - i)))
            {
                break;
            }
        }

        return result;
    }
}
