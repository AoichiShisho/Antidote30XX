using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimePickerInterface : MonoBehaviour
{
    public Button prev, next;
    public List<Sprite> alternateSprites;
    public Image indicator;
    private TimePeriod currentPeriod;
    private Color originalColor;
    static float t = 1.0f;
    public float transitionSpeed = .1f;

    public void Awake()
    {
        next.onClick.AddListener(UpdateInstance);
        prev.onClick.AddListener(UpdateInstance);
        originalColor = indicator.color;
        gameObject.SetActive(false);
    }

    void Start()
    {
        currentPeriod = TimeTravelController.Instance.GetCurrentPeriod();
    }

    void Update()
    {
        indicator.color = Color.Lerp(Color.black, originalColor, t);
        t += transitionSpeed * Time.deltaTime;
        t = Mathf.Min(t, 1);

        if (TimeTravelController.Instance.GetCooldownStatus() && t == 1f)
            TimeTravelController.Instance.SetCooldownStatus(false);
    }

    private IEnumerator SwitchTime(int period)
    {
            currentPeriod = (TimePeriod) period;
            t = 0f;
            GetComponent<WhiteoutEffect>().Trigger();
            GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(.3f);

            Image displayedSprite = GameObject.Find("TimePicker").GetComponent<Image>();
            displayedSprite.sprite = alternateSprites[period]; // offset by one because there is no sprite for TimePeriod.INTRO
    }

    public void UpdateInstance()
    {
        int period = (int) TimeTravelController.Instance.GetCurrentPeriod();

        if (period != (int) currentPeriod) {
            TimeTravelController.Instance.SetCooldownStatus(true);
            StartCoroutine(SwitchTime(period));
        }
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }
}