using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif


public class GameManager : Singleton<GameManager>
{
    [SerializeField]SpeechToText speechToText;
    void Start()
    {
        #if PLATFORM_ANDROID
        //마이크 권한 요청
        if(!Permission.HasUserAuthorizedPermission(Permission.Microphone)){
            Permission.RequestUserPermission(Permission.Microphone);
        }
        #endif
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            //화면 터치 시 음성 녹음 시작 -> 자동으로 음성 녹음 종료, 해당하는 명령 실행
            speechToText.StartRecording();
        }

    }

    public void CommandExecution(string result){
        switch(result){
            case "안녕":
                //voiceController.StartSpeaking("반가워");
                break;
            case "도움말":
                //voiceController.StartSpeaking("그딴 건 없어 바보아ㅋㅋㅋㅋㅋ");
                break;
        }
    }
}
