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
    bool isStrategic;
    bool bookisopen;


    void Start()
    {
        isStrategic = false;
        bookisopen = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void closebook()
    {
        bookisopen = false;
    }

    public void toggleStrategicCam()
    {
        if (bookisopen == false)
        {
            isStrategic = !isStrategic;
            ActivateMainCam();
        }
    }

    public void ActivateMainCam()
    {
        if (isStrategic == false) { 
        strategicCam.SetActive(false);
        FirstPersoncam.SetActive(true);
        enemyCam.SetActive(false);
        BookCam.SetActive(false);
        }else if(isStrategic == true)
        {
            strategicCam.SetActive(true);
            FirstPersoncam.SetActive(false);
            enemyCam.SetActive(false);
            BookCam.SetActive(false);
        }
       
    }

    public void activateBookCam()
    {
        strategicCam.SetActive(false);
        FirstPersoncam.SetActive(false);
        enemyCam.SetActive(false);
        BookCam.SetActive(true);
        bookisopen = true;

    }

    public void activatEnemyCam()
    {
        strategicCam.SetActive(false);
        FirstPersoncam.SetActive(false);
        enemyCam.SetActive(true);
        BookCam.SetActive(false);
    }



}
