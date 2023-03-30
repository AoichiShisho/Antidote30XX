using UnityEngine;
using UnityEngine.UI;

public class WhiteoutEffect : MonoBehaviour
{
    [SerializeField] private Image whiteoutImage;
    [SerializeField] private Animator whiteoutAnimator;
    [SerializeField] private string triggerName = "StartWhiteout";

    public void Trigger()
    {
        Vector2 anchoredPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(whiteoutImage.rectTransform.parent.GetComponent<RectTransform>(), Input.mousePosition, Camera.main, out anchoredPosition);

        whiteoutImage.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        whiteoutImage.rectTransform.anchorMin = whiteoutImage.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        whiteoutImage.rectTransform.anchoredPosition = anchoredPosition;

        whiteoutAnimator.SetTrigger(triggerName);
    }

    public void StartAnimation()
    {
        whiteoutAnimator.SetTrigger(triggerName);
    }
}