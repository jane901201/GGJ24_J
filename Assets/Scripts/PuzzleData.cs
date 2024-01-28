using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleData : MonoBehaviour
{
    public MouseColor triggerColor;

    public List<Item> OtherNewItems => otherSecretItems;
    public List<ButtonController> OtherNewItemButtonController => scretButtons;

    [SerializeField] List<ButtonController> notScretButtons;
    [SerializeField] List<ButtonController> scretButtons;

    [SerializeField] public List<Item> items;
    [SerializeField] List<Item> otherSecretItems;


    public void Awake()
    {
    }

    public void ResetData()
    {
        for (int i = 0; i < notScretButtons.Count; i++)
        {
            notScretButtons[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < scretButtons.Count; i++)
        {
            scretButtons[i].gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        for (int i = 0; i < notScretButtons.Count; i++)
        {
            var j = i;

            notScretButtons[i].OnClick += (() =>
            {
                if (notScretButtons[j].Pickable)
                {
                    UIManager.Instance.AddToBag(items[j]);
                    notScretButtons[j].gameObject.SetActive(false);
                }
                else
                {
                    UIManager.Instance.SetCurrentGoatItem(items[j]);
                }
                notScretButtons[j].SetItemData(items[j]);
            });
        }

        for (int i = 0; i < scretButtons.Count; i++)
        {
            var j = i;

            scretButtons[i].OnClick += (() =>
            {
                if (scretButtons[j].Pickable)
                {
                    UIManager.Instance.AddToBag(otherSecretItems[j]);
                    scretButtons[j].gameObject.SetActive(false);
                }
                else
                {
                    UIManager.Instance.SetCurrentGoatItem(otherSecretItems[j]);
                }
                scretButtons[j].SetItemData(items[j]);

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
    public List<AnimationEventWithParameters> triggerEvents;
}
