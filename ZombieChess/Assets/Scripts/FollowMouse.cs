using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{

    Ray ray;
    RaycastHit hitData;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 mousepos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousepos);
        RaycastHit hit;

        if(Physics.Raycast(ray,out hit))
        {
            gameObject.transform.position = new Vector3(hit.transform.position.x + offset.x,gameObject.transform.position.y + offset.y, hit.transform.position.z + offset.z);
        }
       

    }
}
