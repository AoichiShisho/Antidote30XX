using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicController : AudioEffect
{
    public void StartMusic()
    {
        GetComponent<AudioSource>().loop = true;
        base.Play();
    }
}
