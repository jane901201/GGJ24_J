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


    UniTaskCompletionSource animationTcs;

    private void Awake()
    {
        animationTcs = new UniTaskCompletionSource();
    }

    public async void PlayAnimation(int uid)
    {
        await WaitAnimationPlayOver(allAnimation.animationEventData[uid].previousAnimationTime);
        allAnimation.PlayAnimation(uid);
    }

    public void SetIsSccuess(bool isValid)
    {
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


    private AllAnimation()
    {

    }

    public void PlayAnimation(int uid)
    {
        //int index = animationEventData[uid].targetAnimatorName.IndexOf(animationEventData[uid].targetAnimatorName);
        animationEventData[uid].animator.SetTrigger($"{animationEventData[uid].animationName}");



        originalPosition = animationEventData[uid].button.transform.position;
        AnimationEventData eventData = animationEventData[uid];
        eventData.button.transform.DOMoveY(jumpHeight, jumpDuration)
            .SetLoops(jumpsRemaining * 2, LoopType.Yoyo);
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
    //public string targetAnimatorName;
    public string animationName;

    public ButtonController button;
}