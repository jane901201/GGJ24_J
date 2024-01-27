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
    public bool IsSuccess => isSuccess;

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
            var success = UIManager.Instance.CurrentSelectGoatItem.triggerItem.Any(item => UIManager.Instance.CurrentSelectItem.ItemData.triggerItem.Contains(item));

            if (success)
            {
                Debug.Log("<color=green>success</color>");
                UIManager.Instance.CurrentSelectGoatItem.triggerEvent?.Invoke(item);
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
}
