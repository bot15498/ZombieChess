using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameramanager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject strategicCam;
    public GameObject FirstPersoncam;
    public GameObject enemyCam;
    public GameObject BookCam;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void activateStrategicCam()
    {
        strategicCam.SetActive(true);
        FirstPersoncam.SetActive(false);
        enemyCam.SetActive(false);
        BookCam.SetActive(false);
    }

    public void activateFPSCam()
    {
        strategicCam.SetActive(false);
        FirstPersoncam.SetActive(true);
        enemyCam.SetActive(false);
        BookCam.SetActive(false);
    }

    public void activateBookCam()
    {
        strategicCam.SetActive(false);
        FirstPersoncam.SetActive(false);
        enemyCam.SetActive(false);
        BookCam.SetActive(true);
    }

    public void activatEnemyCam()
    {
        strategicCam.SetActive(false);
        FirstPersoncam.SetActive(false);
        enemyCam.SetActive(true);
        BookCam.SetActive(false);
    }



}
