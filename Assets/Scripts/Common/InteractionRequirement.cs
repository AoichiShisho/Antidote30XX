using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InteractionRequirement {
    public List<ItemProperties> requiredItems = new List<ItemProperties>();
    public List<TimePeriod> availablePeriods = new List<TimePeriod>();
    public List<Dialog> requiredDialog = new List<Dialog>();
    public List<Dialog> untilDialog = new List<Dialog>();

    public static InteractionRequirement None()
    {
        return new InteractionRequirement(new List<ItemProperties>(), 
            new List<TimePeriod>{TimePeriod.INTRO, TimePeriod.ONE_DAY_AGO, TimePeriod.TWO_YRS_AGO, TimePeriod.FIVE_YRS_AGO, TimePeriod.SEVEN_YRS_AGO}, new List<Dialog>());
    }

    private InteractionRequirement(List<ItemProperties> items, List<TimePeriod> periods, List<Dialog> dialogs) {
        this.requiredItems = items;
        this.availablePeriods = periods;
        this.requiredDialog = dialogs;
    }

    private static bool ContainsAll<T>(List<T> a, List<T> b)
    {
        foreach (T t in a)
        {
            if (!b.Contains(t)) return false;
        }
        return true;
    }

    private static bool ContainsAny<T>(List<T> a, List<T> b)
    {
        foreach (T t in a)
        {
            if (b.Contains(t)) {
                return true;
            }
        }
        return false;
    }

    public bool MeetsRequirements()
    {
        return availablePeriods.Contains(TimeTravelController.Instance.GetCurrentPeriod()) &&
            ContainsAll(requiredItems, InventoryManager.Instance.Items) &&
            ContainsAll(requiredDialog, DialogManager.Instance.GetCompletedDialog()) &&
            !ContainsAny(untilDialog, DialogManager.Instance.GetCompletedDialog());
    }
}