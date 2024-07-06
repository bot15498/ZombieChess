using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Bishop : MonoBehaviour, IMoveablePiece
{
    [SerializeField]
    private int _xPos = -1;
    [SerializeField]
    private int _yPos = -1;
    [SerializeField]
    private Board board;

    public int xPos { get => _xPos; set => _xPos = value; }
    public int yPos { get => _yPos; set => _yPos = value; }
    public int health { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public int maxHealth { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public int numActions { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public bool canAct { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public CurrentTurn owner { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    void Start()
    {
        UpgradeManager.current.ActivateBishopUpgrade += BishopUpgrade;
    }

    void Update()
    {

    }
    void BishopUpgrade(int id)
    {
        switch (id)
        {
            case 0:
                //upgrade stuff goes here upgrade id 0
                Debug.Log("AAAAAAAA");
                break;

            case 1:
                //upgrade stuff goes here upgrade id 1 
                break;

            default:

                break;
        }
    }

    public bool Move(int newXPos, int newYPos)
    {
        // Return true if this is a valid move, other wise return false;
        return false;
    }

    public bool Spawn(Board board, int xPos, int yPos, CurrentTurn owner)
    {
        this.board = board;
        this.xPos = xPos;
        this.yPos = yPos;
        this.owner = owner;
        return true;
    }

    public bool PreviewMove(out List<int> newXPos, out List<int> newYPos)
    {

        newXPos = new List<int> { xPos };
        newYPos = new List<int> { yPos };

        return true;
    }
}
