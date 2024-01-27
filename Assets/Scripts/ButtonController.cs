using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour, IButtonController
{
    public bool Pickable => pickable;

    public Action OnClick { get; set; }

    [SerializeField] bool pickable;
    Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickBehavior);
    }

    private void OnClickBehavior()
    {
        OnClick?.Invoke();
    }
}
