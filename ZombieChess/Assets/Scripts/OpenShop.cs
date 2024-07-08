using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenShop : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject shop;
    void Start()
    {
        
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
            shop.SetActive(true);
        }
    }

    public void OnMouseExit()
    {
        //do exit stuff
    }
}
