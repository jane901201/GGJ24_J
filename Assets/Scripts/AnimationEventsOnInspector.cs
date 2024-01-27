using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

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
        Debug.Log("Wait for tcs");
        await completeTcs.Task;
        await UniTask.Delay(1000);
        Debug.Log("OK");

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

        // Check if the animation has completed
        if (!animationEventData[uid].animator.GetCurrentAnimatorStateInfo(0).IsName(animationEventData[uid].animationName))
        {
            // Animation has completed, do something here
            Debug.Log("Animation complete!");
            tcs.TrySetResult();
        }

        Debug.Log("Animating");
        await tcs.Task;
        Debug.Log("Animate Done");

    }
}

[System.Serializable]
public class AnimationEventData
{
    public Animator animator;
    public int previousAnimationTime;
    public string animationName;

    public ButtonController button;
}