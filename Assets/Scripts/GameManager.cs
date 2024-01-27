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
    public bool IsPuzzling => _isPuzzling;

    public UIManager uiManager;
    public GameData gameData;
    public List<PuzzleData> puzzleDatas;
    public int currentCorrectNum = 0;
    public int currentFalseNum = 0;
    public MouseColor roomMousecolor;
    public MouseColor puzzleReturnColor;
    public float playerMoveDis = 5f;
    public float normalDuration = 10f;
    public float fleeDuration = 3f;
    [FormerlySerializedAs("seconds")] public float mouseMoveDuration = 60f;
    public float mouseDistance = 900f;
    
    public GameObject playerCamera;
    public GameObject mouse;

    
    bool _isPuzzling;
    private GameObject currentActiveMouse;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        //PuzzleManager.Instance.OnSuccessResult += PlayerMove;
        _isPuzzling = false;
        roomMousecolor = MouseColor.Red;
        puzzleReturnColor = MouseColor.Blue;
    }

    private void Start()
    {
        PlayerForwordMove();
        //MouseCreate();
        //MouseMove();
    }
    
    private void Update()
    {
        if (!_isPuzzling)
        {
            //VictoryCheck();
        }
    }

    public void PlayerMove(bool isSucceed)
    {
        RoomManager.isSolvingPuzzle = false;
        if (isSucceed)
            PlayerForwordMove();
        else
            PlayerFleeMove();
    }

    public void PlayerForwordMove()
    {
        Vector3 moveDir;
        if(playerCamera.transform.rotation.y == 0)
            moveDir = Vector3.forward;
        else
            moveDir = Vector3.back;
        playerCamera.transform.DOMove(playerCamera.transform.position + moveDir * playerMoveDis, normalDuration)
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
        Vector3 moveDir;
        float rotationY = 0f;
        if (playerCamera.transform.rotation.y == 0)
        {
            moveDir = Vector3.back;
            rotationY = 180f;
        }
        else
        {
            moveDir = Vector3.forward;
            rotationY = 0f;
        }
        Debug.Log("<color=green>GO</color>");
        playerCamera.transform.DORotate(new Vector3(0.0f, rotationY, 0.0f), 1.5f)
            .SetEase(Ease.Linear) // 設定旋轉的緩動曲線
            .OnComplete(() => Debug.Log("旋轉完成")); // 在旋轉完成時執行的回調

        playerCamera.transform.DOMove(playerCamera.transform.position + moveDir * playerMoveDis, fleeDuration)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() => Debug.Log("兩倍移動完成")); // 使用 Quad 曲線實現變速效果
        
        playerCamera.transform.DOMove(playerCamera.transform.position + moveDir * playerMoveDis*2, normalDuration)
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

    public void VictoryCheck()
    {
        if (roomMousecolor == puzzleReturnColor)
        {
            currentCorrectNum++;
            PlayerMove(true);
            MouseColorChange();
        }
        else if(roomMousecolor != puzzleReturnColor)
        {
            currentCorrectNum = 0;
            currentFalseNum++;
            PlayerMove(false);
            MouseColorChange();
        }

        if (currentCorrectNum >= 3)
        {
            Victory();
        }
        else if (currentFalseNum >= 3)
        {
            GameOver();            
        }
    }
    
    //TODO:選題的機制
    public void MouseColorChange()
    {
        //TODO:要問一下美術怎麼換色(直接生不同色的 Prefab?)
    }

    public void MouseCreate()
    {
        currentActiveMouse = Instantiate(mouse);
        Debug.Log("Player Camera Rotation Y: " + playerCamera.transform.rotation);
        if (playerCamera.transform.rotation.w >= 1f)
        {
            currentActiveMouse.transform.position = playerCamera.transform.position + playerCamera.transform.forward * mouseDistance;
        }
        else if(playerCamera.transform.rotation.y >= 1f)
        {
            currentActiveMouse.transform.position = playerCamera.transform.position + playerCamera.transform.forward * mouseDistance;
        }
    }

    public void MouseMove()
    {
        Vector3 moveDir;
        if(playerCamera.transform.rotation.y == 0)
            moveDir = Vector3.back;
        else
            moveDir = Vector3.forward;
        
        currentActiveMouse.transform.DOMove(currentActiveMouse.transform.position + moveDir * mouseDistance, mouseMoveDuration)
            .SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                // 在 Tween 更新時檢查條件
                if (PuzzleManager.Instance.IsPuzzleHide)
                {
                    // 停止 Tween
                    currentActiveMouse.transform.DOKill();
                    Debug.Log("Tween 已停止");
                }
            })
            .OnComplete(() => Debug.Log("移動完成"));
    }
    
    public void Victory()
    {
        //勝利動畫
        Debug.Log("Victory");
    }

    public void GameOver()
    {
        //失敗動畫
        Debug.Log("GameOver");
    }
}
