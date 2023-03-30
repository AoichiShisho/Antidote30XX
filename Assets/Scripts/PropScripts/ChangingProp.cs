using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ChangingProp : MonoBehaviour
{
    public GameObject original;
    public GameObject alternateVersion;
    public bool displayAlternateVersion;

    public void Start()
    {
        original.SetActive(true);
        alternateVersion.SetActive(false);
    }

    public void SwitchToAlternate()
    {
        original.SetActive(false);
        alternateVersion.SetActive(true);
    }

    public void SwitchToOriginal()
    {
        original.SetActive(true);
        alternateVersion.SetActive(false);
    }
}

[CustomEditor(typeof(ChangingProp))]
public class ChangingPropEditor : Editor
{
    private ChangingProp prop;

    void OnEnable()
    {
        prop = (ChangingProp) target;
    }

    void OnSceneGUI()
    {
        if (prop.displayAlternateVersion)
        {
            prop.SwitchToAlternate();
        } else 
        {
            prop.SwitchToOriginal();
        }
    }
}