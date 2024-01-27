using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// 管小遊戲謎題
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public List<GameObject> puzzles;

    public Action<ItemSlot> OnCurrentItemClick;
    public Action<Item> OnCurrentGoatItemClick;
    public Action<bool> OnHideUiPanel;

    public ItemSlot CurrentSelectItem => currentSelectOnBagItem;
    public Item CurrentSelectGoatItem => currentSelectGoatItem;

    [SerializeField] Bag bag;
    [SerializeField] ButtonController showPanelButton, hidePanelButton;
    [SerializeField] RectTransform uiPanel;

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
        RoomManager.OnPuzzleGame += DoOnPuzzple;
    }

    private void Start()
    {
        PuzzleManager.Instance.Init();
        showPanelButton.OnClick += DoShowUIPannel;
        showPanelButton.SetVisible(false);

        hidePanelButton.OnClick += DoHideUIPanel;
    }

    bool isPuzzing;
    private void DoOnPuzzple()
    {
        isPuzzing = true;
        Debug.Log("DoOnPuzzple");
        MoveUIPanel(true);
    }

    private void DoHideUIPanel()
    {
        MoveUIPanel(false);
        showPanelButton.SetVisible(true);
        OnHideUiPanel?.Invoke(false);

    }

    private void DoShowUIPannel()
    {
        // check GameManager is puzzle type

        showPanelButton.SetVisible(false);
        hidePanelButton.SetVisible(true);
        MoveUIPanel(true);
        OnHideUiPanel?.Invoke(true);
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

    public void RemoveCurrentBagItem(Item slot)
    {
        Debug.Log($"RemoveCurrentBagItem");
        currentSelectOnBagItem = null;
        bag.RemoveToBag(slot);
    }

    private void MoveUIPanel(bool isValid)
    {
        uiPanel.DOMoveY(isValid ? +540 : -1200 + 540, 0.5f).SetEase(Ease.OutQuad);
    }


}
