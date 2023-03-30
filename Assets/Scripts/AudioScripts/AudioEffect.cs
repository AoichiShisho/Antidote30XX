using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEffect : MonoBehaviour
{
    private bool isPlayed = false;

    public void Play()
    {
        if (!isPlayed) {
            isPlayed = true;
            GetComponent<AudioSource>().Play();
        }
    }
}
