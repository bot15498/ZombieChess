using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;


public class Shambler : MoveablePiece, IZombiePiece

{
    public override List<BoardTile> PreviewAttack()
    {
        throw new System.NotImplementedException();
    }

    public override List<BoardTile> PreviewMove()
    {
        throw new System.NotImplementedException();
    }

    public BoardTile ZombieAiAction()
    {
        throw new System.NotImplementedException();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
