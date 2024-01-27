using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
    public float playerSpeed = 5f;
    public GameObject playerCamera;

    bool isPuzzing;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    private void Start()
    {
        PlayerMove();
    }

    public void PlayerMove()
    {
        playerCamera.transform.DOMove(playerCamera.transform.position + Vector3.forward * playerSpeed, 100.0f)
            .SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                // 在 Tween 更新時檢查條件
                if (RoomManager.isSolvingPuzzle)
                {
                    // 停止 Tween
                    playerCamera.transform.DOKill();
                    Debug.Log("Tween 已停止");
                }
            })
            .OnComplete(() => Debug.Log("移動完成"));
    }
}
