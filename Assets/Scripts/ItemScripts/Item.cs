using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : IInteractable {
    public ItemProperties item;

    public override void DoInteraction()
    {
        if (gameObject.transform.parent && gameObject.transform.parent.TryGetComponent(out TravelItem parent)) {
            // if the item time travels then we should interact
            // with the parent instead
            parent.DoInteraction();
        } else {
            AddItemToInventory();
        }
        StopInteraction();
    }

    public override void StopInteraction()
    {
        InteractSign.Instance.Hide();
    }

    public void AddItemToInventory()
    {
        InventoryManager.Instance.Add(item);
        Destroy(gameObject);
    }
}