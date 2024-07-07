using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileHighlightType
{
    Move,
    Attack,
    Idle
}

public class BoardTile : MonoBehaviour
{
    public int xCoord = 0;
    public int yCoord = 0;
    public bool canBeOccupied = true;
    private Color startMaterialColor;

    void Start()
    {
        startMaterialColor = GetComponent<MeshRenderer>().material.color;
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
                GetComponent<MeshRenderer>().material.color = Color.blue;
                break;
            case TileHighlightType.Attack:
                GetComponent<MeshRenderer>().material.color = Color.red;
                break;
            case TileHighlightType.Idle:
                GetComponent<MeshRenderer>().material.color = startMaterialColor;
                break;
        }
    }
}
