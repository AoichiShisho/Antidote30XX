using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : IDialogue
{
    public bool IsOneTimeTrigger = true, ShouldPlayerFreeze = true;
    private bool IsRunning = false, HasFrozen = false;
    [System.NonSerialized] public bool IsInteracted = false;

    private bool IsPlayerAdvancingDialog() {
        return (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E));
    }

    public override void DoInteraction()
    {
        if (requirements.MeetsRequirements() && (!IsOneTimeTrigger || (IsOneTimeTrigger && !IsInteracted))){
            IsRunning = true;
            base.DoInteraction();
            PlayerMovement.Instance.SetFrozenStatus((!HasFrozen || IsRunning) && ShouldPlayerFreeze);
            HasFrozen = true;
        }
    }

    public override void StopInteraction()
    {
        base.StopInteraction();
        IsRunning = false;
        IsInteracted = true;
        PlayerMovement.Instance.SetFrozenStatus(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        DoInteraction();
    }

    public void OnTriggerExit(Collider other)
    {
        HasFrozen = false;
    }

    public new void OnCollisionExit(Collision collision)
    {
        base.OnCollisionExit(collision);
        HasFrozen = false;
    }

    new void Update()
    {
        if ((!IsRunning && isPlayerInContact) || (IsRunning && IsPlayerAdvancingDialog())) DoInteraction();
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(Trigger))]
class TriggerEditor : Editor
{
    private Trigger trigger;
    private BoxCollider boxCollider;

    private void OnEnable()
    {
        trigger = (Trigger) target;
        boxCollider = trigger.GetComponent<BoxCollider>();
    }

    private void OnSceneGUI()
    {
        if (boxCollider != null)
        {
            Bounds bounds = boxCollider.bounds;

            Handles.color = Color.green;
            Handles.DrawWireCube(bounds.center, bounds.size);
        }
    }
}

#endif