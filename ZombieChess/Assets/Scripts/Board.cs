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
    public Dictionary<(int xPos, int yPos), MoveablePiece> allPieces = new Dictionary<(int xPos, int yPos), MoveablePiece>();
    public Dictionary<(int xPos, int yPos), BoardTile> theBoard = new Dictionary<(int xPos, int yPos), BoardTile>();
    public int minXPos = 0;
    public int minYPos = 0;
    public int maxXPos = 0;
    public int maxYPos = 0;
    public List<MoveablePiece> objectsMoving = new List<MoveablePiece>();
    public float moveTime = 0.5f;

    void Start()
    {

    }

    void Update()
    {

    }

    public MoveablePiece Get(int xPos, int yPos)
    {
        // Given the boards x and y position, find the position inside the actual
        // list representation, then return if there is a piece there or not
        MoveablePiece piece;
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
        bool somethingThere = allPieces.ContainsKey((xPos, yPos));
        if (somethingThere)
        {
            return false;
        }

        // Instantiate object
        Vector3 pos = new Vector3(xPos * gridStep, 0 + pieceYOffset, yPos * gridStep) + transform.position;
        GameObject newObject = Instantiate(obj, pos, Quaternion.identity, transform);
        MoveablePiece piece = newObject.GetComponent<MoveablePiece>();
        piece.Spawn(this, xPos, yPos, owner);
        allPieces.Add((xPos, yPos), piece);

        return true;
    }

    public bool MovePiece(GameObject obj, int newXPos, int newYPos)
    {
        MoveablePiece piece = obj.GetComponent<MoveablePiece>();
        if (piece != null)
        {
            allPieces.Remove((piece.xPos, piece.yPos));
            allPieces.Add((newXPos, newYPos), piece);
            // Move it physically
            // If this is a zombie, give it a random delay before hand
            float delay = 0f;
            if(piece.owner == CurrentTurn.Zombie)
            {
                delay = Random.Range(0f, 1f);
            }
            StartCoroutine(MovePieceLerp(obj, newXPos, newYPos, delay));
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator MovePieceLerp(GameObject obj, int newXPos, int newYPos, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        objectsMoving.Add(obj.GetComponent<MoveablePiece>());
        Vector3 origPosition = obj.transform.position;
        Vector3 newPosition = new Vector3(newXPos * gridStep, 0 + pieceYOffset, newYPos * gridStep) + transform.position;
        float elapsedTime = 0;
        while (Mathf.Sin(elapsedTime / moveTime * (Mathf.PI / 2)) <= 0.95f)
        {
            if (!obj)
            {
                break;
            }
            obj.transform.position = Vector3.Lerp(origPosition, newPosition, Mathf.Sin(elapsedTime / moveTime * (Mathf.PI / 2)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        obj.transform.position = newPosition;
        objectsMoving.Remove(obj.GetComponent<MoveablePiece>());
        yield return null;
    }

    public void ExpandBoard(int val, BoardDirections direction)
    {
        // On a standard chess board, if you are white, the bottom left square is BLACK
        GameObject obj;
        switch (direction)
        {
            case BoardDirections.North:
                // for each column
                for (int i = minXPos; i <= maxXPos; i++)
                {
                    // for each row you want to add
                    for (int j = maxYPos + 1; j <= maxYPos + val; j++)
                    {
                        Vector3 position = new Vector3(i * gridStep, 0, j * gridStep) + transform.position;
                        if ((i % 2 == 0 && j % 2 == 0) || (i % 2 != 0 && j % 2 != 0))
                        {
                            // spawn a black one
                            obj = Instantiate(tile2Prefab, position, Quaternion.identity, transform);
                        }
                        else
                        {
                            // spawn a white one
                            obj = Instantiate(tile1Prefab, position, Quaternion.identity, transform);
                        }
                        BoardTile tile = obj.GetComponent<BoardTile>();
                        tile.SetCoord(i, j);
                        theBoard.Add((i, j), tile);
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
                        Vector3 position = new Vector3(i * gridStep, 0, j * gridStep) + transform.position;
                        if ((j % 2 == 0 && i % 2 == 0) || (j % 2 != 0 && i % 2 != 0))
                        {
                            // spawn a black one
                            obj = Instantiate(tile2Prefab, position, Quaternion.identity, transform);
                        }
                        else
                        {
                            // spawn a white one
                            obj = Instantiate(tile1Prefab, position, Quaternion.identity, transform);
                        }
                        BoardTile tile = obj.GetComponent<BoardTile>();
                        tile.SetCoord(i, j);
                        theBoard.Add((i, j), tile);
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
                        Vector3 position = new Vector3(j * gridStep, 0, i * gridStep) + transform.position;
                        if ((i % 2 == 0 && j % 2 == 0) || (i % 2 != 0 && j % 2 != 0))
                        {
                            // spawn a black one
                            obj = Instantiate(tile2Prefab, position, Quaternion.identity, transform);
                        }
                        else
                        {
                            // spawn a white one
                            obj = Instantiate(tile1Prefab, position, Quaternion.identity, transform);
                        }
                        BoardTile tile = obj.GetComponent<BoardTile>();
                        tile.SetCoord(j, i);
                        theBoard.Add((j, i), tile);
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
                        Vector3 position = new Vector3(j * gridStep, 0, i * gridStep) + transform.position;
                        if ((i % 2 == 0 && j % 2 == 0) || (i % 2 != 0 && j % 2 != 0))
                        {
                            // spawn a black one
                            obj = Instantiate(tile2Prefab, position, Quaternion.identity, transform);
                        }
                        else
                        {
                            // spawn a white one
                            obj = Instantiate(tile1Prefab, position, Quaternion.identity, transform);
                        }
                        BoardTile tile = obj.GetComponent<BoardTile>();
                        tile.SetCoord(j, i);
                        theBoard.Add((j, i), tile);
                    }
                }
                minXPos -= val;
                break;

        }
    }
}
