using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

/// <summary>
/// 管小遊戲謎題
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public List<GameObject> puzzles;
    public List<Button> itemButtons;

    public Action<ItemSlot> OnCurrentItemClick;
    public Action<Item> OnCurrentGoatItemClick;

    public ItemSlot CurrentSelectItem => currentSelectOnBagItem;
    public Item CurrentSelectGoatItem => currentSelectGoatItem;

    [SerializeField] Bag bag;

    List<PuzzleData> puzzleDatas;
    ItemSlot currentSelectOnBagItem;
    Item currentSelectGoatItem;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        puzzleDatas = GetComponentsInChildren<PuzzleData>().ToList();
        Assert.IsNotNull(puzzleDatas);
        Debug.Log("UiManager awake");
    }

    public void AddToBag(Item item)
    {
        bag.AddToBag(item);
    }


    public void SetCurrentItem(ItemSlot itemSlot)
    {
        currentSelectOnBagItem = itemSlot;
        OnCurrentItemClick?.Invoke(itemSlot);
    }

    public void SetCurrentGoatItem(Item item)
    {
        Debug.Log($"SetCurrentGoatItem {item.name}");
        currentSelectGoatItem = item;
        OnCurrentGoatItemClick?.Invoke(item);
    }
}
