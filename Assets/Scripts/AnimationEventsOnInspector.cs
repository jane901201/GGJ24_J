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
        await allAnimation.PlayAnimation(uid, completeTcs);
        PuzzleManager.Instance.OnAnimationEnd?.Invoke(false);
    }

    public async void SetIsSccuess(bool isValid)
    {
        if (completeTcs != null)
        {
            await completeTcs.Task;
        }
        await UniTask.Delay(1000);

        PuzzleManager.Instance.SetPuzzleResult(isValid);
    }
    private async UniTask WaitAnimationPlayOver(int animationTime)
    {
        if (animationTcs != null)
        {
            animationTcs = new UniTaskCompletionSource();
            await UniTask.Delay(animationTime * 1000);
            animationTcs.TrySetResult();
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
        await UniTask.DelayFrame(ReturnColorWaitTime * 1000);
        if ((int)MouseColor.Red == colorNum)
        {
            GameManager.Instance.puzzleReturnColor = MouseColor.Red;
        }
        else
        {
            GameManager.Instance.puzzleReturnColor = MouseColor.Blue;
        }
        SetIsSccuess(true);
    }
}


[System.Serializable]
public class AllAnimation
{
    public List<AnimationEventData> animationEventData;

    public async UniTask PlayAnimation(int uid, UniTaskCompletionSource tcs)
    {
        animationEventData[uid].animator.SetTrigger($"{animationEventData[uid].animationName}");

        await UniTask.Yield(PlayerLoopTiming.PostLateUpdate);

        if (!animationEventData[uid].animator.GetCurrentAnimatorStateInfo(0).IsName(animationEventData[uid].animationName))
        {
            tcs.TrySetResult();
        }
        await tcs.Task;
    }
}

[System.Serializable]
public class AnimationEventData
{
    public Animator animator;
    public int previousAnimationTime;
    public string animationName;
}