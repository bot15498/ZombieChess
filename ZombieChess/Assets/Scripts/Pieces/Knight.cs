using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Knight : MonoBehaviour, IMoveablePiece
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
        UpgradeManager.current.ActivateKnightUpgrade += KnightUpgrade;
    }

    void Update()
    {

    }
    void KnightUpgrade(int id)
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

    public bool Attack(int targetXPos, int targetYPos)
    {
        // Do damage
        IMoveablePiece enemy;
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
        // Give all the possible places that the sam knight can move to.
        List<BoardTile> result = GetAllValidMoveTiles();
        return result.Where(x => !board.allPieces.ContainsKey((x.xCoord, x.yCoord))).ToList();
    }

    public List<BoardTile> PreviewAttack()
    {
        // Give all the possible places that the knighjt can move to.
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
        // This returns all valid move tiles, regardless if something is there or not
        List<BoardTile> result = new List<BoardTile>();
        BoardTile tile;
        if (board.theBoard.TryGetValue((xPos - 2, yPos + 1), out tile) && tile.canBeOccupied) { result.Add(tile); }
        if (board.theBoard.TryGetValue((xPos - 1, yPos + 2), out tile) && tile.canBeOccupied) { result.Add(tile); }
        if (board.theBoard.TryGetValue((xPos + 1, yPos + 2), out tile) && tile.canBeOccupied) { result.Add(tile); }
        if (board.theBoard.TryGetValue((xPos + 2, yPos + 1), out tile) && tile.canBeOccupied) { result.Add(tile); }
        if (board.theBoard.TryGetValue((xPos + 2, yPos - 1), out tile) && tile.canBeOccupied) { result.Add(tile); }
        if (board.theBoard.TryGetValue((xPos + 1, yPos - 2), out tile) && tile.canBeOccupied) { result.Add(tile); }
        if (board.theBoard.TryGetValue((xPos - 1, yPos - 2), out tile) && tile.canBeOccupied) { result.Add(tile); }
        if (board.theBoard.TryGetValue((xPos - 2, yPos - 1), out tile) && tile.canBeOccupied) { result.Add(tile); }

        return result;
    }

    public bool Die()
    {
        // delete yourself from the board
        board.allPieces.Remove((xPos, yPos));
        // delete yourself from existence
        Destroy(gameObject);
        return true;
    }
}
