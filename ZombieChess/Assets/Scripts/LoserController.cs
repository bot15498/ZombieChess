using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoserController : MonoBehaviour
{
    // Start is called before the first frame update
    Cameramanager cm;

    public Animator zomanim;
    public Animator anim;
    public Animator loseanim;
    void Start()
    {
        cm = GetComponent<Cameramanager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ITSLOSERTIME()
    {
        cm.lose();
        zomanim.Play("Zombie_Lunge");
        anim.Play("losecamfall");
        loseanim.Play("losemenuopen");
    }
}
