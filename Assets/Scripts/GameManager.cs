using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public SpeechToText speechToText;
    public GameController gameController;

    public TextMeshProUGUI logText;

    public bool[] targetFoundList = new bool[3];
    public bool isAllTargetFound;
    bool isGameStart;

    public void Stt(string command){
        switch(command){
            case "게임 설명":
                break;
            case "게임 시작":
                //모든 마커가 인식완료된 상태에서만 가능
                if(isAllTargetFound){
                    if(!isGameStart){
                        isGameStart = true;
                        SoundManager.Instance.PlayBGMSound();
                        gameController.OnGameStart();
                    }
                }
                else{
                    logText.text = "모든 마커를 인식해 주세요.";
                    Invoke("HelpLogText", 2f);
                }
                break;
            case "어플 종료":
                Invoke("QuitApplication", 2f);
                break;
            case "굴려":
                if(isGameStart){
                    if(!gameController.isThrowingDice){
                        gameController.ThrowDice();
                    }
                    else{
                        logText.text = "조금 뒤에 던져주세요";
                        Invoke("HelpLogText", 2f);
                    }
                }
                else{
                    logText.text = "게임을 시작해 주세요";
                    Invoke("HelpLogText", 2f);
                }
                
                break;
            case "게임 재시작":
                SceneManager.LoadScene("MainScene");
                break;
            default:
                logText.text = "잘못된 명령입니다.";
                Invoke("HelpLogText", 2f);
                break;
        }
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space)){
            Stt("굴려");
        }
        if(targetFoundList[0] && targetFoundList[1] && targetFoundList[2]){
            isAllTargetFound = true;
        }
        else{
            isAllTargetFound = false;
        }
    }

    private void HelpLogText(){
        logText.text = "음성인식 명령어 : '게임 설명', '게임 시작', '굴려', '게임 재시작', '어플 종료'";
    }

    private void QuitApplication(){
        Application.Quit();
    }
}
