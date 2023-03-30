using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DigitalKeypadPuzzle : Puzzle
{
    public TextMeshProUGUI display;
    public Transform buttons;
    public Image emptyImage;
    public string answer = "0615";
    private string currentGuess = "";
    private bool noGuessing = false, screenOn = false;
    private int idleSecs = 0;
    public static readonly float FLASH_DURATION = .4f;
    public static readonly float FLASH_COUNT = 3;
    public static readonly float TRANSITION_SPEED = 3f;
    public static readonly float MAXIMUM_IDLE_SECS = 3f;
    private IEnumerator ScreenOnCheck;

    static float t = 1.0f;

    private void InitButtons()
    {
        Button screenOnButton = emptyImage.gameObject.GetComponent<Button>();
        screenOnButton.onClick.AddListener(TurnOn);

        foreach (Transform input in buttons)
        {
            Button inputButton = input.gameObject.GetComponent<Button>();
            inputButton.onClick.AddListener(() => AddToGuess(input.name));
        }
    }

    private void TurnOn()
    {
        if (ScreenOnCheck == null && gameObject.activeSelf) 
        {
            idleSecs = 0;
            screenOn = true;
            ScreenOnCheck = LivenessChecker();
            StartCoroutine(ScreenOnCheck);
        }
    }

    private void TurnOff()
    {
        if (ScreenOnCheck != null) {
            emptyImage.gameObject.SetActive(true);
            screenOn = false;
            StopCoroutine(ScreenOnCheck);
            ScreenOnCheck = null;
        }
    }

    private IEnumerator LivenessChecker()
    {
        while (true) {
            if (idleSecs++ > MAXIMUM_IDLE_SECS) 
            {
                TurnOff();
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private void AddToGuess(string newNum) 
    {
        if (noGuessing) return;
        idleSecs = 0;
        currentGuess += newNum;
    }

    private void MakeGuess() 
    {
        if (currentGuess == answer) {
            StartCoroutine(RightAnswer());
        } else if (!noGuessing) {
            StartCoroutine(WrongAnswer());
        }
    }

    private IEnumerator WrongAnswer() 
    {
        noGuessing = true;
        display.color = Color.red;
        
        for (int i = 0; i < FLASH_COUNT; i++) {
            yield return new WaitForSeconds(FLASH_DURATION);
            currentGuess = "XXXX";
            yield return new WaitForSeconds(FLASH_DURATION);
            currentGuess = "";
        }

        display.color = Color.white;
        noGuessing = false;
        yield break;
    }

    private IEnumerator RightAnswer() 
    {
        noGuessing = true;
        display.color = Color.green;
        yield return new WaitForSeconds(FLASH_DURATION);

        Solve();
        yield break;
    }

    new void Start()
    {
        InitButtons();
        Hide();
    }

    void Update()
    {
        Color color = emptyImage.color;
        color.a = Mathf.Lerp(0, 1, t);
        emptyImage.color = color;

        t += TRANSITION_SPEED * Time.deltaTime * (screenOn ? -1 : 1);
        t = Mathf.Min(t, 1);
        t = Mathf.Max(t, 0);

        // hack to check wrong answer animation isn't playing
        if (t == 0f && display.color != Color.red) {
            noGuessing = false;
            emptyImage.gameObject.SetActive(false);
        }


        if (screenOn && !noGuessing && currentGuess.Length >= answer.Length) {
            MakeGuess();
        }

        display.text = currentGuess;
    }
}
