using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class PropTimeline 
{
    public TimePeriod period;
    public GameObject model;
}

public class TimeTravelProp : MonoBehaviour, ITimeTravels
{
    public PropTimeline[] timelines = new PropTimeline[1];
    public TimePeriod period;

    void Start()
    {
        SetTimePeriod(TimeTravelController.Instance.GetCurrentPeriod()); // should always be TimePeriod.INTRO
    }

    public void SetTimePeriod(TimePeriod timePeriod)
    {
        foreach (PropTimeline timeline in timelines)
        {
            if (timeline.model == null) continue;
            if (timeline.period == timePeriod)
            {
                timeline.model.SetActive(true);
            } else {
                timeline.model.SetActive(false);
            }
        }
    }

    public void OnDrawGizmosSelected()
    {
        foreach (TimePeriod period in TimePeriod.GetValues(typeof(TimePeriod)))
        {
            foreach (PropTimeline timeline in timelines) {
                if (timeline.model == null) continue;
                if (timeline.period == period) {
                    GameObject item = timeline.model;
                    Gizmos.color = Color.Lerp(Color.red, Color.green, (int) period / (float) (int) TimePeriod.INTRO);
                    Gizmos.DrawSphere(item.transform.position, .1f);
                }
            }
        }
    }
}

[CustomEditor(typeof(TimeTravelProp))]
public class TimeTravelPropEditor : Editor
{
    private TimeTravelProp prop;

    void OnEnable()
    {
        prop = (TimeTravelProp) target;
    }

    void OnSceneGUI()
    {
        prop.SetTimePeriod(prop.period);
    }
}