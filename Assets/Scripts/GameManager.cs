using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif


public class GameManager : Singleton<GameManager>
{
    [SerializeField]SpeechToText speechToText;
    [SerializeField]TextToSpeech textToSpeech;
    void Start()
    {

    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            //화면 터치 시 음성 녹음 시작 -> 자동으로 음성 녹음 종료, 해당하는 명령 실행
            //이미 녹음 중인데 또 터치하면 동작 x
            speechToText.StartRecording();
        }
    }

    public void CommandExecution(string result){
        switch(result){
            case "안녕":
                textToSpeech.PlayTTS("");
                break;
            case "도움말":
                textToSpeech.PlayTTS("");
                break;
        }
    }
}
