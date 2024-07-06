using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Pawn : MonoBehaviour, IMoveablePiece
{
    [SerializeField]
    private int _xPos = -1;
    [SerializeField]
    private int _yPos = -1;
    [SerializeField]
    private bool isFirstMove = true;
    [SerializeField]
    private Board board;

    public int xPos { get => _xPos; set => _xPos = value; }
    public int yPos { get => _yPos; set => _yPos = value; }
    public int health { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public int maxHealth { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public int numActions { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public bool canAct { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public CurrentTurn owner { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    void Start()
    {

    }

    void Update()
    {

    }

    public bool Move(int newXPos, int newYPos)
    {
        // Return true if this is a valid move, other wise return false;
        isFirstMove = false;
        return false;
    }

    public bool Spawn(Board board, int xPos, int yPos, CurrentTurn owner)
    {
        this.board = board;
        this.xPos = xPos;
        this.yPos = yPos;
        this.owner = owner;
        return true;
    }

    public bool PreviewMove(out List<int> newXPos, out List<int> newYPos)
    {
        // Pawns can only move forward one square, or two squares if it's the first move for this pawn
        newXPos = new List<int> { xPos };
        newYPos = new List<int> { yPos };

        if (isFirstMove)
        {
            newXPos.Add(xPos);
            newYPos.Add(yPos + 2);
        }

        return true;
    }
}
