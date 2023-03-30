using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractSign : IUIElement<InteractSign>
{
    public GameObject allowed, notAllowed;

    public override void Awake()
    {
        Instance = this;
        UIElement = allowed;
        notAllowed.SetActive(false);
    }
    
    public void MakeForbidden()
    {
        SwapTo(notAllowed);
    }

    public void MakeAllowed()
    {
        SwapTo(allowed);
    }

    private void SwapTo(GameObject go)
    {
        if (UIElement == go) return;
        bool wasVisible = IsVisible();
        if (wasVisible) Hide();
        UIElement = go;
        if (wasVisible) Show();
    }
}