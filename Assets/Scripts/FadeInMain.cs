using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FadeInMain : MonoBehaviour
{
    [SerializeField]CanvasGroup canvasGroup;
    private void OnEnable() {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn() {
        canvasGroup.alpha = 0;
        Tween tween = canvasGroup.DOFade(1f, 2f);
        yield return tween.WaitForCompletion();
    }
}
