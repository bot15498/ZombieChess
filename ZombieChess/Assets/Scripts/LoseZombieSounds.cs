using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseZombieSounds : MonoBehaviour
{

    public AudioClip startAudioclip;
    public AudioClip lungeclip;
    public AudioClip falling;
    public AudioClip zombieeat;
    AudioSource asource;
    // Start is called before the first frame update
    void Start()
    {
        asource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void zombiestartaudio()
    {
        //asource.PlayOneShot(startAudioclip, 1.0f);


        asource.clip = startAudioclip;
        asource.pitch = 0.4f;
        asource.Play();
    }

    public void playLungeSound()
    {
        asource.PlayOneShot(lungeclip, 1.0f);
    }

    public void playfallsound()
    {
        asource.PlayOneShot(falling, 1.0f);
    }

    public void eat()
    {
        asource.PlayOneShot(zombieeat, 1.0f);
    }



}
