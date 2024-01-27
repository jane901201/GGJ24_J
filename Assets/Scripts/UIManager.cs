using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using static UnityEngine.UI.CanvasScaler;

/// <summary>
/// 管小遊戲謎題
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public List<GameObject> puzzles;

    public Action<ItemSlot> OnCurrentItemClick;
    public Action<Item> OnCurrentGoatItemClick;

    public ItemSlot CurrentSelectItem => currentSelectOnBagItem;
    public Item CurrentSelectGoatItem => currentSelectGoatItem;

    [SerializeField] Bag bag;
    [SerializeField] ButtonController showPanelButton, hidePanelButton;
    [SerializeField] RectTransform uiPanel;

    List<PuzzleData> puzzleDatas;
    ItemSlot currentSelectOnBagItem;
    Item currentSelectGoatItem;

    PuzzleManager puzzleManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        puzzleDatas = GetComponentsInChildren<PuzzleData>().ToList();
        Assert.IsNotNull(puzzleDatas);
        puzzleManager = new PuzzleManager();
        Debug.Log("UiManager awake");
    }

    private void Start()
    {
        puzzleManager.Init();
        showPanelButton.OnClick += DoShowUIPannel;
        showPanelButton.SetVisible(true);

        hidePanelButton.OnClick += DoHideUIPanel;

    }

    private void DoHideUIPanel()
    {
        MoveUIPanel(false);
        showPanelButton.SetVisible(true);

    }

    private void DoShowUIPannel()
    {
        // check GameManager is puzzle type

        showPanelButton.SetVisible(false);
        hidePanelButton.SetVisible(true);
        MoveUIPanel(true);
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

    private void MoveUIPanel(bool isValid)
    {
        uiPanel.DOMoveY(isValid ? +540 : -1200 + 540, 0.5f).SetEase(Ease.OutQuad);
    }


}
