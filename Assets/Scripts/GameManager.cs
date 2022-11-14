using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public SpeechToText speechToText;
    public TextToSpeech textToSpeech;
    public GameController gameController;

    public TextMeshProUGUI logText;

    public bool[] targetFoundList = new bool[3];

    public void Stt(string command){
        switch(command){
            case "게임 설명":
                break;
            case "게임 시작":
                //모든 마커가 인식완료된 상태에서만 가능
                if(targetFoundList[0] && targetFoundList[1] && targetFoundList[2]){
                    gameController.OnGameStart();
                }
                else{
                    logText.text = "모든 마커를 인식해 주세요.";
                }
                break;
            case "어플 종료":
                Application.Quit();
                break;
            case "공 이동":
                gameController.ScoreCtrl(true, 6);
                break;
            case "재시작":
                SceneManager.LoadScene("MainScene");
                break;
            default:
                logText.text = "잘못된 명령입니다.";
                break;
        }
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space)){
            Stt("공 이동");
        }
    }
}
