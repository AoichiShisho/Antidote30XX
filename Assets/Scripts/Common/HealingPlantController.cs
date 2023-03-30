using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HealingPlantController : MonoBehaviour, ITimeTravels
{
    private bool wateringMachineFixed = false;
    private TimePeriod plantedPeriod = TimePeriod.INTRO;
    public GameObject smallPlant, mediumPlant, largePlant;
    public IInteractable farmBox;

    void Start()
    {
        wateringMachineFixed = false;
        plantedPeriod = TimePeriod.INTRO;
    }

    public void SetTimePeriod(TimePeriod period)
    {
        bool wasPlayerInContact = smallPlant.GetComponent<NPC>().isPlayerInContact ||
                                mediumPlant.GetComponent<NPC>().isPlayerInContact ||
                                largePlant.GetComponent<NPC>().isPlayerInContact ||
                                farmBox.isPlayerInContact;

        smallPlant.SetActive(false);
        mediumPlant.SetActive(false);
        largePlant.SetActive(false);

        GameObject activePlant = null;

        switch(period) {
            case TimePeriod.INTRO:
            case TimePeriod.SEVEN_YRS_AGO:
                break;
            case TimePeriod.FIVE_YRS_AGO:
                if (plantedPeriod == TimePeriod.SEVEN_YRS_AGO) activePlant = smallPlant;
                break;
            case TimePeriod.TWO_YRS_AGO:
                if (plantedPeriod == TimePeriod.SEVEN_YRS_AGO) {
                    if (wateringMachineFixed) {
                        activePlant = mediumPlant;
                    } else {
                        activePlant = smallPlant;
                    }
                }
                else if (plantedPeriod == TimePeriod.FIVE_YRS_AGO && wateringMachineFixed) activePlant = smallPlant;
                break;
            case TimePeriod.ONE_DAY_AGO:
                if (plantedPeriod == TimePeriod.SEVEN_YRS_AGO) {
                    if (wateringMachineFixed) {
                        activePlant = largePlant;
                    } else {
                        activePlant = mediumPlant;
                    }
                } else if (plantedPeriod == TimePeriod.FIVE_YRS_AGO) {
                    if (wateringMachineFixed) {
                        activePlant = mediumPlant;
                    } else {
                        activePlant = smallPlant;
                    }
                } else if (plantedPeriod == TimePeriod.TWO_YRS_AGO) {
                    activePlant = smallPlant;
                }
                break;
            default:
                break;
        }

        if (activePlant != null)
        {
            activePlant.SetActive(true);
            activePlant.GetComponent<NPC>().SetTimePeriod(period);
            activePlant.GetComponent<NPC>().isPlayerInContact = wasPlayerInContact;
        }
    }

    public void FixWateringMachine()
    {
        wateringMachineFixed = true;
    }

    public void PlantSeed()
    {
        if (TimeTravelController.Instance.GetCurrentPeriod() < plantedPeriod) {
            plantedPeriod = TimeTravelController.Instance.GetCurrentPeriod();
        }
    }
}