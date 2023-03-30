using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Puzzle : IUIElement<Puzzle>
{
    public UnityEvent onComplete;
    private bool isSolved = false;

    public bool IsSolved()
    {
        return isSolved;
    }

    // attach this as a callback to the solving action
    public void Solve()
    {
        Destroy(gameObject);
        if (onComplete != null) onComplete.Invoke();
        isSolved = true;
    }

    public override void Awake()
    {
        Instance = this;
        UIElement = gameObject;
    }
}
