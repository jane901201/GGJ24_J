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
    public float playerMoveDis = 5f;
    
    public GameObject playerCamera;

    bool isPuzzing;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        PuzzleManager.Instance.OnSuccessResult += PlayerMove;
    }

    private void Start()
    {
        PlayerForwordMove();
    }

    public void PlayerMove(bool isSucceed)
    {
        if (isSucceed)
            PlayerForwordMove();
        else
            PlayerFleeMove();
    }

    public void PlayerForwordMove()
    {
        playerCamera.transform.DOMove(playerCamera.transform.position + Vector3.forward * playerMoveDis, 10.0f)
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

    public void PlayerFleeMove()
    {
        playerCamera.transform.DORotate(new Vector3(0.0f, 180.0f, 0.0f), 1.5f)
            .SetEase(Ease.Linear) // 設定旋轉的緩動曲線
            .OnComplete(() => Debug.Log("旋轉完成")); // 在旋轉完成時執行的回調

        playerCamera.transform.DOMove(playerCamera.transform.position + Vector3.back * playerMoveDis/2, 5.0f)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() => Debug.Log("兩倍移動完成")); // 使用 Quad 曲線實現變速效果
        
        playerCamera.transform.DOMove(playerCamera.transform.position + Vector3.back * playerMoveDis, 10.0f)
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
