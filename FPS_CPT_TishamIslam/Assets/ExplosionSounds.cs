using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSounds : MonoBehaviour
{
    public float reduction;

    void Start()
    {
        //get audio component, set it up and then play it
        AudioSource aud = GetComponent<AudioSource>();
        aud.volume = Options.Volume / reduction;
        aud.Play();
    }
}
