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
    PieceSelect,
    TurnEnd
}

public class BoardStateManager : MonoBehaviour
{
    public Board board;
    public GameObject pawnPrefab;
    public GameObject rookPrefab;
    public GameObject knightPrefab;
    public GameObject bishopPrefab;
    public GameObject queenPrefab;
    public GameObject kingPrefab;
    public GameState currState;
    public CurrentTurn currentTurn;

    void Start()
    {
        currentTurn = CurrentTurn.Player;
        // On Start, set up the board like a normal chess match
        board.PlacePiece(0, 1, CurrentTurn.Player, pawnPrefab);
        board.PlacePiece(1, 1, CurrentTurn.Player, pawnPrefab);
        board.PlacePiece(2, 1, CurrentTurn.Player, pawnPrefab);
        board.PlacePiece(3, 1, CurrentTurn.Player, pawnPrefab);
        board.PlacePiece(4, 1, CurrentTurn.Player, pawnPrefab);
        board.PlacePiece(5, 1, CurrentTurn.Player, pawnPrefab);
        board.PlacePiece(6, 1, CurrentTurn.Player, pawnPrefab);
        board.PlacePiece(7, 1, CurrentTurn.Player, pawnPrefab);
    }

    void Update()
    {
        switch(currState)
        {
            case GameState.TurnStart:
                // Any animation or something.
                currState = GameState.PieceSelect;
                break;
            case GameState.PieceSelect:
                // For player, enable selecting pieces to move / attack
                break;
            case GameState.TurnEnd:
                // animation
                // change whose turn it is.
                currentTurn = currentTurn == CurrentTurn.Player ? CurrentTurn.Zombie : CurrentTurn.Player;
                break;
        }
    }
}
