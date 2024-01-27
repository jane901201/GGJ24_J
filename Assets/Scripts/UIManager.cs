using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 管小遊戲謎題
/// </summary>
public class UIManager : MonoBehaviour
{

    public List<GameObject> puzzles; 
    public List<Button> itemButtons;

    private void Start()
    {
        
        
    }

    public void PuzzleUpdate()
    {
        for (int i = 0; i < itemButtons.Count; i++)
        {
            itemButtons[i].onClick.AddListener(() => TriggerEvent());   
            
        }
    }

    private void Update()
    {
    }

    public void TriggerEvent()
    {
        
    }
}
