using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KKSpeech;
using TMPro;
using UnityEngine.UI;

public class SpeechToText : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    [SerializeField]private Image recordingIcon;
    void Start() {
        SpeechRecognizer.SetDetectionLanguage("ko-KR");
        if(SpeechRecognizer.ExistsOnDevice()){
            SpeechRecognizerListener listener = GameObject.FindObjectOfType<SpeechRecognizerListener>();
            listener.onAuthorizationStatusFetched.AddListener(OnAuthorizationStatusFetched);
            listener.onAvailabilityChanged.AddListener(OnAvailabilityChange);
            listener.onErrorDuringRecording.AddListener(OnError);
            listener.onErrorOnStartRecording.AddListener(OnError);
            listener.onFinalResults.AddListener(OnFinalResult);
            listener.onPartialResults.AddListener(OnPartialResult);
            listener.onEndOfSpeech.AddListener(OnEndOfSpeech);
            SpeechRecognizer.RequestAccess();
        }
        else{
            resultText.text = "이 기기는 음성인식 서비스를 이용하실 수 없습니다.";
        }

        recordingIcon.gameObject.SetActive(false);
        resultText.text = "음성인식 서비스\n명령어 '도움말'\n지원 언어 '한국어'";
    }

    public void OnFinalResult(string result){
        resultText.text = result;
        
        //명령함수 호출
        GameManager.Instance.CommandExecution(result);
    }

    public void OnPartialResult(string result){
        resultText.text = result;
    }

    public void OnAvailabilityChange(bool available){
        recordingIcon.gameObject.SetActive(available);
        if(!available){
            resultText.text = "Speech Recognition not available";
        }
        else{
            resultText.text = "Say something :-)";
        }
    }

    public void OnAuthorizationStatusFetched(AuthorizationStatus status){
        switch(status){
            case AuthorizationStatus.Authorized:
                break;
            default:
                resultText.text = "Cannot use Speech Recognition, authorization status is " + status;
                break;
        }
    }

    public void OnEndOfSpeech(){
        recordingIcon.gameObject.SetActive(false);
    }

    public void OnError(string error){
        Debug.LogError(error);
        resultText.text = "음성 인식 오류";
        recordingIcon.gameObject.SetActive(false);
    }

    public void StartRecording() {
        if(SpeechRecognizer.IsRecording()){
#if UNITY_IOS && !UNITY_EDITOR
            // SpeechRecognizer.StopIfRecording();
            // startRecordingButton.GetComponentInChildren<Text>().text = "Stopping";
            // startRecordingButton.enabled = false;
#elif UNITY_ANDROID && !UNITY_EDITOR
            //SpeechRecognizer.StopIfRecording();

            //SpeechRecognizer.StartRecording(true);
            //recordingIcon.gameObject.SetActive(true);
            //resultText.text = "......";
#endif
        }
        else{
            SpeechRecognizer.StartRecording(true);
            recordingIcon.gameObject.SetActive(true);
            resultText.text = "......";
        }
    }
}
