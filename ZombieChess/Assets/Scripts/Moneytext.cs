using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Moneytext : MonoBehaviour
{


     TextMeshProUGUI cashtext;
    public UpgradeManager um;
    // Start is called before the first frame update


    void Start()
    {
        cashtext = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        cashtext.text = um.zombieBucks.ToString();
    }
}
