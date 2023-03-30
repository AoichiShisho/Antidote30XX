using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    private bool isPaused = false, playerWasFrozen;
    private Dictionary<AudioSource, bool> audioWasPlaying = new Dictionary<AudioSource, bool>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseStatus();
        }
    }

    public void TogglePauseStatus()
    {
        isPaused = !isPaused;
        Color color = GetComponent<Image>().color;
        color.a = isPaused ? .5f : 0f;
        GetComponent<Image>().color = color;
        if (isPaused)
            playerWasFrozen = PlayerMovement.Instance.IsPlayerFrozen();
        PlayerMovement.Instance.SetFrozenStatus(isPaused || playerWasFrozen);

        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.SetActive(isPaused);
        }

        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource source in audioSources)
        {
            if (isPaused) {
                audioWasPlaying[source] = source.isPlaying;
                if (source.isPlaying) source.Pause();
            } else {
                if (audioWasPlaying[source]) source.Play();
            }
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}