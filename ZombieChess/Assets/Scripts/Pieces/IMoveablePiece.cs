using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveablePiece
{
    public int xPos { get; set; }
    public int yPos { get; set; }
    public bool Move(out int newXPos, out int newYPos);
    public bool Spawn(Board board, int xPos, int ypos);
}
