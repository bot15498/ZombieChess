using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveablePiece
{
    public int health { get; set; }
    public int maxHealth { get; set; }
    public int numActions { get; set; }
    public int maxNumActions { get; set; }
    public int xPos { get; set; }
    public int yPos { get; set; }
    public bool canAct { get; set; }
    public CurrentTurn owner { get; set; }
    public List<BoardTile> PreviewMove();
    public List<BoardTile> PreviewAttack();
    public bool Move(int newXPos, int newYPos);
    public bool Spawn(Board board, int xPos, int yPos, CurrentTurn owner);
}
