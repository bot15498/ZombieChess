using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CloseBook : MonoBehaviour
{
    public GameObject book;
    public Animator anim;
    Cameramanager cm;
    public OpenShop bookcontrol;
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
        if (bookcontrol.bookisopen == true)
        {
            anim.SetBool("openbook", false);
            anim.SetBool("Hover", false);
            //book.SetActive(false);
            cm.activateFPSCam();
            bookcontrol.bookclose();
        }
    }
}
