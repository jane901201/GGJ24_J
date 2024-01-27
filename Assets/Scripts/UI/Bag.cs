using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class Bag : MonoBehaviour
{
    [SerializeField] ItemSlot itemSlotPrefab;
    [SerializeField] RectTransform contentRect;

    List<ItemSlot> allItems;


    private void Awake()
    {
        allItems = new List<ItemSlot>();
    }

    public void AddToBag(Item item)
    {
        var tempitem = Instantiate(itemSlotPrefab, contentRect);
        tempitem.Init(item);
        allItems.Add(tempitem);


        UIManager.Instance.OnCurrentItemClick = DoTriggerEvent;

    }

    public void RemoveToBag(Item item)
    {
        var tempItem = allItems.Where(data => data.ItemData.name == item.name).FirstOrDefault();

        Destroy(tempItem.gameObject);
    }



    private void DoTriggerEvent(ItemSlot slot)
    {
        Debug.Log($"current item click {slot.name}   {slot.ItemData.name}");

    }

    public void ResetBag()
    {
        for (int i = 0; i < allItems.Count; i++)
        {
            if (allItems[i] is null || allItems[i] == null)
            {
                continue;
            }
            Destroy(allItems[i].gameObject);
        }
        allItems.Clear();
    }
}
