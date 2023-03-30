using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemContainer : IHover
{
    public ItemProperties item;

    public void SetItem(ItemProperties item)
    {
        this.item = item;
        Image image = GetComponent<Image>();
        Color color = new Color(255f, 255f, 255f, 1f);
        image.color = color;
        image.sprite = item.icon;
    }

    public override void DoHoverAction()
    {
        Tooltip.Instance.DisplayTooltip(item.itemName, item.desc);
        Tooltip.Instance.Show();
    }

    public override void StopHoverAction()
    {
        Tooltip.Instance.Hide();
    }
}
