using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class MoveablePiece : MonoBehaviour
{
    [SerializeField]
    private int _moneyValue = 0;

    public int health { get; set; } = 1;
    public int maxHealth { get; set; } = 1;
    public int numActions { get; set; } = 1;
    public int maxNumActions { get; set; } = 1;
    public float moveTileSpeed { get; set; } = 0.1f; // Measured in seconds spent between unity world units
    public int xPos { get; set; } = 0;
    public int yPos { get; set; } = 0;
    public int moneyValue { get => _moneyValue; set => _moneyValue = value; }   
    public bool canAct { get; set; } = true;
    public int size { get; set; } = 1;
    public CurrentTurn owner { get; set; }
    public bool isMoving { get; set; } = false;
    public BoardTile moveTarget { get; set; }

    [SerializeField]
    public GameObject bloodDecal;
    protected Board board { get; set; }
    protected Rigidbody rb { get; set; }
    // For all the possible places that a piece will end at, get the places that the piece has to move to before it reaches the end.
    // For simple movement, this will just be a list of length 1 with the end being the same as the key
    public Dictionary<BoardTile, List<BoardTile>> MovePaths { get; set; } = new Dictionary<BoardTile, List<BoardTile>>();
    // For the current movement, Get the places that are going to be attacked by this turn. 
    public List<BoardTile> AttackTiles { get; set; } = new List<BoardTile>();
    public abstract List<BoardTile> PreviewMove();
    public abstract List<BoardTile> PreviewAttack();

    public virtual bool Move(int newXPos, int newYPos)
    {
        // When running preview move, it is assumed that MovePaths and Attack tiles are populated. 
        // If they aren't, assume that they are empty, and we are just doing a simple move. 
        BoardTile targetTile;
        if (board.theBoard.TryGetValue((newXPos, newYPos), out targetTile))
        {
            // Move the piece according to the key. If you can't find a key, then do just a simple move. 
            List<BoardTile> placesToMove;
            if (!MovePaths.TryGetValue(targetTile, out placesToMove))
            {
                placesToMove = new List<BoardTile> { targetTile };
            }
            float startDelay = owner == CurrentTurn.Zombie ? Random.Range(0f, 1f) : 0;
            StartCoroutine(DoPieceMovement(placesToMove, startDelay, 0f));
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual bool Spawn(Board board, int xPos, int yPos, CurrentTurn owner)
    {
        this.board = board;
        this.xPos = xPos;
        this.yPos = yPos;
        this.owner = owner;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        BoardStateManager.current.TurnStartAction += GenericOnTurnStart;
        BoardStateManager.current.TurnEndAction += GenericOnTurnEnd;
        return true;
    }

    private void GenericOnTurnStart(int turnCount)
    {
        AttackTiles.Clear();
    }

    private void GenericOnTurnEnd(int turnCount)
    {

    }

    public virtual bool Attack(int targetXPos, int targetYPos)
    {
        // The default attack behavior is to just move the piece to the place you are attacking
        BoardTile target;
        if (board.theBoard.TryGetValue((targetXPos, targetYPos), out target))
        {
            AttackTiles.Add(target);
            Move(targetXPos, targetYPos);
            return true;
        }
        else
        {
            return false;
        }
    }
    public virtual bool Die()
    {
        UpgradeManager.current.addMoney(moneyValue);
        // delete yourself from the board
        board.allPieces.Remove((xPos, yPos));
        // delete yourself from existence
        Instantiate(bloodDecal, gameObject.transform.position, bloodDecal.transform.rotation);
        Destroy(gameObject);
        return true;
    }
    public int ManDistance(MoveablePiece other)
    {
        // Returns the manhattan distance between this piece and another
        return Mathf.Abs(other.xPos - xPos) + Mathf.Abs(other.yPos - yPos);
    }
    public int ManDistance(BoardTile tile)
    {
        // Returns the manhattan distance between this piece and another tile
        return Mathf.Abs(tile.xCoord - xPos) + Mathf.Abs(tile.yCoord - yPos);
    }
    public bool LoseCheck()
    {
        // Checks to see if the player has lost the game
        //If you are a zombie and are at minYPos, then you lose
        return owner == CurrentTurn.Zombie && yPos == board.minYPos;
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        MoveablePiece enemy;
        if (collision.gameObject.TryGetComponent(out enemy) && enemy.owner != owner)
        {
            BoardTile tile;
            if (board.theBoard.TryGetValue((enemy.xPos, enemy.yPos), out tile) && AttackTiles.Contains(tile))
            {
                // Hit an enemy on a square you meant to attack, do damage to them. 
                enemy.Die();
            }
        }
    }

    protected IEnumerator DoPieceMovement(List<BoardTile> placesToMove, float delay, float interDelay)
    {
        board.objectsMoving.Add(this);
        yield return new WaitForSeconds(delay);

        foreach (BoardTile tile in placesToMove)
        {
            // Move the piece to the target tile
            Vector3 origPosition = transform.position;
            Vector3 newPosition = board.GetPositionForGridPosition(tile);
            float elapsedTime = 0;
            float totalTime = moveTileSpeed * (tile.transform.position - transform.position).magnitude;
            //while (Mathf.Sin(elapsedTime / totalTime * (Mathf.PI / 2)) <= 0.98f)
            while (elapsedTime < totalTime)
            {
                if (!gameObject) { break; }
                //transform.position = Vector3.Lerp(origPosition, newPosition, Mathf.Sin(elapsedTime / totalTime * (Mathf.PI / 2)));
                transform.position = Vector3.Lerp(origPosition, newPosition, elapsedTime / totalTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.position = newPosition;
            yield return new WaitForSeconds(interDelay);
        }

        // Update the board
        // Todo, if there is a piece there, where do you end up?
        board.allPieces.Remove((xPos, yPos));
        xPos = placesToMove.Last().xCoord;
        yPos = placesToMove.Last().yCoord;
        board.allPieces.Add((xPos, yPos), this);

        board.objectsMoving.Remove(this);
        yield return null;
    }

    protected IEnumerator DoPieceJumpMovement(List<BoardTile> placesToMove, float jumpHeight, float delay, float interDelay)
    {
        board.objectsMoving.Add(this);
        yield return new WaitForSeconds(delay);

        foreach (BoardTile tile in placesToMove)
        {
            // Move the piece to the target tile
            Vector3 origPosition = transform.position;
            Vector3 newPosition = board.GetPositionForGridPosition(tile);
            Vector3 centerPosition = Vector3.Lerp(origPosition, newPosition, 0.5f) + Vector3.up * jumpHeight;
            float elapsedTime = 0;
            float totalTime = moveTileSpeed * (tile.transform.position - transform.position).magnitude;
            while (elapsedTime < totalTime)
            {
                if (!gameObject) { break; }
                Vector3 m1 = Vector3.Lerp(origPosition, centerPosition, elapsedTime / totalTime);
                Vector3 m2 = Vector3.Lerp(centerPosition, newPosition, elapsedTime / totalTime);
                transform.position = Vector3.Lerp(m1, m2, elapsedTime / totalTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.position = newPosition;
            yield return new WaitForSeconds(interDelay);
        }

        // Update the board
        // Todo, if there is a piece there, where do you end up?
        board.allPieces.Remove((xPos, yPos));
        xPos = placesToMove.Last().xCoord;
        yPos = placesToMove.Last().yCoord;
        board.allPieces.Add((xPos, yPos), this);

        board.objectsMoving.Remove(this);
        yield return null;
    }
}
