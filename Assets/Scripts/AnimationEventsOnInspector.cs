using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class AnimationEventWithParameters : UnityEvent<Item> { }

[System.Serializable]
public class AnimationEventsOnInspector : MonoBehaviour
{
    public AllAnimation allAnimation;
    public int ReturnColorWaitTime;

    UniTaskCompletionSource animationTcs, completeTcs;

    private void Awake()
    {
        animationTcs = new UniTaskCompletionSource();
    }

    public async void PlayAnimation(int uid)
    {
        PuzzleManager.Instance.OnAnimationStart?.Invoke(true);
        completeTcs = new UniTaskCompletionSource();
        await WaitAnimationPlayOver(allAnimation.animationEventData[uid].previousAnimationTime);
        Debug.Log("PlayAnimation await");
        await allAnimation.PlayAnimation(uid, completeTcs);
        Debug.Log("PlayAnimation done");

        PuzzleManager.Instance.OnAnimationEnd?.Invoke(false);
    }

    public async void SetIsSccuess(bool isValid)
    {
        //if (completeTcs != null)
        //{
        //    Debug.Log("SetIsSccuess await");
        //    await completeTcs.Task;
        //    Debug.Log("SetIsSccuess done");

        //}
        await UniTask.Delay(ReturnColorWaitTime * 1000);

        Debug.Log(" <color=green>SetPuzzleResult</color> ");
        PuzzleManager.Instance.SetPuzzleResult(isValid);
    }
    private async UniTask WaitAnimationPlayOver(int animationTime)
    {
        if (animationTcs != null)
        {
            Debug.Log("WaitAnimationPlayOver await");

            animationTcs = new UniTaskCompletionSource();
            await UniTask.Delay(animationTime * 1000);
            animationTcs.TrySetResult();
            Debug.Log("WaitAnimationPlayOver done");

        }
    }

    public void AddItem(string itemName)
    {
        UIManager.Instance.EnableNewItem(itemName);
    }

    /// <summary>
    /// 紅0藍1
    /// </summary>
    /// <param name="colorNum"></param>
    public async void ReturnColor(int colorNum)
    {

        Debug.Log("ReturnColor await");

        Debug.Log("ReturnColor done");

        if ((int)MouseColor.Red == colorNum)
        {
            GameManager.Instance.puzzleReturnColor = MouseColor.Red;
        }
        else
        {
            GameManager.Instance.puzzleReturnColor = MouseColor.Blue;
        }
        var checkResult = (int)GameManager.Instance.roomMousecolor == colorNum;
        await UniTask.DelayFrame(ReturnColorWaitTime * 1000);
        SetIsSccuess(checkResult);
        GameManager.Instance.VictoryCheck();
    }

    public void StopMouseMove()
    {
        PuzzleManager.Instance.DoStopMouse();
    }
}


[System.Serializable]
public class AllAnimation
{
    public List<AnimationEventData> animationEventData;

    public async UniTask PlayAnimation(int uid, UniTaskCompletionSource tcs)
    {
        animationEventData[uid].animator.SetTrigger($"{animationEventData[uid].animationName}");

        //await UniTask.Yield(PlayerLoopTiming.PostLateUpdate);

        //if (!animationEventData[uid].animator.GetCurrentAnimatorStateInfo(0).IsName(animationEventData[uid].animationName))
        //{
        //    Debug.Log("<color=red>OK</color>");

        //    tcs.TrySetResult();
        //}
        //else
        //{
        //    Debug.Log("<color=red>No ok</color>");
        //    tcs.TrySetResult();
        //}
        //tcs.TrySetResult();

        //await tcs.Task;
        Debug.Log("<color=red>PlayAnimation done</color>");

    }
}

[System.Serializable]
public class AnimationEventData
{
    public Animator animator;
    public int previousAnimationTime;
    public string animationName;
}