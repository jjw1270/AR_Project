using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]SpeechToText speechToText;
    [SerializeField]TextToSpeech textToSpeech;

    public TextMeshProUGUI logText;

    public void Stt(string command){
        switch(command){
            case "게임 설명":
                break;
            case "게임 시작":
                //모든 마커가 인식완료된 상태에서만 가능
                break;
            case "어플 종료":
                Application.Quit();
                break;
            default:
                logText.text = "잘못된 명령입니다.";
                break;
        }
    }

}
