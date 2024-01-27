using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ButtonController))]
public class ItemSlot : MonoBehaviour
{
    public Item ItemData => itemData;
    public Action<ItemSlot> OnClick;

    Item itemData;
    ButtonController buttonController;


    private void Awake()
    {
        buttonController = GetComponent<ButtonController>();
        UIManager.Instance.OnCurrentItemClick += ResetButton;
    }

    private void ResetButton(ItemSlot slot)
    {
        if (slot != this)
        {
            
        }
    }

    public void Init(Item data)
    {
        this.itemData = data;
        buttonController.OnClick += OnClickSlot;
    }

    private void OnClickSlot()
    {
        UIManager.Instance.SetCurrentItem(this);
        OnClick?.Invoke(this);
    }

    internal object Any()
    {
        throw new NotImplementedException();
    }
}
