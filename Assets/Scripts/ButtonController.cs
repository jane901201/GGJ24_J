using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class ButtonController : MonoBehaviour, IButtonController
{
    public bool Pickable => pickable;

    public Action OnClick { get; set; }

    [SerializeField] bool pickable;

    Button button;
    Image image;
    CanvasGroup canvasGroup;

    private void Awake()
    {
        button = GetComponent<Button>();
        canvasGroup = GetComponent<CanvasGroup>();
        image = GetComponent<Image>();

        RegisterEvenets();

    }

    private void RegisterEvenets()
    {
        button.onClick.AddListener(OnClickBehavior);
        EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry deselectEntry = new EventTrigger.Entry { eventID = EventTriggerType.Deselect };
        deselectEntry.callback.AddListener((data) => { OnDeselectBehavior(); });

        eventTrigger.triggers.Add(deselectEntry);
    }

    private void OnDeselectBehavior()
    {
        image.color = Color.white;
    }

    private void OnClickBehavior()
    {
        OnClick?.Invoke();
    }

    public void SetVisible(bool isValid)
    {
        canvasGroup.alpha = isValid ? 1 : 0;
        canvasGroup.blocksRaycasts = isValid;
        canvasGroup.interactable = isValid;
    }

    public void SetSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }

}
