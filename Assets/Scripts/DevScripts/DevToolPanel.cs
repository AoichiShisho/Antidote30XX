using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevToolPanel : MonoBehaviour
{
    private Rect _panelRect = new Rect(Screen.width - 210, 10, 200, 100);
    private bool endOfIntro=false, farm=false, stalkberrySolve=false;

    void Start()
    {
        if (Homework.IsItJebTimeYet)
        {
            Jeb();
        }
    }

    private void Jeb()
    {
        Sprite sprite = Resources.Load<Sprite>("Sprites/Legal");
        AudioClip clip = Resources.Load<AudioClip>("Audio/audiotest");
        Renderer[] allRenderers = FindObjectsOfType<Renderer>();
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        IDialogue[] allDialogues = FindObjectsOfType<IDialogue>();
        
        foreach (Renderer renderer in allRenderers)
        {
            Material[] materials = renderer.sharedMaterials;
            
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].mainTexture = sprite.texture;
            }
        }

        foreach (AudioSource audioSource in allAudioSources)
        {
            if (!audioSource.loop) audioSource.clip = clip;
            else audioSource.gameObject.SetActive(false);
        }

        foreach (IDialogue dialogue in allDialogues)
        {
            dialogue.dialogHolder = new List<DialogHolder>(){new DialogHolder(new List<DialogEntry>(){ new DialogEntry("Jeb", "Please clap", false) }, false) };
        }
    }

    private void OnGUI()
    {
#if UNITY_EDITOR
        _panelRect = GUILayout.Window(GetInstanceID(), _panelRect, DrawDevToolWindow, "Dev Tools");
#endif
    }

    private void DrawDevToolWindow(int windowID)
    {
        if (GUILayout.Button("EndOfIntro"))
        {
            if (!endOfIntro) EndOfIntro();
            endOfIntro = true;
        }

        if (GUILayout.Button("Farm"))
        {
            if (!farm) Farm();
            farm = true;
        }

        if (GUILayout.Button("StalkberrySolve"))
        {
            if (!stalkberrySolve) StalkberrySolve();
            stalkberrySolve = true;
        }

        GUI.DragWindow();
    }
    
    private void OverrideTimePeriod(TimePeriod period)
    {

        TimePickerInterface ttc = GameObject.FindObjectsOfType<TimePickerInterface>(true)[0];
        ttc.Enable();

        TimeTravelController.Instance.SetTimePeriod(period);
    }

    private void GiveItemToPlayer(string itemObjectName)
    {
        GameObject itemObject = GameObject.Find(itemObjectName); 
        if (itemObject == null) return;
        ItemProperties item = itemObject.GetComponent<Item>().item;
        if (!InventoryManager.Instance.Items.Contains(item))
        {
            InventoryManager.Instance.Add(item);
            GameObject.Destroy(itemObject);
        }
    }

    private void SolvePuzzleObject(PuzzleTrigger trigger)
    {
        Puzzle puzzle = trigger.puzzle;
        trigger.DoInteraction();
        puzzle.Solve();
    }

    private void SolveStalkberryPuzzle()
    {
        HealingPlantController hpc = GameObject.Find("HealingPlant").GetComponent<HealingPlantController>();
        SolvePuzzleObject(GameObject.Find("DoorLock").GetComponent<PuzzleTrigger>());
        hpc.FixWateringMachine();
        OverrideTimePeriod(TimePeriod.SEVEN_YRS_AGO);
        hpc.PlantSeed();
    }

    private void EndOfIntro()
    {
        if (TimeTravelController.Instance.GetCurrentPeriod() != TimePeriod.INTRO) return;
        DialogManager.Instance.AddDialog(Dialog.LAB_NOTES);
        DialogManager.Instance.AddDialog(Dialog.FOUND_HAMMER);
        GiveItemToPlayer("Items/Hammer");
        PlayerMovement.Instance.Teleport(GameObject.Find("EndOfIntroTrigger").transform);
    }

    private void Farm()
    {
        DialogManager.Instance.AddDialog(Dialog.LAB_NOTES);
        DialogManager.Instance.AddDialog(Dialog.FOUND_HAMMER);
        GiveItemToPlayer("Items/Hammer");
        OverrideTimePeriod(TimePeriod.ONE_DAY_AGO);
        TimeTravelController.Instance.UnlockOneDayAgo();
        PlayerMovement.Instance.Teleport(GameObject.Find("NoStalkberryLeft").transform);
    }

    private void StalkberrySolve()
    {
        DialogManager.Instance.AddDialog(Dialog.LAB_NOTES);
        DialogManager.Instance.AddDialog(Dialog.FOUND_HAMMER);
        DialogManager.Instance.AddDialog(Dialog.PLANT1);
        DialogManager.Instance.AddDialog(Dialog.PLANT2);
        DialogManager.Instance.AddDialog(Dialog.PLANT5);
        DialogManager.Instance.AddDialog(Dialog.PLANT7);
        DialogManager.Instance.AddDialog(Dialog.TOUCHPLANT);
        DialogManager.Instance.AddDialog(Dialog.SMALLPLANT5);
        GiveItemToPlayer("Items/Hammer");
        TimeTravelController.Instance.UnlockAllPeriods();
        SolveStalkberryPuzzle();

        
        OverrideTimePeriod(TimePeriod.ONE_DAY_AGO);
        GiveItemToPlayer("Items/Stalkberry");

        PlayerMovement.Instance.Teleport(GameObject.Find("NoStalkberryLeft").transform);
    }
}