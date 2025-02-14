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
    public Material highlightblue;
    public Material highlightred;
    private Material defaultmat;
    private MeshRenderer mr;

    void Start()
    {
        startMaterialColor = GetComponent<MeshRenderer>().material.color;
        mr = GetComponent<MeshRenderer>();
        defaultmat = mr.material;
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
        switch (highLightType)
        {
            case TileHighlightType.Move:
                //GetComponent<MeshRenderer>().material.color = ApplyColorTint(startMaterialColor, Color.blue);
                mr.material = highlightblue;
                break;
            case TileHighlightType.Attack:
                //GetComponent<MeshRenderer>().material.color = ApplyColorTint(startMaterialColor, Color.red);
                mr.material = highlightred;
                break;
            case TileHighlightType.Idle:
                //GetComponent<MeshRenderer>().material.color = startMaterialColor;
                mr.material = defaultmat;
                break;
        }
    }

    private Color ApplyColorTint(Color incolor, Color tint)
    {
        float red = Mathf.Clamp((tint.r * 2.5f - 0.5f) * incolor.r + incolor.r, 0f, 1f);
        float green = Mathf.Clamp((tint.g * 2.5f - 0.5f) * incolor.g + incolor.g, 0f, 1f);
        float blue = Mathf.Clamp((tint.b * 2.5f - 0.5f) * incolor.b + incolor.b, 0f, 1f);
        return new Color(red, green, blue);
    }
}
