using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScripts : MonoBehaviour
{
    // Start is called before the first frame update
   public void menu()
    {
        SceneManager.LoadScene(0);
    }

    public void startover()
    {
        SceneManager.LoadScene(1);
    }
}
