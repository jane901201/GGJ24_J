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

    List<ButtonController> buttons;

    public void Awake()
    {
        buttons = new List<ButtonController>();
        buttons = GetComponentsInChildren<ButtonController>().ToList();
    }

    private void Start()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            var j = i;
            
            buttons[i].OnClick += (() =>
            {
                if (buttons[j].Pickable)
                {
                    UIManager.Instance.AddToBag(items[j]);
                    buttons[j].gameObject.SetActive(false);
                }
                else
                {
                    UIManager.Instance.SetCurrentGoatItem(items[j]);
                }
            });
        }
    }
}


[Serializable]
public struct Item
{
    public string name;
    public List<string> triggerItem;
    public Image image;
    public AnimationEventWithParameters triggerEvent;
}
