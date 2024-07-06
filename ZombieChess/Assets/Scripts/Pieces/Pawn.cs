using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour, IMoveablePiece
{
    [SerializeField]
    private int _xPos = -1;
    [SerializeField]
    private int _yPos = -1;
    [SerializeField]
    private bool isFirstMove = true;

    public int xPos { get => _xPos; set => _xPos = value; }
    public int yPos { get => _yPos; set => _yPos = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Move(out int newXPos, out int newYPos)
    {
        // Pawns can only move forward one space, unless 
        if(isFirstMove)
        {
            newXPos = xPos;
            newYPos = yPos + 2;
        }
        else
        {
            newXPos = xPos;
            newYPos = yPos + 1;
        }
        return true;
    }

    public bool Spawn(Board board, int xPos, int ypos)
    {
        throw new System.NotImplementedException();
    }
}
