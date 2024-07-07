using System.Collections;
using System.Collections.Generic;
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
    public GameState currState;
    public CurrentTurn currentTurn;

    private IMoveablePiece currSelectedPiece;
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
        board.ExpandBoard(7, BoardDirections.North);

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
    }

    void Update()
    {
        switch(currState)
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
                break;
            case GameState.PieceSelected:
                if (currentTurn == CurrentTurn.Player)
                {
                    // Piece selected. Highlight the spots you can move to, then go to next state
                    possiblePlacesToMove = currSelectedPiece.PreviewMove();
                    possiblePlacesToAttack = currSelectedPiece.PreviewAttack();
                    if (possiblePlacesToMove.Count == 0 && possiblePlacesToAttack.Count == 0)
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
                    if(currSelectedBoardTile != null)
                    {
                        currState = GameState.PieceMove;
                    }
                }
                break;
            case GameState.PieceMove:
                if (currentTurn == CurrentTurn.Player)
                {
                    // move the stupid thing.
                    currSelectedPiece.Move(currSelectedBoardTile.xCoord, currSelectedBoardTile.yCoord);

                    // Clear out the highlighted tiles
                    SetTileHighlightColor(possiblePlacesToMove, TileHighlightType.Idle);
                    SetTileHighlightColor(possiblePlacesToAttack, TileHighlightType.Idle);

                    // If piece has more thane one move, go to previous state. Otherwise go forward in time.
                    currSelectedPiece.numActions--;
                    if (currSelectedPiece.numActions > 0)
                    {
                        // Recalculate the pieces' allowed places it can move / attackl
                        possiblePlacesToMove = currSelectedPiece.PreviewMove();
                        possiblePlacesToAttack = currSelectedPiece.PreviewAttack();
                        currState = GameState.WaitForPieceMove;
                    }
                    else
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
                // change whose turn it is.
                ResetMoveCount(currentTurn);
                //currentTurn = currentTurn == CurrentTurn.Player ? CurrentTurn.Zombie : CurrentTurn.Player;
                currState = GameState.TurnStart;
                break;
        }
    }

    public void PieceSelectedForMovement(IMoveablePiece piece)
    {
        if (currentTurn == piece.owner)
        {
            currSelectedPiece = piece;
        }
    }

    public void TileSelectedForMovement(int xPos, int yPos)
    {
        BoardTile tile;
        bool found = board.theBoard.TryGetValue((xPos, yPos), out tile);
        if (found && (possiblePlacesToAttack.Contains(tile) || possiblePlacesToMove.Contains(tile)))
        {
            currSelectedBoardTile = tile;
        }
    }

    private void ResetMoveCount(CurrentTurn whois)
    {
        foreach(IMoveablePiece piece in board.allPieces.Values)
        {
            if(piece.owner == whois)
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
}
