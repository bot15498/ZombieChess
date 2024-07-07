using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Shambler : MoveablePiece, IZombiePiece

{
    [SerializeField]
    private int moveChanceOneInX = 3;

    public override List<BoardTile> PreviewMove()
    {
        // Give all the possible places that the pawn can move to.
        List<BoardTile> result = new List<BoardTile>();
        BoardTile tile;
        if (board.theBoard.TryGetValue((xPos, yPos - 1), out tile) && tile.canBeOccupied && !board.allPieces.ContainsKey((xPos, yPos - 1)))
        {
            result.Add(tile);
        }
        return result;
    }

    public override List<BoardTile> PreviewAttack()
    {
        // Give all the possible places that the pawn can move to.
        List<BoardTile> result = new List<BoardTile>();
        BoardTile tile;
        MoveablePiece enemyPiece;
        if (board.allPieces.TryGetValue((xPos, yPos - 1), out enemyPiece) && enemyPiece.owner != owner)
        {
            board.theBoard.TryGetValue((xPos, yPos - 1), out tile);
            result.Add(tile);
        }
        if (board.allPieces.TryGetValue((xPos - 1, yPos - 1), out enemyPiece) && enemyPiece.owner != owner)
        {
            board.theBoard.TryGetValue((xPos - 1, yPos - 1), out tile);
            result.Add(tile);
        }
        if (board.allPieces.TryGetValue((xPos + 1, yPos - 1), out enemyPiece) && enemyPiece.owner != owner)
        {
            board.theBoard.TryGetValue((xPos + 1, yPos - 1), out tile);
            result.Add(tile);
        }
        return result;
    }

    public void ZombieAiAction()
    {
        List<BoardTile> possibleAttacks = this.PreviewAttack();
        List<BoardTile> possibleMoves = this.PreviewMove();

        if (possibleAttacks.Count > 0)
        {
            int pickedAttack = Random.Range(0, possibleAttacks.Count);
            this.Attack(possibleAttacks[pickedAttack].xCoord, possibleAttacks[pickedAttack].yCoord);
        }
        else if (possibleMoves.Count > 0)
        {
            int moveRoll = Random.Range(0, moveChanceOneInX);
            if (moveRoll == 0)
            {
                this.Move(possibleMoves[0].xCoord, possibleMoves[0].yCoord);
            }

        }

        return;

    }

    void Start()
    {
        
    }
}
