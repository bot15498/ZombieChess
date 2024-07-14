using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hunter : MoveablePiece, IZombiePiece
{
    public int pounceCharge = 1;
    public int jumpRadius = 3;
    public float jumpHeight = 1.5f;
    private int currPounceCharge = 0;
    private BoardTile target = null;

    public override bool Move(int newXPos, int newYPos)
    {
        // For hunter, use the special jump lerp movement routine
        BoardTile targetTile;
        if (board.theBoard.TryGetValue((newXPos, newYPos), out targetTile))
        {
            // Move the piece according to the key. If you can't find a key, then do just a simple move. 
            List<BoardTile> placesToMove;
            if (!MovePaths.TryGetValue(targetTile, out placesToMove))
            {
                placesToMove = new List<BoardTile> { targetTile };
            }

            // Update the board
            // Todo, if there is a piece there, where do you end up?
            board.allPieces.Remove((xPos, yPos));
            xPos = placesToMove.Last().xCoord;
            yPos = placesToMove.Last().yCoord;
            if (board.allPieces.ContainsKey((placesToMove.Last().xCoord, placesToMove.Last().yCoord)))
            {
                // If you are moving somewhere, it's assumed that you already passed the check that something is there or not.
                // If something is there, kick it out, you are going to be there soon. 
                board.allPieces.Remove((placesToMove.Last().xCoord, placesToMove.Last().yCoord));
            }
            board.allPieces.Add((xPos, yPos), this);

            // Face wher we are going
            FaceObject(targetTile.transform);

            float startDelay = owner == CurrentTurn.Zombie ? Random.Range(0f, 1f) : 0;
            StartCoroutine(DoPieceJumpMovement(placesToMove, jumpHeight, startDelay, 0f));
            return true;
        }
        else
        {
            return false;
        }
    }

    public override List<BoardTile> PreviewAttack()
    {
        List<BoardTile> validMoves = GetAllValidMoveTiles();
        List<BoardTile> reuslt = new List<BoardTile>();
        MoveablePiece enemy;
        foreach (BoardTile tile in validMoves)
        {
            if(board.allPieces.TryGetValue((tile.xCoord, tile.yCoord), out enemy) && enemy.owner != owner)
            {
                reuslt.Add(tile);
            }
        }

        return reuslt;
    }

    public override List<BoardTile> PreviewMove()
    {
        List<BoardTile> result = GetAllValidMoveTiles();
        return result.Where(x => !board.allPieces.ContainsKey((x.xCoord, x.yCoord))).ToList();
    }

    public void ZombieAiAction()
    {
        // If you see something that can be attacked, start to charge. If it's still there after charge, then attack it.
        List<BoardTile> attacks = PreviewAttack();
        List<BoardTile> moves = PreviewMove();

        if (target != null && !attacks.Contains(target))
        {
            // No longer see target. Going to clear out and do something else this turn
            target = null;
        }

        if (target == null && attacks.Count > 0)
        {
            // Somethig is in range, and we don't have a target. Set it as the target and start the coutdown
            int idx = Random.Range(0, attacks.Count);
            target = attacks[idx];
            currPounceCharge = 0;
        }
        else if (target == null && moves.Count > 0)
        {
            // Nothing is in range, and we don't have at arget. But we can move. So pick a place and move. 
            // Lets move randomly for now for increased chaos
            int idx = Random.Range(0, moves.Count);
            BoardTile targetTile = moves[idx];
            Move(targetTile.xCoord, targetTile.yCoord);
            board.boardAudioController.PlayOneShot(board.zombieHunterLeap, 1.0f);
        }
        else if(target != null && attacks.Contains(target) && currPounceCharge < pounceCharge)
        {
            // Still charging
            currPounceCharge++;
        }
        else if (target != null && attacks.Contains(target))
        {
            // time to kill
            Attack(target.xCoord, target.yCoord);
            board.boardAudioController.PlayOneShot(board.zombieHunterAttack, 1.0f);
        }
        // Other wise, you have no moves you can do
    }

    private List<BoardTile> GetAllValidMoveTiles()
    {
        // Hunters can only move in a 3 square radius of itself.
        List<BoardTile> result = new List<BoardTile>();

        BoardTile tile;
        for (int xCoord = -jumpRadius; xCoord < jumpRadius; xCoord++)
        {
            int yCoord = jumpRadius - Mathf.Abs(xCoord);
            if (board.theBoard.TryGetValue((xCoord + xPos, yPos + yCoord), out tile))
            {
                result.Add(tile);
            }
            if (board.theBoard.TryGetValue((xCoord + xPos, yPos - yCoord), out tile))
            {
                result.Add(tile);
            }
        }
        return result;
    }

    void FixedUpdate()
    {
        if(target != null)
        {
            // face the target
            FaceObject(target.transform);
        }
    }

    private void FaceObject(Transform obj)
    {
        Vector3 direction = obj.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Euler(0f, rotation.eulerAngles.y + 180, 0f);
    }
}
