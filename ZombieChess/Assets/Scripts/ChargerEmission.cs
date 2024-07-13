using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerEmission : MonoBehaviour
{
    // Start is called before the first frame update
    public bool ischargingup;
    float currentemissive;


    public Material glowbrain;
    void Start()
    {
        currentemissive = 100;
    }

    // Update is called once per frame
    void Update()
    {
        glowbrain.SetColor("_EmissiveColor", Color.red * currentemissive);
    }
}

