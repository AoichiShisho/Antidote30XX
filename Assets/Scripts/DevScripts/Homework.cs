using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Homework : MonoBehaviour
{
    private List<KeyCode> keySequence = new List<KeyCode>() { KeyCode.UpArrow, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.B, KeyCode.A, KeyCode.Return };
    private Sprite absolutelyNothing;
    public static bool IsItJebTimeYet = false;
    private Queue<KeyCode> currentSequence;

    private void Jeb()
    {
        GameObject.Find("Background").GetComponent<Image>().sprite = absolutelyNothing;
        GameObject.Find("Buttons").transform.Translate(new Vector3(0, 200, 0));
        IsItJebTimeYet = true;
    }

    private void Start()
    {
        currentSequence = new Queue<KeyCode>(keySequence);
        absolutelyNothing = Resources.Load<Sprite>("Sprites/Legal");
    }

    private void Update()
    {
        if (currentSequence.Count > 0)
        {
            KeyCode nextKey = currentSequence.Peek();

            if (Input.GetKeyDown(nextKey))
            {
                currentSequence.Dequeue();
            }
        }
        else
        {
            Jeb();
            currentSequence = new Queue<KeyCode>(keySequence);
        }
    }
}
