using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.UIElements;

public class Book : MonoBehaviour
{

    public float pageSpeed = 0.5f;
    public List<Transform> pages;
    int index = -1;
    bool rotate = false;
    public GameObject frontbutton;
    public GameObject backbutton;

    public AudioSource bookAudioController;
    public AudioClip pageFlipping;

    // Start is called before the first frame update
    public void InitialState()
    {
        for(int i = 0; i < pages.Count; i++)
        {
            pages[i].transform.rotation = Quaternion.identity;

        }
        pages[0].SetAsLastSibling();
        backbutton.SetActive(false);
    }

    public void RotateNext()
    {
        if (rotate == true)
        {
            return;
        }
        index++;
        float angle = -180;
        ForwardButtonActions();
        pages[index].SetAsLastSibling();
        this.bookAudioController.PlayOneShot(this.pageFlipping, 1.0f);
        StartCoroutine(Rotate(angle, true));
    }

    public void ForwardButtonActions()
    {
        if (backbutton.activeInHierarchy == false)
        {
            backbutton.SetActive(true);
        }
        if (index == pages.Count - 1)
        {
            frontbutton.SetActive(false);
        }
    }

    public void RotateBack()
    {
        if(rotate == true)
        {
            return;
        }
        float angle = 0;
        pages[index].SetAsLastSibling();
        BackbuttonActions();
        this.bookAudioController.PlayOneShot(this.pageFlipping, 1.0f);
        StartCoroutine(Rotate(angle, false));
    }

    public void BackbuttonActions()
    {
        if (frontbutton.activeInHierarchy == false)
        {
            frontbutton.SetActive(true);
        }
        if (index - 1 == -1)
        {
            backbutton.SetActive(false);
        }
    }

    IEnumerator Rotate(float angle, bool forward){
        float value = 0f;
        while (true)
        {
            rotate = true;
            Quaternion targetRotation = Quaternion.Euler(0, angle, 0);
            value += Time.deltaTime * pageSpeed;
            pages[index].rotation = Quaternion.Slerp(pages[index].rotation, targetRotation, value);
            float angle1 = Quaternion.Angle(pages[index].rotation, targetRotation);
            if(angle1< 0.1f)
            {
                if(forward == false)
                {
                    index--;
                }
                rotate = false;
                break;
            }
            yield return null;
        }
    }

    void Start()
    {
        InitialState();
    }

}
