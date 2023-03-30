using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogBox : IUIElement<DialogBox>
{
    private TMP_Text nameText;
    private TMP_Text speechText;

    public override void Awake()
    {
        Instance = this;
        UIElement = GameObject.Find("SpeechBubble");
        nameText = GameObject.Find("NameText").GetComponent<TMP_Text>();
        speechText = GameObject.Find("DialogueText").GetComponent<TMP_Text>();
    }

    public void SetDialogBox(DialogEntry values)
    {
        nameText.text = values.speaker;
        if (values.isThought) {
            speechText.fontStyle |= FontStyles.Italic;
        } else {
            speechText.fontStyle &= ~FontStyles.Italic;
        }
        speechText.text = values.line;
    }
}