using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UI.GridLayoutGroup;

public class Pawn : MonoBehaviour, IMoveablePiece
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
    private bool isFirstMove = true;
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
        UpgradeManager.current.ActivatePawnUpgrade += pawnUpgrade;
    }

    void Update()
    {

    }

    void pawnUpgrade(int id)
    {
        switch (id)
        {
            case 0:
                //upgrade stuff goes here
                Debug.Log("AAAAAAAA");
                break;

            case 1:

                break;

            default:

                break;
        }
        //Do upgrade stuff
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

        // Reset first move
        isFirstMove = false;
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
        // Give all the possible places that the pawn can move to.
        List<BoardTile> result = new List<BoardTile>();
        BoardTile tile;
        if (board.theBoard.TryGetValue((xPos, yPos + 1), out tile) && !board.allPieces.ContainsKey((xPos, yPos + 1)))
        {
            result.Add(tile);
        }
        if (isFirstMove && board.theBoard.TryGetValue((xPos, yPos + 2), out tile) && !board.allPieces.ContainsKey((xPos, yPos + 2)))
        {
            result.Add(tile);
        }
        return result;
    }

    public List<BoardTile> PreviewAttack()
    {
        // Give all the possible places that the pawn can move to.
        List<BoardTile> result = new List<BoardTile>();
        BoardTile tile;
        IMoveablePiece enemyPiece;
        if (board.allPieces.TryGetValue((xPos, yPos + 1), out enemyPiece) && enemyPiece.owner != owner)
        {
            board.theBoard.TryGetValue((xPos, yPos + 1), out tile);
            result.Add(tile);
        }
        if (isFirstMove && board.allPieces.TryGetValue((xPos, yPos + 2), out enemyPiece) && enemyPiece.owner != owner)
        {
            board.theBoard.TryGetValue((xPos, yPos + 2), out tile);
            result.Add(tile);
        }
        return result;
    }
}
