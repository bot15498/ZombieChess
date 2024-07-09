using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Randomblood : MonoBehaviour
{

    public Material[] bloodDecals;

    public DecalProjector dp;
    // Start is called before the first frame update
    void Start()
    {
        dp = GetComponent<DecalProjector>();
        dp.material = bloodDecals[Random.Range(0, bloodDecals.Length)];
        transform.Rotate(0.0f, 0.0f, Random.Range(0f, 360f));
        

    }

    // Update is called once per frame
    void Update()
    {
        

    }
}
