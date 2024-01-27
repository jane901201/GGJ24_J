using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PuzzleManager
{


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
                UIManager.Instance.CurrentSelectGoatItem.triggerEvent?.Invoke();
            }
            else
            {
                Debug.Log("<color=red>failed</color>");

            }

        }
    }
}
