using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class AlternateItem
{
    public TimePeriod period;
    public Item item;
}

public class TravelItem : Item, ITimeTravels
{
    public TimePeriod CurrentPeriod;
    public List<AlternateItem> AlternateRepresenations;

    public void Start()
    {
        SetTimePeriod(CurrentPeriod);
    }

    public void SetTimePeriod(TimePeriod newPeriod)
    {
        foreach (AlternateItem alternateItem in AlternateRepresenations)
        {
            if (alternateItem.item == null) continue;
            if (alternateItem.period == newPeriod)
            {
                alternateItem.item.gameObject.SetActive(true);
                item = alternateItem.item.item;
                CurrentPeriod = newPeriod;
            } else
            {
                alternateItem.item.gameObject.SetActive(false);
            }
        }
    }

    public override void DoInteraction()
    {
        InventoryManager.Instance.Add(item);
        while (transform.childCount > 0) DestroyImmediate(transform.GetChild(0).gameObject);
        Destroy(gameObject);
    }

    public void OnDrawGizmosSelected()
    {
        foreach (TimePeriod period in TimePeriod.GetValues(typeof(TimePeriod)))
        {
            foreach (AlternateItem alternateItem in AlternateRepresenations)
            {
                if (alternateItem.item == null) continue;
                if (alternateItem.period == period)
                {
                    GameObject item = alternateItem.item.gameObject;
                    Gizmos.color = Color.Lerp(Color.red, Color.green, (int) period / (float) (int) TimePeriod.INTRO);
                    Gizmos.DrawSphere(item.transform.position, .1f);
                }
            }
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(TravelItem))]
public class TravelItemEditor : Editor
{
    void OnSceneGUI()
    {
        TravelItem obj = (TravelItem) target;
        
        foreach (AlternateItem alternateItem in obj.AlternateRepresenations)
        {
            if (alternateItem.item == null) continue;
            if (alternateItem.period == obj.CurrentPeriod)
            {
                alternateItem.item.gameObject.SetActive(true);
            } else 
            {
                alternateItem.item.gameObject.SetActive(false);
            }
        }
    }
}
#endif