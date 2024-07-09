using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenShop : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator anim;
    public GameObject shop;
    Cameramanager CM;
    public bool bookisopen;
    public Animator upgradeanim;

    void Start()
    {
        CM = GameObject.FindGameObjectWithTag("Gamemanager").GetComponent<Cameramanager>();
        bookisopen = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseOver()
    {

        //do OnMouseOver animation
        if (bookisopen == false)
        {
            anim.SetBool("Hover",true);
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CM.activateBookCam();
            bookisopen = true;
            StartCoroutine(ExampleCoroutine());

            anim.Play("Book_Open");
        }
    }

    public void OnMouseExit()
    {
        if (bookisopen == false)
        {
            anim.SetBool("Hover", false);
        }
        //do exit stuff
    }

    public void bookclose()
    {
        StartCoroutine(bookclosedelay());
        
    }


    IEnumerator ExampleCoroutine()
    {

        yield return new WaitForSeconds(0.5f);

        //After we have waited 5 seconds print the time again.

        
        shop.SetActive(true);
        
        if(bookisopen == true)
        {
            upgradeanim.Play("upgrade_open");
        }
        
    }

    IEnumerator bookclosedelay()
    {

        yield return new WaitForSeconds(0.25f);

        upgradeanim.Play("upgrade_close");

        //After we have waited 5 seconds print the time again.

        bookisopen = false;
        
    }
}


