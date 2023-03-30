using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsScreen : MonoBehaviour
{
    public Transform scrollEndMarker;
    public BlackoutEffect blackoutEffect;
    public float rollTime = 10f;

    private float rollingTime = 0f;

    public void RollCredits()
    {
        StartCoroutine(RollToMarker(scrollEndMarker));
    }

    private IEnumerator RollToMarker(Transform marker)
    {
        blackoutEffect.DoBlackout();

        Vector3 endPos = marker.position;

        while (Vector3.Distance(gameObject.transform.position, endPos) > 0)
        {
            rollingTime += Time.deltaTime;
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, endPos, rollingTime / rollTime);
            yield return null;
        }

        yield return new WaitForSeconds(2);
        Application.Quit();
    }
}