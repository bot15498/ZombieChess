using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum CurrentTurn
{
    Player,
    Zombie
}

public enum GameState
{
    TurnStart,
    WaitForPieceSelect,
    PieceSelected,
    WaitForPieceMove,
    PieceMove,
    TurnEnd
}

public class BoardStateManager : MonoBehaviour
{
    public static BoardStateManager current;

    public MeshClickDetector detector;
    public GameObject startTile;
    public Board board;
    public GameObject pawnPrefab;
    public GameObject rookPrefab;
    public GameObject knightPrefab;
    public GameObject bishopPrefab;
    public GameObject queenPrefab;
    public GameObject kingPrefab;
    public GameObject shamblerPrefab;
    public GameObject boomerPrefab;
    public GameObject chargerPrefab;
    public GameObject hunterPrefab;
    public GameState currState;
    public CurrentTurn currentTurn;
    public int turnCount = 1;

    private int zombieLevel = 0;
    private MoveablePiece currSelectedPiece;
    private BoardTile currSelectedBoardTile;
    private List<BoardTile> possiblePlacesToMove = new List<BoardTile>();
    private List<BoardTile> possiblePlacesToAttack = new List<BoardTile>();

    private void Awake()
    {
        // I swear to god unity sucks this isn't how you make a singleton.
        current = this;
    }

    void Start()
    {
        currentTurn = CurrentTurn.Player;
        // First make the board a 8x8 board. We start with 1 tile, so increase it by 7
        board.theBoard.Add((0, 0), startTile.GetComponent<BoardTile>());
        board.ExpandBoard(7, BoardDirections.East);
        board.ExpandBoard(13, BoardDirections.North); board.ExpandBoard(13, BoardDirections.West);

        // On Start, set up the board like a normal chess match
        board.PlacePiece(0, 1, CurrentTurn.Player, pawnPrefab);
        board.PlacePiece(1, 1, CurrentTurn.Player, pawnPrefab);
        board.PlacePiece(2, 1, CurrentTurn.Player, pawnPrefab);
        board.PlacePiece(3, 1, CurrentTurn.Player, pawnPrefab);
        board.PlacePiece(4, 1, CurrentTurn.Player, pawnPrefab);
        board.PlacePiece(5, 1, CurrentTurn.Player, pawnPrefab);
        board.PlacePiece(6, 1, CurrentTurn.Player, pawnPrefab);
        board.PlacePiece(7, 1, CurrentTurn.Player, pawnPrefab);

        board.PlacePiece(0, 0, CurrentTurn.Player, rookPrefab);
        board.PlacePiece(7, 0, CurrentTurn.Player, rookPrefab);
        board.PlacePiece(1, 0, CurrentTurn.Player, knightPrefab);
        board.PlacePiece(6, 0, CurrentTurn.Player, knightPrefab);
        board.PlacePiece(2, 0, CurrentTurn.Player, bishopPrefab);
        board.PlacePiece(5, 0, CurrentTurn.Player, bishopPrefab);
        board.PlacePiece(3, 0, CurrentTurn.Player, queenPrefab);
        board.PlacePiece(4, 0, CurrentTurn.Player, kingPrefab);

        ZombieSpawnCheck();
    }

