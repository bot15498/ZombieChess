using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int zomkills;
    private BoardStateManager bm;
    public TextMeshProUGUI scoretext;
    void Start()
    {
        zomkills = 0;
       
        bm = GetComponent<BoardStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        increaseTurnCount();
    }

    public void increaseZomkills()
    {
        zomkills += 1;
    }

    public void increaseTurnCount()
    {

        scoretext.text = "Survived: " + bm.turnCount.ToString();
    }
}
