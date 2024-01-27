using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class AnimationEventWithParameters : UnityEvent<int, string, string> { }

[System.Serializable]
public class AnimationEventsOnInspector : MonoBehaviour
{
    UniTaskCompletionSource animationTcs;

    public List<string> animatorName;
    public List<Animator> animators;

    private void Awake()
    {
        animationTcs = new UniTaskCompletionSource();
    }

    public async void PlayAnimation(int previousAnimationTime)
    {
        await WaitAnimationPlayOver(previousAnimationTime);
        Debug.Log("Do Animation");

        //int index = animatorName.IndexOf(targetAnimatorName);

        //if (index != -1 && index < animators.Count)
        //{
        //    Animator targetAnimator = animators[index];
        //    targetAnimator.Play($"{animationName}");
        //}
        //else
        //{
        //    Debug.LogWarning($"Animator with name {targetAnimatorName} not found.");
        //}
    }

    public async void TestAnination2(int previousAnimationTime)
    {
        await WaitAnimationPlayOver(previousAnimationTime);
        Debug.Log("Do Animation2");
    }

    public async void TestAnination3(int previousAnimationTime)
    {
        await WaitAnimationPlayOver(previousAnimationTime);
        Debug.Log("Do Animation3");
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
