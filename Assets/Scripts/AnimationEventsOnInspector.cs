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
        await completeTcs.Task;
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