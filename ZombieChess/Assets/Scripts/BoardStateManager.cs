using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CurrentTurn
{
    Player,
    Zombie
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

    void Start()
    {
        // On Start, set up the board like a normal chess match
    }

    void Update()
    {
        
    }
}
