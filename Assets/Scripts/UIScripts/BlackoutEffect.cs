using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackoutEffect : MonoBehaviour
{
    public static BlackoutEffect Instance;
    public float fadeDuration = 1f;

    void Awake()
    {
        Instance = this;
    }

    public void DoBlackout()
    {
        Image image = GetComponent<Image>();
        Color color = image.color;
        color.a = 1f;
        image.color = color;
    }

    public void EndBlackout()
    {
        StartCoroutine(FadeOut());
    }

    public void TimedBlackout(int seconds)
    {
        StartCoroutine(WaitAndEndBlackout(seconds));
    }

    private IEnumerator WaitAndEndBlackout(int seconds)
    {
        DoBlackout();
        yield return new WaitForSeconds(seconds);

        EndBlackout();
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Image targetImage = GetComponent<Image>();
        Color initialColor = targetImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(initialColor.a, 0f, elapsedTime / fadeDuration);
            targetImage.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            yield return null;
        }

        // Explicitly set the alpha value to 0 after the fadeout is complete
        targetImage.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);
    }
}
