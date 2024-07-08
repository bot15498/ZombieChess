using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseBook : MonoBehaviour
{
    public GameObject book;
    Cameramanager cm;
    // Start is called before the first frame update
    void Start()
    {
        cm = GameObject.FindGameObjectWithTag("Gamemanager").GetComponent<Cameramanager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void closeBook()
    {
        book.SetActive(false);
        cm.activateFPSCam();
    }
}
