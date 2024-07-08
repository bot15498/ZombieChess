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
                GetComponent<MeshRenderer>().material.color = ApplyColorTint(startMaterialColor, Color.blue);
                break;
            case TileHighlightType.Attack:
                GetComponent<MeshRenderer>().material.color = ApplyColorTint(startMaterialColor, Color.red);
                break;
            case TileHighlightType.Idle:
                GetComponent<MeshRenderer>().material.color = startMaterialColor;
                break;
        }
    }

    private Color ApplyColorTint(Color incolor, Color tint)
    {
        float red = (tint.r - 0.5f) * incolor.r + incolor.r;
        float green = (tint.g - 0.5f) * incolor.g + incolor.g;
        float blue = (tint.b - 0.5f) * incolor.b + incolor.b;
        Debug.Log(incolor);
        Debug.Log(tint);
        Debug.Log($"{red}, {green}, {blue}");
        return new Color(red, green, blue); 
    }
}
