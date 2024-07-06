using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileHighlightType
{
    Move,
    Attack
}

public class BoardTile : MonoBehaviour
{
    [SerializeField]
    private int xCoord = 0;
    [SerializeField]
    private int yCoord = 0;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetCoord(int x, int y)
    {
        xCoord = x;
        yCoord = y;
    }

    public void Highlight(TileHighlightType highLightType)
    {
        switch(highLightType)
        {
            case TileHighlightType.Move:
                break;
            case TileHighlightType.Attack:
                break;
        }
    }
}
