using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrategicVision : MonoBehaviour
{
    // Start is called before the first frame update
    public Cameramanager cm;
    public Material[] Button;
    MeshRenderer mr;

    void Start()
    {
        mr = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnMouseOver()
    {
        mr.material = Button[1];
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            cm.toggleStrategicCam();
        }
    }

    private void OnMouseExit()
    {
        mr.material = Button[0];
    }
}
