using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : MoveablePiece, IZombiePiece
{
    public override List<BoardTile> PreviewAttack()
    {
        throw new System.NotImplementedException();
    }

    public override List<BoardTile> PreviewMove()
    {
        throw new System.NotImplementedException();
    }

    public void ZombieAiAction()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
