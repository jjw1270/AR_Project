using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KKSpeech;
using TMPro;
using UnityEngine.UI;

/*
Mobile Speech Recognizer 에셋 필요
*/
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
            GameManager.Instance.logText.text = "이 기기는 음성인식을 지원하지 않습니다.";
        }

        recordingIcon.gameObject.SetActive(false);
        GameManager.Instance.logText.text = "음성인식 명령어 : '게임 설명', '게임 시작', '굴려', '게임 재시작', '어플 종료'";
    }

    public void OnFinalResult(string result){
        resultText.text = result;
        
        //명령함수 호출
        GameManager.Instance.Stt(result);
    }

    public void OnPartialResult(string result){
        resultText.text = result;
    }

    public void OnAvailabilityChange(bool available){
        recordingIcon.gameObject.SetActive(available);
        if(!available){
            GameManager.Instance.logText.text = "음성 인식 불가";
        }
        else{
            //resultText.text = "Say something :-)";
        }
    }

    public void OnAuthorizationStatusFetched(AuthorizationStatus status){
        switch(status){
            case AuthorizationStatus.Authorized:
                break;
            default:
                GameManager.Instance.logText.text = "Cannot use Speech Recognition, 승인 상태 : " + status;
                break;
        }
    }

    public void OnEndOfSpeech(){
        recordingIcon.gameObject.SetActive(false);
        Invoke("ShowCommandList", 3f);
    }

    public void OnError(string error){
        Debug.LogError(error);
        GameManager.Instance.logText.text = "음성 인식 오류";
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
            resultText.text = "";
        }
    }

    private void ShowCommandList(){
        GameManager.Instance.logText.text = "음성인식 명령어 : '게임 설명', '게임 시작', '어플 종료'";
        resultText.text = "";
    }
}
