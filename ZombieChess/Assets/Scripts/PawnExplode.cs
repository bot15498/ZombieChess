using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PawnExplode : MonoBehaviour
{
    // Start is called before the first frame update
    float timer = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= 2)
        {
            Destroy(gameObject);
        }
    }
}
