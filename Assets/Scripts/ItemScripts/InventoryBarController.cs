using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryBarController : MonoBehaviour
{
    public Button backpackButton;
    public GameObject inventoryBar;
    public GameObject itemContainer;
    public Transform slots;
    private bool isOpen = true, isUpdated = false;
    private static float TRANSITION_SPEED = 10f;
    public static InventoryBarController Instance;

    static float t = 1.0f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        backpackButton.onClick.AddListener(ToggleState);
    }

    void Update()
    {
        Transform transform = inventoryBar.transform;
        transform.localScale = new Vector3(Mathf.Lerp(0, 1, t), 1, 1);

        t += TRANSITION_SPEED * Time.deltaTime * (isOpen ? 1 : -1);
        t = Mathf.Min(t, 1);
        t = Mathf.Max(t, 0);

        if (t == 1f) UpdateOnce();
        if (t == 0f) isUpdated = false;
    }

    public void ToggleState()
    {
        isOpen = !isOpen;
    }

    public void UpdateItems()
    {
        List<ItemProperties> items = InventoryManager.Instance.Items;

        // Destroy items before refresh
        foreach (Transform slot in slots)
        {
            if (slot.GetComponent<ItemContainer>() != null) {
                Destroy(slot.GetComponent<ItemContainer>());
            }
        }

        int i = 0;        
        foreach (ItemProperties item in items)
        {
            GameObject slot = slots.GetChild(i++).gameObject;
            ItemContainer itemContainer = slot.AddComponent<ItemContainer>();
            itemContainer.SetItem(item);
        }
    }

    private void UpdateOnce()
    {
        if (!isUpdated) UpdateItems();
        isUpdated = true;
    }
}
