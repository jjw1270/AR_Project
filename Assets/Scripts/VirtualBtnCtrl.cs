using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class VirtualBtnCtrl : MonoBehaviour
{
    private VirtualButtonBehaviour vb;
    [SerializeField]Renderer rderer;
    [SerializeField]Material defaultMat;
    [SerializeField]Material onPressedMat;
    private void Start() {
        vb = this.GetComponent<VirtualButtonBehaviour>();
        vb.RegisterOnButtonPressed(OnButtonPressed);
        vb.RegisterOnButtonReleased(OnButtonReleased);
    }

    public void OnButtonPressed(VirtualButtonBehaviour vb){
        Debug.Log(this.gameObject.name + " 버튼 눌림");
        SoundManager.Instance.textToSpeech.ttsAudioSource.Stop();
        ChangeBtnColor(true);
        SoundManager.Instance.PlaySFXSound("Button");
#if UNITY_EDITOR
        GameManager.Instance.Stt("게임 시작");
#elif UNITY_ANDROID
        GameManager.Instance.speechToText.StartRecording();
#endif
    }

    public void ChangeBtnColor(bool onPressed){
        if(onPressed){
            rderer.material = onPressedMat;
        }
        else{
            rderer.material = defaultMat;
        }   
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb){
        Debug.Log(this.gameObject.name + " 버튼 떼짐");
    }
}
