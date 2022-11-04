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
            resultText.text = "Sorry, but this device doesn't support speech recognition";
        }

        recordingIcon.gameObject.SetActive(false);
    }

    public void OnFinalResult(string result){
        resultText.text = result;
        recordingIcon.gameObject.SetActive(false);
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
                resultText.text = "음성인식 서비스. 명령어 '도움말'";
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
        resultText.text = error;
    }

    public void StartRecording() {
        if(SpeechRecognizer.IsRecording()){
#if UNITY_IOS && !UNITY_EDITOR
            // SpeechRecognizer.StopIfRecording();
            // startRecordingButton.GetComponentInChildren<Text>().text = "Stopping";
            // startRecordingButton.enabled = false;
#elif UNITY_ANDROID && !UNITY_EDITOR
            SpeechRecognizer.StopIfRecording();
            SpeechRecognizer.StartRecording(true);
            recordingIcon.gameObject.SetActive(true);
            resultText.text = "Say something :-)";
#endif
        }
        else{
            SpeechRecognizer.StartRecording(true);
            recordingIcon.gameObject.SetActive(true);
            resultText.text = "Say something :-)";
        }
    }
}
