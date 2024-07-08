using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenShop : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject shop;
    Cameramanager CM;
    void Start()
    {
        CM = GameObject.FindGameObjectWithTag("Gamemanager").GetComponent<Cameramanager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseOver()
    {

        //do OnMouseOver animation
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CM.activateBookCam();
            StartCoroutine(ExampleCoroutine());
        }
    }

    public void OnMouseExit()
    {
        //do exit stuff
    }

    IEnumerator ExampleCoroutine()
    {

        yield return new WaitForSeconds(2.5f);

        //After we have waited 5 seconds print the time again.

        shop.SetActive(true);
    }
}
