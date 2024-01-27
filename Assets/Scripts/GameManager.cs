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
    public static GameManager Instance;
    public bool IsPuzzong => isPuzzing;

    public UIManager uiManager;
    public GameData gameData;
    public List<PuzzleData> puzzleDatas;
    public int currentCorrectNum = 0;
    public MouseColor color;


    bool isPuzzing;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
