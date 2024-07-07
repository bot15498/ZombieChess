using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshClickDetector : MonoBehaviour
{
    public bool canClickPiece = false;
    public bool canClickTile = false;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousepos = Input.mousePosition;
        Ray ray = mainCamera.ScreenPointToRay(mousepos);
        RaycastHit hit;
        bool inline = Physics.Raycast(ray, out hit);

        if (inline && Input.GetMouseButtonDown(0))
        {
            if(canClickPiece)
            {
                // Clicking a piece to select it to move
                IMoveablePiece piece = hit.transform.GetComponent<IMoveablePiece>();
                if (piece != null)
                {
                    BoardStateManager.current.PieceSelectedForMovement(piece);
                }
            }
            else if (canClickTile)
            {
                // click a tile or a enemy to move to.
                // When you are here, if you click a piece with a IMoveablePiece on it, check if it's an enemy, if it is, then use that instead. 
                IMoveablePiece piece = hit.transform.GetComponent<IMoveablePiece>();
                BoardTile tile = hit.transform.GetComponent<BoardTile>();
                if(piece != null)
                {
                    BoardStateManager.current.TileSelectedForMovement(piece.xPos, piece.yPos);
                }
                else if (tile != null)
                {
                    BoardStateManager.current.TileSelectedForMovement(tile.xCoord, tile.yCoord);
                }
            }
        }
        Debug.DrawRay(ray.origin, ray.direction * 9999, Color.red);
    }
}