    void Update()
    {
        switch (currState)
        {
            case GameState.TurnStart:
                // Any animation or something.
                if (currentTurn == CurrentTurn.Player)
                {
                    detector.canClickPiece = true;
                    detector.canClickTile = false;
                }
                currState = GameState.WaitForPieceSelect;
                break;
            case GameState.WaitForPieceSelect:
                if (currentTurn == CurrentTurn.Player)
                {
                    // Wait for player to select the piece to move
                    if (currSelectedPiece != null)
                    {
                        currState = GameState.PieceSelected;
                    }
                }
                else if (currentTurn == CurrentTurn.Zombie)
                {
                    // Zombie moves all pieces at once, so go to next state.
                    currState = GameState.WaitForPieceMove;
                }
                break;
            case GameState.PieceSelected:
                if (currentTurn == CurrentTurn.Player)
                {
                    // Piece selected. Highlight the spots you can move to, then go to next state
                    possiblePlacesToMove = currSelectedPiece.PreviewMove();
                    possiblePlacesToAttack = currSelectedPiece.PreviewAttack();
                    if ((possiblePlacesToMove.Count == 0 && possiblePlacesToAttack.Count == 0) || currSelectedPiece.numActions <= 0)
                    {
                        // go back you doofus you can't move this piece.
                        currState = GameState.WaitForPieceSelect;
                        currSelectedPiece = null;
                    }
                    else
                    {
                        // highlight all the tiles
                        SetTileHighlightColor(possiblePlacesToMove, TileHighlightType.Move);
                        SetTileHighlightColor(possiblePlacesToAttack, TileHighlightType.Attack);
                        detector.canClickTile = true;
                        detector.canClickPiece = false;
                        currState = GameState.WaitForPieceMove;
                    }
                }
                break;
            case GameState.WaitForPieceMove:
                if (currentTurn == CurrentTurn.Player)
                {
                    // Wait for player to select a place to move to
                    if (currSelectedBoardTile != null)
                    {
                        // move the stupid thing.
                        if (possiblePlacesToAttack.Contains(currSelectedBoardTile))
                        {
                            // attacking
                            currSelectedPiece.Attack(currSelectedBoardTile.xCoord, currSelectedBoardTile.yCoord);
                        }
                        else
                        {
                            // just moving
                            currSelectedPiece.Move(currSelectedBoardTile.xCoord, currSelectedBoardTile.yCoord);
                        }
                        
                        // Clear out the highlighted tiles
                        SetTileHighlightColor(possiblePlacesToMove, TileHighlightType.Idle);
                        SetTileHighlightColor(possiblePlacesToAttack, TileHighlightType.Idle);

                        currState = GameState.PieceMove;
                    }
                    if (Input.GetMouseButtonDown(1))
                    {
                        // Cancelled!
                        SetTileHighlightColor(possiblePlacesToMove, TileHighlightType.Idle);
                        SetTileHighlightColor(possiblePlacesToAttack, TileHighlightType.Idle);
                        detector.canClickTile = false;
                        detector.canClickPiece = true;
                        currSelectedPiece = null;
                        currState = GameState.WaitForPieceSelect;
                    }
                }
                else
                {
                    List<IZombiePiece> pieces = board.allPieces.Values.Where(x => x.owner == currentTurn).Cast<IZombiePiece>().ToList();
                    foreach (IZombiePiece zom in pieces)
                    {
                        zom.ZombieAiAction();
                    }
                    currState = GameState.PieceMove;
                }
                break;
            case GameState.PieceMove:
                if (currentTurn == CurrentTurn.Player)
                {
                    // If piece has more than one move, go to previous state. Otherwise go forward in time.
                    if (board.objectsMoving.Count == 0)
                    {
                        currSelectedPiece.numActions--;
                        if (currSelectedPiece.numActions > 0)
                        {
                            // Recalculate the pieces' allowed places it can move / attack
                            possiblePlacesToMove = currSelectedPiece.PreviewMove();
                            possiblePlacesToAttack = currSelectedPiece.PreviewAttack();
                            currState = GameState.WaitForPieceMove;
                        }
                        else
                        {
                            currState = GameState.TurnEnd;
                        }
                    }
                }
                else
                {
                    // Pause or something, i dunno
                    if(board.objectsMoving.Count == 0)
                    {
                        currState = GameState.TurnEnd;
                    }
                }
                break;
            case GameState.TurnEnd:
                // animation
                if (currentTurn == CurrentTurn.Player)
                {
                    detector.canClickPiece = false;
                    detector.canClickTile = false;
                    currSelectedPiece = null;
                    currSelectedBoardTile = null;
                    possiblePlacesToMove.Clear();
                }
                else
                {
                    bool didLose = board.allPieces.Values.Where(x => x.owner == currentTurn).Any(x => x.LoseCheck());
                    if(didLose)
                    {
                        Lose();
                    }

                    turnCount++;
                    ZombieSpawnCheck();
                }
                // change whose turn it is.
                ResetMoveCount(currentTurn);
                currentTurn = currentTurn == CurrentTurn.Player ? CurrentTurn.Zombie : CurrentTurn.Player;
                currState = GameState.TurnStart;
                break;
        }
    }

