using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovementController : MonoBehaviour
{
    public static NPCMovementController Instance;

    void Awake()
    {
        Instance = this;
    }

    public void MoveGuardsToLab()
    {
        GameObject dormsGuard = GameObject.Find("DormsGuard");
        GameObject cafeteriaGuard = GameObject.Find("CafeteriaGuard");
        GameObject location = GameObject.Find("GuardsLabLocation");

        dormsGuard.transform.position = location.transform.position;
        cafeteriaGuard.transform.position = location.transform.position + new Vector3(1, 0, 1);
    }

    public void MoveNPCsToLocation(List<NPC> npcs, Transform location)
    {
        foreach (NPC npc in npcs)
        {
            npc.transform.position = location.position + new Vector3(Random.Range(0,1f), 0, Random.Range(0, 1f));
        }
    }
}
