using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

/// <summary>
/// 管整個遊戲流程
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool IsPuzzling => _isPuzzling;

    public Action OnTimerEnd;
    public float OffSetPosition;

    public UIManager uiManager;
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
    public GameObject redMickey;
    public GameObject blueMickey;
    private Coroutine countdownCoroutine; // 保存 Coroutine 的引用


    bool _isPuzzling;
    private GameObject currentActiveMouse;
    private float currentHaveTime = 0f;

    public GameObject victoryAnimator;
    public float victoryWaitTime = 10f;
    public GameObject gameOverAnimator;
    public float gameOverWaitTime = 10f;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        _isPuzzling = false;
        roomMousecolor = MouseColor.Red;
        puzzleReturnColor = MouseColor.Blue;
        currentHaveTime = mouseMoveDuration;
    }

    private void Start()
    {
        PlayerForwordMove();
        //StartCoroutine(TestRoomCreate());
    }

    private void Update()
    {
        if (!_isPuzzling)
        {
            //VictoryCheck();
        }
    }

    private IEnumerator TestRoomCreate()
    {
        PlayerForwordMove();
        yield return new WaitForSeconds(10f);
        PlayerFleeMove();
    }

    public async void PlayerMove(bool isSucceed)
    {
        await WaitUI();
        RoomManager.isSolvingPuzzle = false;
        if (isSucceed)
            PlayerForwordMove();
        else
            PlayerFleeMove();
    }

    private async void CheckMouseAndCamDistance()
    {
        //Debug.Log("<color=red>CheckMouseAndCamDistance</color>");

        if (currentActiveMouse != null)
        {
            var sss = Mathf.Abs(currentActiveMouse.transform.position.magnitude - playerCamera.transform.position.magnitude);
            //Debug.Log($"CheckMouseAndCamDistance {sss}");
            if (sss <= 60)
            {
                currentActiveMouse.transform.position = playerCamera.transform.forward + new Vector3(0, 0, playerCamera.transform.position.z>=0? playerCamera.transform.position.z + OffSetPosition : playerCamera.transform.position.z - OffSetPosition);
                playerCamera.transform.DOKill();

                await WaitUI(true);

            }
        }
    }

    public async UniTask WaitUI(bool wait6s = false)
    {
        if (UIManager.Instance.hideTcs != null)
        {
            Debug.Log("<color=red>Wait</color>");

            await UIManager.Instance.hideTcs.Task;
            Debug.Log("<color=red>Wait done</color>");

        }
        if (wait6s)
        {
            var animator = currentActiveMouse.GetComponent<Animator>();
            animator.SetTrigger("Run");
            await UniTask.Delay(3000);
            Destroy(currentActiveMouse);
            Move();
        }
        Debug.Log("<color=red>Destory</color>");
        Debug.Log("<color=red>Wait done ?????</color>");

    }

    public void PlayerForwordMove()
    {
        Vector3 moveDir;
        //TODO:playerCamera.transform.rotation.w >= 1f?
        if (playerCamera.transform.rotation.y == 0)
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

        playerCamera.transform.DOMove(playerCamera.transform.position + moveDir * playerMoveDis * 2, fleeDuration)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() => Debug.Log("兩倍移動完成")); // 使用 Quad 曲線實現變速效果

        playerCamera.transform.DOMove(playerCamera.transform.position + moveDir * playerMoveDis * 3, normalDuration)
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

    private void Move()
    {
        if (roomMousecolor == puzzleReturnColor)
        {
            currentHaveTime = mouseMoveDuration;
            currentCorrectNum++;
            PlayerMove(true);
        }
        else if (roomMousecolor != puzzleReturnColor)
        {
            currentHaveTime = mouseMoveDuration;
            currentCorrectNum = 0;
            currentFalseNum++;
            PlayerMove(false);
        }
    }

    public void VictoryCheck()
    {
        var animator = currentActiveMouse.GetComponent<Animator>();
        animator.SetTrigger("Run");
        if (roomMousecolor == puzzleReturnColor)
        {
            currentHaveTime = mouseMoveDuration;
            currentCorrectNum++;
            PlayerMove(true);
        }
        else if (roomMousecolor != puzzleReturnColor)
        {
            currentHaveTime = mouseMoveDuration;
            currentCorrectNum = 0;
            currentFalseNum++;
            PlayerMove(false);
        }
        else if (currentHaveTime <= 0f)
        {
            currentHaveTime = mouseMoveDuration;
            currentCorrectNum = 0;
            currentFalseNum++;
            PlayerMove(false);
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

    public void StartCountdown()
    {
        countdownCoroutine = StartCoroutine(CountDown());
    }

    private IEnumerator CountDown()
    {
        while (currentHaveTime > 0)
        {
            Debug.Log("剩餘時間：" + currentHaveTime.ToString("F2")); // 以兩位小數顯示時間
            yield return new WaitForSeconds(1.0f); // 每一秒等待一次

            currentHaveTime -= 1.0f;
        }

        Debug.Log("倒數計時結束");
    }

    public void StopCountdown()
    {
        // 在特定條件下停止 Coroutine
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            Debug.Log("倒數計時被手動停止");
            OnTimerEnd?.Invoke();
        }
    }

    public void MouseCreate()
    {
        // 0 = red, 1 = blue
        int randomValue = Random.Range(0, 2);

        GameObject selectedColorMouse;

        if (randomValue == 0)
        {
            selectedColorMouse = redMickey;
            Debug.Log("生成紅色");
        }
        else
        {
            selectedColorMouse = blueMickey;
            Debug.Log("生成藍色");
        }

        var tempMouse = Instantiate(selectedColorMouse);

        if (currentActiveMouse != null)
        {
            Destroy(currentActiveMouse.gameObject);
        }
        currentActiveMouse = tempMouse;
        Debug.Log("Player Camera Rotation Y: " + playerCamera.transform.rotation);
        if (playerCamera.transform.rotation.w >= 1f)
        {
            currentActiveMouse.transform.position = playerCamera.transform.position + playerCamera.transform.forward * mouseDistance;
            currentActiveMouse.transform.rotation = new Quaternion(0, 1,0,0);
        }
        else if (playerCamera.transform.rotation.y >= 1f)
        {
            currentActiveMouse.transform.position = playerCamera.transform.position + playerCamera.transform.forward * mouseDistance;
            currentActiveMouse.transform.rotation = new Quaternion(0, 0,0,1);
        }
    }

    public void MouseMove()
    {
        Vector3 moveDir;
        if (playerCamera.transform.rotation.y == 0)
            moveDir = Vector3.back;
        else
            moveDir = Vector3.forward;

        currentActiveMouse.transform.DOMove(currentActiveMouse.transform.position + moveDir * mouseDistance, mouseMoveDuration)
            .SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                CheckMouseAndCamDistance();
                //if (PuzzleManager.Instance.IsSuccess)
                //{
                //    Debug.Log("<color=red> add event</color>");

                //    Debug.Log("<color=red>Stop mouse</color>");

                //    if (currentActiveMouse != null)
                //    {
                //        currentActiveMouse.transform.DOKill();
                //    }
                //    return;
                //}
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
        StartCoroutine(VictoryCoroutine());
    }

    public IEnumerator VictoryCoroutine()
    {
        victoryAnimator.SetActive(true);
        yield return new WaitForSeconds(victoryWaitTime);
        Debug.Log("VictoryAnimComplete");
        SceneManager.LoadScene("StartMenuScene");
    }

    public void GameOver()
    {
        StartCoroutine(GameOverCoroutine());
    }

    public IEnumerator GameOverCoroutine()
    {
        gameOverAnimator.SetActive(true);
        yield return new WaitForSeconds(gameOverWaitTime);
        Debug.Log("GameOver");
        SceneManager.LoadScene("StartMenuScene");
    }
}