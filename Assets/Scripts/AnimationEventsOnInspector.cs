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
}


[System.Serializable]
public class AllAnimation
{
    public List<AnimationEventData> animationEventData;


    public async UniTask PlayAnimation(int uid, UniTaskCompletionSource tcs)
    {
        //int index = animationEventData[uid].targetAnimatorName.IndexOf(animationEventData[uid].targetAnimatorName);
        animationEventData[uid].animator.SetTrigger($"{animationEventData[uid].animationName}");



        originalPosition = animationEventData[uid].button.transform.position;
        AnimationEventData eventData = animationEventData[uid];
        eventData.button.transform.DOMoveY(jumpHeight, jumpDuration)
            .SetLoops(jumpsRemaining * 2, LoopType.Yoyo).OnComplete(() => { tcs.TrySetResult(); });

        Debug.Log("Animating");
        await tcs.Task;
        Debug.Log("Animate Done");

        //Debug.Log($"<color=green>PlayAnimatio PlayAnimation {animationEventData[uid].targetAnimatorName} {animationEventData[uid].animationName} </color>");
    }


    #region Test
    Vector3 originalPosition;
    int jumpsRemaining = 3;
    float jumpHeight = 15f;
    float jumpDuration = 0.3f;

    private void PingPongJump(AnimationEventData eventData)
    {
        eventData.button.transform.DOJump(Vector3.up * jumpHeight, jumpDuration, 1, 1f);
    }

    //private void OnJumpComplete(AnimationEventData eventData)
    //{
    //    jumpsRemaining--;

    //    if (jumpsRemaining > 0)
    //    {
    //        PingPongJump(eventData);
    //    }
    //    else
    //    {
    //        eventData.button.transform.DOMove(originalPosition, jumpDuration)
    //           .OnComplete(() => Debug.Log("Done"));
    //    }
    //}
    #endregion
}

[System.Serializable]
public class AnimationEventData
{
    public Animator animator;
    public int previousAnimationTime;
    public string animationName;

    public ButtonController button;
}