    public void PieceSelectedForMovement(MoveablePiece piece)
    {
        if (currentTurn == piece.owner)
        {
            currSelectedPiece = piece;
        }
    }

    public void TileSelectedForMovement(int xPos, int yPos)
    {
        // Depite the name, this is for movement and for attacking
        BoardTile tile;
        bool found = board.theBoard.TryGetValue((xPos, yPos), out tile);
        if (found && (possiblePlacesToAttack.Contains(tile) || possiblePlacesToMove.Contains(tile)))
        {
            currSelectedBoardTile = tile;
        }
    }

    private void ResetMoveCount(CurrentTurn whois)
    {
        foreach (MoveablePiece piece in board.allPieces.Values)
        {
            if (piece.owner == whois)
            {
                piece.numActions = piece.maxNumActions;
            }
        }
    }

    private void SetTileHighlightColor(List<BoardTile> tiles, TileHighlightType highlightType)
    {
        foreach (BoardTile tile in tiles)
        {
            bool foundTile = board.theBoard.ContainsKey((tile.xCoord, tile.yCoord));
            if (foundTile)
            {
                tile.Highlight(highlightType);
            }
        }
    }

    private void ZombieSpawnCheck()
    {
        bool spawnNow = false;
        // Logic for determining if we are going to spawn more zombies or not. 
        int zombieCount = board.allPieces.Values.Where(x => x.owner == CurrentTurn.Zombie).Count();
        if (zombieCount == 0)
        {
            // Immediately go up a level
            zombieLevel++;
            spawnNow = true;
        }
        else if (turnCount % 5 == 0 && zombieLevel * 5 <= turnCount)
        {
            // 5 turns have passed, go up a level if you haven't already
            zombieLevel++;
            spawnNow = true;
        }

        if (spawnNow)
        {
            // based on the spawn level, do something different
            if (zombieLevel >= 0 && zombieLevel <= 2)
            {
                int numZombies = Random.Range(3, 5);
                SpawnZombieAtBackRow(3, numZombies, shamblerPrefab);
                SpawnZombieAtBackRow(3, 1, chargerPrefab);
            }
            else if (zombieLevel > 2 && zombieLevel <= 4)
            {
                int numZombies = Random.Range(2,4);
                SpawnZombieAtBackRow(3, numZombies, shamblerPrefab);
                SpawnZombieAtBackRow(3, 1, boomerPrefab);
            }
            else
            {
                int numZombies = Random.Range(5, 8);
                SpawnZombieAtBackRow(3, numZombies, shamblerPrefab);
                SpawnZombieAtBackRow(3, 2, boomerPrefab);
            }
        }
    }

    private bool SpawnZombieAtBackRow(int backRowRange, int numZombies, GameObject zombie)
    {
        // Figure out how many free spaces
        List<BoardTile> openTiles = new List<BoardTile>();
        BoardTile tile;
        for (int x = board.minXPos; x <= board.maxXPos; x++)
        {
            for (int y = board.maxYPos - backRowRange + 1; y <= board.maxYPos; y++)
            {
                if (!board.allPieces.ContainsKey((x, y)) && board.theBoard.TryGetValue((x, y), out tile))
                {
                    openTiles.Add(tile);
                }
            }
        }

        int toSpawn = Mathf.Min(numZombies, openTiles.Count);

        for (int i = 0; i < toSpawn; i++)
        {
            int openTileIdx = Random.Range(0, openTiles.Count);
            board.PlacePiece(openTiles[openTileIdx].xCoord, openTiles[openTileIdx].yCoord, CurrentTurn.Zombie, zombie);
        }

        return toSpawn != numZombies;
    }

    public void Lose()
    {
        // rip in peace lose 
        Debug.Log("You lose.");
    }
}
