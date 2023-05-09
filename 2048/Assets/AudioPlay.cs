using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlay : MonoBehaviour
{
    public AudioSource audio;
    public AudioClip[] clip;
    // Start is called before the first frame update
    void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        audio.PlayOneShot(clip[0]);
    }

    public void PlayZoomSound()
    {
        audio.PlayOneShot(clip[1]);
    }
}
