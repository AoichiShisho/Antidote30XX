using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum TimePeriod {
	SEVEN_YRS_AGO,
	FIVE_YRS_AGO,
	TWO_YRS_AGO,
	ONE_DAY_AGO,
	INTRO
}

public class TimeTravelController : MonoBehaviour
{
	public static TimeTravelController Instance;
	private TimePeriod currentTime { get; set; }
	private bool isCoolingDown = false;
	private List<TimePeriod> allowedPeriods = new List<TimePeriod>(){ TimePeriod.ONE_DAY_AGO };

	public TimePeriod GetCurrentPeriod()
	{
		return currentTime;
	}

	public void Awake()
	{
		Instance = this;
		currentTime = TimePeriod.INTRO;
	}

	void Start()
	{
		broadcastTime();
	}

	private void broadcastTime()
	{
		GameObject[] travellers;
		travellers = GameObject.FindGameObjectsWithTag("TimeTravels");
		Debug.Log("Changing time to " + currentTime);
		foreach (GameObject traveler in travellers)
		{
			traveler.SendMessage("SetTimePeriod", currentTime, SendMessageOptions.DontRequireReceiver);
		}
	}

	public void goForward()
	{
		if (isCoolingDown) return;
		currentTime = (TimePeriod) Mathf.Min((int) currentTime + 1, (int) allowedPeriods.Max());
		broadcastTime();
	}

	public void goBackwards()
	{
		if (isCoolingDown) return;
		currentTime = (TimePeriod) Mathf.Max((int) currentTime - 1, (int) allowedPeriods.Min());
		broadcastTime();
	}

	public bool hasNext()
	{
		return currentTime != TimePeriod.ONE_DAY_AGO || currentTime != TimePeriod.INTRO;
	}

	public bool hasPrev()
	{
		return currentTime != TimePeriod.SEVEN_YRS_AGO;
	}

	public void SetCooldownStatus(bool cooldown)
	{
		this.isCoolingDown = cooldown;
	}

	public static int GetNumberOfPeriods()
	{
		return System.Enum.GetValues(typeof(TimePeriod)).Length;
	}

	public void SetTimePeriod(TimePeriod period)
	{
		currentTime = period;
		broadcastTime();
	}

	private void UnlockPeriod(TimePeriod period)
	{
		if (!allowedPeriods.Contains(period))
		{
			allowedPeriods.Add(period);
		}
	}

	public void UnlockOneDayAgo()
	{
		UnlockPeriod(TimePeriod.ONE_DAY_AGO);
	}

	public void UnlockTwoYrsAgo()
	{
		UnlockPeriod(TimePeriod.TWO_YRS_AGO);
	}

	public void UnlockFiveYrsAgo()
	{
		UnlockPeriod(TimePeriod.FIVE_YRS_AGO);
	}

	public void UnlockSevenYearsAgo()
	{
		UnlockPeriod(TimePeriod.SEVEN_YRS_AGO);
	}

	public void UnlockAllPeriods()
	{
		allowedPeriods = new List<TimePeriod> { TimePeriod.ONE_DAY_AGO, TimePeriod.TWO_YRS_AGO, TimePeriod.FIVE_YRS_AGO, TimePeriod.SEVEN_YRS_AGO };
	}

	public bool GetCooldownStatus() { return isCoolingDown; }
}