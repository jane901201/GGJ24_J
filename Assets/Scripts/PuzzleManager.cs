using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PuzzleManager
{
    static PuzzleManager instance;

    public static PuzzleManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PuzzleManager();
            }
            return instance;
        }
    }

    public Action<bool> OnSuccessResult;
    public Action<bool> OnAnimationStart, OnAnimationEnd;
    public Action OnMouseStop;

    public bool IsSuccess => isSuccess;

    public bool IsPuzzleHide = false;

    bool isSuccess;

    public void Init()
    {
        UIManager.Instance.OnCurrentGoatItemClick += DoEvent;
    }

    private void DoEvent(Item item)
    {
        Debug.Log("<color=yellow>DoEvent trigger </color>");

        if (UIManager.Instance.CurrentSelectItem != null)
        {
            var currentItem = UIManager.Instance.CurrentSelectGoatItem;
            var success = currentItem.triggerItem.Any(item => UIManager.Instance.CurrentSelectItem.ItemData.triggerItem.Contains(item));

            int indexToCheck = currentItem.triggerItem.IndexOf(UIManager.Instance.CurrentSelectItem.ItemData.triggerItem.FirstOrDefault());
            if (success)
            {
                Debug.Log("<color=green>success</color>");

                if (indexToCheck >= 0 && indexToCheck < currentItem.triggerEvents.Count)
                {
                    currentItem.triggerEvents[indexToCheck]?.Invoke(item);
                    Debug.Log($"Do currentItem.triggerEvents[indexToCheck] {indexToCheck}");
                }
                UIManager.Instance.RemoveCurrentBagItem(UIManager.Instance.CurrentSelectItem.ItemData);
            }
            else
            {
                Debug.Log("<color=red>failed</color>");

            }

        }
    }

    public void SetPuzzleResult(bool isValid)
    {
        isSuccess = isValid;
        OnSuccessResult?.Invoke(isSuccess);
    }

    public void DoStopMouse()
    {
        Debug.Log("<color=yellow>DoStopMouse </color>");
        OnMouseStop?.Invoke();
    }
}
