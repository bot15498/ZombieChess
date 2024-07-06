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
    public GameObject tile1Prefab;
    public GameObject tile2Prefab;
    public float gridStep;
    public float pieceYOffset;
    private List<IMoveablePiece> playerPieces;
    private List<IMoveablePiece> enemyPieces;
    [SerializeField]
    private Dictionary<(int xPos, int yPos), IMoveablePiece> allPieces = new Dictionary<(int xPos, int yPos), IMoveablePiece>();
    [SerializeField]
    private Dictionary<(int xPos, int yPos), bool> theBoard = new Dictionary<(int xPos, int yPos), bool>();
    [SerializeField]
    private int minXPos = 0;
    [SerializeField]
    private int minYPos = 0;
    [SerializeField]
    private int maxXPos = 0;
    [SerializeField]
    private int maxYPos = 0;

    void Start()
    {

    }

    void Update()
    {

    }

    public IMoveablePiece Get(int xPos, int yPos)
    {
        // Given the boards x and y position, find the position inside the actual
        // list representation, then return if there is a piece there or not
        IMoveablePiece piece;
        bool returnval = allPieces.TryGetValue((xPos, yPos), out piece);
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
        bool somethingThere = theBoard.ContainsKey((xPos, yPos));
        if (somethingThere)
        {
            return false;
        }

        // Instantiate object
        Vector3 pos = new Vector3(xPos * gridStep, 0 + pieceYOffset, yPos * gridStep);
        GameObject newObject = Instantiate(obj, pos, Quaternion.identity, transform);
        IMoveablePiece piece = newObject.GetComponent<IMoveablePiece>();
        piece.Spawn(this, xPos, yPos, owner);
        allPieces.Add((xPos, yPos), piece);

        return true;
    }

    public void ExpandBoard(int val, BoardDirections direction)
    {
        // On a standard chess board, if you are white, the bottom left square is BLACK
        switch (direction)
        {
            case BoardDirections.North:
                // for each column
                for(int i=minXPos; i <= maxXPos; i++)
                {
                    // for each row you want to add
                    for(int j=maxYPos + 1; j<=maxYPos + val; j++)
                    {
                        Vector3 position = new Vector3(i * gridStep, 0, j * gridStep);
                        if ((i % 2 == 0 && j % 2 == 0) || (i % 2 != 0 && j % 2 != 0))
                        {
                            // spawn a black one
                            Instantiate(tile2Prefab, position, Quaternion.identity, transform);
                        }
                        else
                        {
                            // spawn a white one
                            Instantiate(tile1Prefab, position, Quaternion.identity, transform);
                        }
                    }
                }
                maxYPos += val;
                break;
            case BoardDirections.South:
                for (int i = minXPos; i <= maxXPos; i++)
                {
                    // for each row you want to add
                    for (int j = minYPos - 1; j >= minYPos - val; j--)
                    {
                        Vector3 position = new Vector3(i * gridStep, 0, j * gridStep);
                        if ((j % 2 == 0 && i % 2 == 0) || (j % 2 != 0 && i % 2 != 0))
                        {
                            // spawn a black one
                            Instantiate(tile2Prefab, position, Quaternion.identity, transform);
                        }
                        else
                        {
                            // spawn a white one
                            Instantiate(tile1Prefab, position, Quaternion.identity, transform);
                        }
                    }
                }
                minYPos -= val;
                break;
            case BoardDirections.East:
                // For each row
                for (int i = minYPos; i <= maxYPos; i++)
                {
                    // for each column you want to add
                    for (int j = maxXPos + 1; j <= maxXPos + val; j++)
                    {
                        Vector3 position = new Vector3(j * gridStep, 0, i * gridStep);
                        if ((i % 2 == 0 && j % 2 == 0) || (i % 2 != 0 && j % 2 != 0))
                        {
                            // spawn a black one
                            Instantiate(tile2Prefab, position,Quaternion.identity,transform);
                        }
                        else
                        {
                            // spawn a white one
                            Instantiate(tile1Prefab, position, Quaternion.identity, transform);
                        }
                    }
                }
                maxXPos += val;
                break;
            case BoardDirections.West:
                // For each row
                for (int i = minYPos; i <= maxYPos; i++)
                {
                    // for each column you want to add
                    for (int j = minXPos - 1; j >= minXPos - val; j--)
                    {
                        Vector3 position = new Vector3(j * gridStep, 0, i * gridStep);
                        if ((i % 2 == 0 && j % 2 == 0) || (i % 2 != 0 && j % 2 != 0))
                        {
                            // spawn a black one
                            Instantiate(tile2Prefab, position, Quaternion.identity, transform);
                        }
                        else
                        {
                            // spawn a white one
                            Instantiate(tile1Prefab, position, Quaternion.identity, transform);
                        }
                    }
                }
                minXPos -= val;
                break;

        }
    }
}
