using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Shambler : MonoBehaviour, IMoveablePiece
{
    [SerializeField]
    private int _xPos = -1;
    [SerializeField]
    private int _yPos = -1;
    private CurrentTurn _owner = CurrentTurn.Zombie;
    private bool _canAct = true;
    private int _numActions = 1;
    private int _maxNumActions = 1;
    [SerializeField]
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

    public bool Die()
    {
        // delete yourself from the board
        board.allPieces.Remove((xPos, yPos));
        // delete yourself from existence
        Destroy(gameObject);
        Debug.Log("I die a hero");
        return true;
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
        throw new System.NotImplementedException();
    }

    public List<BoardTile> PreviewAttack()
    {
        throw new System.NotImplementedException();
    }

    public List<BoardTile> PreviewMove()
    {
        throw new System.NotImplementedException();
    }

    public bool Spawn(Board board, int xPos, int yPos, CurrentTurn owner)
    {
        this.board = board;
        this.xPos = xPos;
        this.yPos = yPos;
        this.owner = owner;
        return true;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
