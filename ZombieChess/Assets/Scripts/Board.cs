using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoardDirections
{
    North,
    West,
    East,
    South
}

public class Board : MonoBehaviour
{
    public Transform originPoint;
    public int gridStep;
    private List<IMoveablePiece> playerPieces;
    private List<IMoveablePiece> enemyPieces;
    private Dictionary<(int xPos, int yPos), IMoveablePiece> theBoard;
    private int minXPos = 0;
    private int minYPos = 0;
    private int maxXPos = 7;
    private int maxYPos = 7;

    void Start()
    {
        theBoard = new Dictionary<(int xPos, int yPos), IMoveablePiece>();
    }

    void Update()
    {

    }

    public IMoveablePiece Get(int xPos, int yPos)
    {
        // Given the boards x and y position, find the position inside the actual
        // list representation, then return if there is a piece there or not
        IMoveablePiece piece;
        bool returnval = theBoard.TryGetValue((xPos, yPos), out piece);
        if (returnval)
        {
            return piece;
        }
        else
        {
            return null;
        }
    }

    public bool PlacePiece(int xPos, int yPos, CurrentTurn owner, GameObject obj)
    {
        // First check if we can put the piece there or not
        bool canPlace = theBoard.ContainsKey((xPos, yPos));
        if (!canPlace)
        {
            return false;
        }

        // Instantiate object
        Vector3 pos = new Vector3(xPos * gridStep, 0, yPos * gridStep);
        GameObject newObject = Instantiate(obj, pos, Quaternion.identity, transform);
        IMoveablePiece piece = newObject.GetComponent<IMoveablePiece>();
        piece.Spawn(this, xPos, yPos, owner);
        theBoard.Add((xPos, yPos), piece);

        return true;
    }

    public void ExpandBoard(int val, BoardDirections direction)
    {
        switch (direction)
        {
            case BoardDirections.North:
                maxYPos += val;
                break;
            case BoardDirections.South:
                maxYPos -= val;
                break;
            case BoardDirections.East:
                minXPos += val;
                break;
            case BoardDirections.West:
                minXPos -= val;
                break;

        }
    }
}
