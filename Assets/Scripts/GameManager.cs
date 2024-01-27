using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 管整個遊戲流程
/// </summary>
public class GameManager : MonoBehaviour
{
    public UIManager uiManager;
    public GameData gameData;
    public List<PuzzleData> puzzleDatas;
    public int currentCorrectNum = 0;
    public MouseColor color;


    private void Awake()
    {
        
    }
}
