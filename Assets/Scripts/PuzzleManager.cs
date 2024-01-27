using System;
using System.Collections;
using System.Collections.Generic;
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
    }
}
