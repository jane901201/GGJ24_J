using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class AnimationEventsOnInspector : MonoBehaviour
{
    UniTaskCompletionSource animationTcs;

    private void Awake()
    {
        animationTcs = new UniTaskCompletionSource();
    }

    public async void TestAnination(int previousAnimationTime)
    {
        await WaitAnimationPlayOver(previousAnimationTime);
        Debug.Log("Do Animation");

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
