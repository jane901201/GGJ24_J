using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleData : MonoBehaviour
{
    public MouseColor triggerColor;
    [SerializeField]
    public List<Item> items;

    public List<Button> buttons;

    public void Awake()
    {
        buttons = GetComponentsInChildren<Button>().ToList();
    }
}

[System.Serializable]
public struct Item
{
    public string name;
    public string triggerItem;
    public Sprite sprite;
}