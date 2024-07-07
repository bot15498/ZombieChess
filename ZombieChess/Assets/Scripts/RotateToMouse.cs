using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RotateToMouse : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform invis;
    Transform thisobject;
    CinemachineVirtualCamera CM;
    public float yclamp;

    public Vector3 screenPosition;
    public Vector3 worldPosition;
    void Start()
    {
        CM = GetComponent<CinemachineVirtualCamera>();
        thisobject = gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {



        screenPosition = Input.mousePosition;
        screenPosition.z = Camera.main.nearClipPlane + 1;
        worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        invis.position = worldPosition;
   
            CM.LookAt = invis;
        

        //thisobject.eulerAngles.y = Mathf.Clamp(transform.eulerAngles.y, -90, 90);

    }



}
