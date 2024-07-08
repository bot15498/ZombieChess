using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseBook : MonoBehaviour
{
    public GameObject book;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void closeBook()
    {
        book.SetActive(false);
    }
}
