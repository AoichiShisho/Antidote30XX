using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

[System.Serializable]
public class DialogEntry
{
    public string speaker;
    public string line;
    public bool isThought;

    public DialogEntry(string speaker, string line, bool isThought)
    {
        this.speaker = speaker;
        this.line = line;
        this.isThought = isThought;
    }
}

[System.Serializable]
public class DialogHolder
{
    public List<DialogEntry> lines;
    public bool isRepeating;

    public DialogHolder(List<DialogEntry> lines, bool isRepeating)
    {
        this.lines = lines;
        this.isRepeating = isRepeating;
    }
}

public class IDialogue : IInteractable {
    public Dialog dialogId;
    public List<DialogHolder> dialogHolder;
    public UnityEvent onComplete;
    public List<NPC> npcsToTeleport;
    public Transform npcTeleportLocation;
    public Transform playerTeleportLocation;
    public int playerTeleportDelay;
    public float interactCooldown = 2f;

    private int currentLine, currentDialog;
    private float currentCooldown = 0f;

    public virtual void Start()
    {
        currentLine = 0;
        currentDialog = 0;
    }

    public override void DoInteraction()
    {
        if (currentCooldown > 0f) return;
        base.DoInteraction();
        if (dialogHolder == null) return;
        if (currentDialog < dialogHolder.Count && currentLine < dialogHolder[currentDialog].lines.Count)
        {
            DialogBox.Instance.Show();
            DialogBox.Instance.SetDialogBox(dialogHolder[currentDialog].lines[currentLine++]);
        } else {
            StopInteraction();
            DialogManager.Instance.AddDialog(dialogId);
        }
    }

    public override void StopInteraction()
    {
        if (currentCooldown > 0f) return;
        currentCooldown = interactCooldown;
        DialogBox.Instance.Hide();
        currentLine = 0;
        if (!dialogHolder[currentDialog].isRepeating && currentDialog < dialogHolder.Count - 1)
            currentDialog += 1;
        if (npcsToTeleport != null && npcTeleportLocation != null) {
            NPCMovementController.Instance.MoveNPCsToLocation(npcsToTeleport, npcTeleportLocation);
        } 
        if (playerTeleportLocation != null) 
        {
            if (playerTeleportDelay != 0) {
                PlayerMovement.Instance.WaitAndTeleport(playerTeleportDelay, playerTeleportLocation);
            } else {
                PlayerMovement.Instance.Teleport(playerTeleportLocation);
            }
        }
        if (onComplete != null) onComplete.Invoke();
        PlayerMovement.Instance.SetFrozenStatus(false);
    }

    public void NextDialog()
    {
        StopInteraction();
        currentDialog++;
    }

    public override void Update()
    {
        if (currentCooldown > 0) {
            currentCooldown -= Time.deltaTime;
        }

        base.Update();
    }
}