using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BestiaryManager : MonoBehaviour
{
    // Start is called before the first frame update
    enum currentpage {page1,page2,page3,page4 };
    currentpage activepage;
    public Animator anim2;
    public Animator anim3;
    public Animator anim4;
    Animator anim;
    bool zombookopen;
    
    public Cameramanager cm;
    void Start()
    {
        activepage = currentpage.page1;
        anim = GetComponent<Animator>();
        zombookopen = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        anim.SetBool("ishover", true);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (cm.bookisopen == false)
            {
                zombookopen = true;
                cm.activatezombookcam();
            }

        }
    }

    public void closezombobook()
    {
        zombookopen = false;
    }

    private void OnMouseExit()
    {
        if (zombookopen == false)
        {
            anim.SetBool("ishover", false);
        }
    }

    public void activatePage1()
    {

        if (activepage == currentpage.page2)
        {
            anim2.Play("zompage2_close");
        }

        if (activepage == currentpage.page3)
        {
            anim3.Play("zompage3_close");
        }
        if (activepage == currentpage.page4)
        {
            anim4.Play("zompage4_close");
        }


        activepage = currentpage.page1;
    }

    public void activatePage2()
    {
        if(activepage != currentpage.page2)

        {
            anim2.Play("zompage2_open");

            if (activepage == currentpage.page3)
            {
                anim3.Play("zompage3_close");
            }
            if (activepage == currentpage.page4)
            {
                anim4.Play("zompage4_close");
            }
            activepage = currentpage.page2;
        }
    }

    public void activatePage3()
    {
        if (activepage != currentpage.page3)
        {
            anim3.Play("zompage3_open");
           
            if (activepage == currentpage.page2)
            {
                anim2.Play("zompage2_close");
            }

            if (activepage == currentpage.page4)
            {
                anim4.Play("zompage4_close");
            }
            activepage = currentpage.page3;

        }
    }

    public void activatePage4()
    {
        if (activepage != currentpage.page4)
        {
            anim4.Play("zompage4_open");
            

            if (activepage == currentpage.page2)
            {
                anim2.Play("zompage2_close");
            }

            if (activepage == currentpage.page3)
            {
                anim3.Play("zompage3_close");
            }
            activepage = currentpage.page4;
        }
    }
}
