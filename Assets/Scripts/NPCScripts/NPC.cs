using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

[System.Serializable]
public class Timeline {
    public Transform location;
    public TimePeriod period;
    public IDialogue dialogue;
}

public class NPC : IInteractable, ITimeTravels
{
    public Timeline[] timelines = new Timeline[TimeTravelController.GetNumberOfPeriods()];
    private IDialogue currentDialog;

    public void SetTimePeriod(TimePeriod newPeriod)
    {
        Timeline newTimeline = null;
        foreach (Timeline timeline in timelines)
        {
            if (timeline.period == newPeriod) {
                newTimeline = timeline;
            }
        }
        
        if (newTimeline == null) {
            // team rocket blasting off once again
            transform.position = new Vector3(10000, 10000, 10000);
            return;
        }
        
        if (newTimeline.location != null) {
            transform.position = newTimeline.location.position;
        }

        currentDialog = newTimeline.dialogue;
        if (currentDialog == null) {
            requirements = InteractionRequirement.None();
        } else {
            requirements = currentDialog.requirements;
        }
    }

    public override void DoInteraction()
    {
        if (currentDialog != null) currentDialog.DoInteraction();
    }

    public override void StopInteraction()
    {
        if (currentDialog != null) currentDialog.StopInteraction();
    }

    protected override void OnCollisionStay(Collision collision)
    {
        if (currentDialog != null)
            base.OnCollisionStay(collision);
    }

    public void NextDialog()
    {
        if (currentDialog != null) currentDialog.NextDialog();
    }
}