using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleTrigger : IInteractable
{
    public Puzzle puzzle;

    public override void DoInteraction()
    {
        if (puzzle.IsVisible()) {
            StopInteraction();
        } else {
            puzzle.Show();
            PlayerMovement.Instance.SetFrozenStatus(true);
        }
    }

    public override void StopInteraction()
    {
        puzzle.Hide();
        PlayerMovement.Instance.SetFrozenStatus(false);
    }

    protected override bool IsPlayerInteracting()
    {
        return !puzzle.IsSolved() && base.IsPlayerInteracting();
    }

    protected override void OnCollisionStay(Collision collision)
    {
        if (!puzzle.IsSolved()) base.OnCollisionStay(collision);
    }

    public override void Update()
    {
        base.Update();
        if (puzzle.IsSolved()) {
            PlayerMovement.Instance.SetFrozenStatus(false);
            InteractSign.Instance.Hide();
            Destroy(GetComponent<PuzzleTrigger>());
        }
    }
}