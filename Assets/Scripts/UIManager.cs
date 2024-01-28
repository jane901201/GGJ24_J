using System;
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

        puzzleDatas = GetComponentsInChildren<PuzzleData>(true).ToList();
        Assert.IsNotNull(puzzleDatas);
        RoomManager.OnPuzzleGame += DoOnPuzzple;
    }

    private void Start()
    {
        PuzzleManager.Instance.Init();
        showPanelButton.OnClick += DoShowUIPannel;
        showPanelButton.SetVisible(false);

        hidePanelButton.OnClick += DoHideUIPanel;

        PuzzleManager.Instance.OnSuccessResult += DoEndPuzzle;
        PuzzleManager.Instance.OnAnimationStart += SetInteracbleClickOnShowButton;
        PuzzleManager.Instance.OnAnimationEnd += SetInteracbleClickOnShowButton;
    }

    private void SetInteracbleClickOnShowButton(bool isValid)
    {
        Debug.Log("SetInteracbleClickOnShowButton");
        hidePanelButton.SetInteracble(!isValid);
    }

    private void DoEndPuzzle(bool isValid)
    {
        MoveUIPanel(false);
        hidePanelButton.SetVisible(false);
        GameManager.Instance.StopCountdown();
    }
    bool isFirst = true;
    private void DoOnPuzzple()
    {
        if (isFirst)
        {
            bag.ResetBag();
            puzzleDatas.First().gameObject.SetActive(true);
            isFirst = false;
        }
        else
        {
            var random = UnityEngine.Random.Range(1, puzzleDatas.Count);
            for (int i = 0; i < puzzleDatas.Count; i++)
            {
                puzzleDatas[i].gameObject.SetActive(false);
            }
            bag.ResetBag();
            puzzleDatas[random].ResetData();
            puzzleDatas[random].gameObject.SetActive(true);

        }
        MoveUIPanel(true);
        PuzzleManager.Instance.IsPuzzleHide = false;
    }

    private void DoHideUIPanel()
    {
        MoveUIPanel(false);
        showPanelButton.SetVisible(true);
        OnHideUiPanel?.Invoke(false);
        GameManager.Instance.StopCountdown();
        PuzzleManager.Instance.IsPuzzleHide = true;
    }

    private void DoShowUIPannel()
    {
        showPanelButton.SetVisible(false);
        hidePanelButton.SetVisible(true);
        MoveUIPanel(true);
        OnHideUiPanel?.Invoke(true);
        PuzzleManager.Instance.IsPuzzleHide = false;
        GameManager.Instance.StartCountdown();
        if (GameManager.Instance.mouse != null)
        {
            GameManager.Instance.MouseMove();
        }

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
        currentSelectGoatItem = item;
        OnCurrentGoatItemClick?.Invoke(item);
    }

    public void RemoveCurrentBagItem(Item slot)
    {
        currentSelectOnBagItem = null;
        bag.RemoveToBag(slot);
    }

    private void MoveUIPanel(bool isValid)
    {
        uiPanel.DOMoveY(isValid ? +540 : -1200 + 540, 0.5f).SetEase(Ease.OutQuad);
    }

    public void EnableNewItem(string itemName)
    {
        for (int i = 0; i < puzzleDatas.Count; i++)
        {
            Debug.Log($"Check {puzzleDatas[i].name}  // {itemName}");

            var newItemButton = puzzleDatas[i].OtherNewItemButtonController.FirstOrDefault(item => item.name == itemName);
            newItemButton.SetVisible(true);
            break;
        }
    }

}
