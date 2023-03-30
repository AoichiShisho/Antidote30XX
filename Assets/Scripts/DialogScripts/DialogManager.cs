using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    private List<Dialog> completedDialogList = new List<Dialog>();
    public static DialogManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public void AddDialog(Dialog dialog)
    {
        if (!completedDialogList.Contains(dialog)) {
            Debug.Log("Adding " + dialog + " to dialog list.");
            completedDialogList.Add(dialog);
        }
    }

    public bool HasCompletedDialog(Dialog dialog)
    {
        return completedDialogList.Contains(dialog);
    }

    public List<Dialog> GetCompletedDialog()
    {
        return completedDialogList;
    }
}
