using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class FadeOutTitle : MonoBehaviour
{
    [SerializeField]CanvasGroup canvasGroup;
    [SerializeField]GameObject MainPanel;
    [SerializeField]TextMeshProUGUI title_d;
    [SerializeField]RectTransform title_ice;
    [SerializeField]TextMeshProUGUI title_l;
    
    bool doTitleAnim;

    private void Start() {
        StartCoroutine(FadeOut());
        doTitleAnim = true;
    }

    private IEnumerator FadeOut() {
        yield return new WaitForSeconds(2f);
        canvasGroup.alpha = 1;
        Tween tween = canvasGroup.DOFade(0f, 2f);
        yield return tween.WaitForCompletion();
        canvasGroup.gameObject.SetActive(false);
        MainPanel.SetActive(true);
    }

    float t;
    private void Update() {
        if(doTitleAnim){
            title_d.characterSpacing = Mathf.Lerp(40f,  -24f, t);
            title_ice.anchoredPosition = new Vector3(Mathf.Lerp(0, -120, t), title_ice.anchoredPosition.y);
            t += 0.4f * Time.deltaTime;
            if(t>1.0f){
                title_l.gameObject.SetActive(false);
                doTitleAnim = false;
                title_d.characterSpacing = -24f;
                title_ice.anchoredPosition = new Vector3(-120, title_ice.anchoredPosition.y);
            }
        }
    }
}
