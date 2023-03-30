using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnalogKeypadPuzzle : Puzzle
{
    public Transform buttons;
    public Transform indicators;
    public string answer = "1104";
    private string currentGuess = "";
    private bool noGuessing = false;
    public static readonly int FLASH_COUNT = 3;
    public static readonly float FLASH_DURATION = 0.4f;
    public Color originalIndicatorColor;

    private void InitButtons()
    {
        foreach (Transform input in buttons)
        {
            Button inputButton = input.gameObject.GetComponent<Button>();
            inputButton.onClick.AddListener(() => AddToGuess(input.name));
        }
    }

    private void AddToGuess(string newNum)
    {
        if (noGuessing) return;
        currentGuess += newNum;
    }

    private void MakeGuess()
    {
        StartCoroutine(
            currentGuess == answer ? RightAnswer() : WrongAnswer()
        );
    }

    private void SetIndicatorColors(Color color)
    {
        foreach (Transform indicator in indicators)
        {
            Image indicatorImage = indicator.GetComponent<Image>();
            indicatorImage.color = color;
        }
    }

    private void SetIndicatorStatus()
    {
        int i = 0;
        foreach (Transform indicator in indicators)
        {
            Image indicatorImage = indicator.GetComponent<Image>();
            if (i++ < currentGuess.Length)
            {
                indicatorImage.color = Color.yellow;
            } else
            {
                indicatorImage.color = originalIndicatorColor;
            }
        }
    }

    private IEnumerator WrongAnswer()
    {
        noGuessing = true;
        
        for (int i = 0; i < FLASH_COUNT; i++) {
            yield return new WaitForSeconds(FLASH_DURATION);
            SetIndicatorColors(Color.red);
            yield return new WaitForSeconds(FLASH_DURATION);
            SetIndicatorColors(originalIndicatorColor);
        }

        currentGuess = "";
        noGuessing = false;

        GameObject.Find("IncorrectAnalogKeypadTrigger").GetComponent<Trigger>().DoInteraction();
        yield break;
    }

    private IEnumerator RightAnswer()
    {
        noGuessing = true;
        SetIndicatorColors(Color.green);
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
        if (currentGuess.Length >= answer.Length && !noGuessing)
        {
            MakeGuess();
        }

        if (!noGuessing) SetIndicatorStatus();
    }
}
